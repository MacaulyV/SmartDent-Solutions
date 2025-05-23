import os
import sys
import pandas as pd
import numpy as np
import joblib
from joblib import dump
from datetime import datetime
import warnings
from sklearn.ensemble import RandomForestClassifier
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import classification_report, accuracy_score
from sklearn.model_selection import cross_val_score, StratifiedKFold, train_test_split
from sklearn.calibration import CalibratedClassifierCV

warnings.filterwarnings('ignore')

# garante que o módulo prepare_dataset seja encontrado
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '../..')))
from model.preprocessing.prepare_dataset import load_dataset_from_ndjson

def preprocess_data(df):
    """
    Prepara os dados para treinamento:
    - Drop de colunas irrelevantes de texto
    - One-hot encoding de 'plano' e 'tipoPlano'
    - Conversão de object → numérico
    - Remoção de NaNs
    - Feature engineering derivada
    - Label encoding
    """
    # remover texto irrelevante
    drop_cols = ['cpf', 'dataNascimento', 'email', 'telefone', 'endereco', 'cidade', 'empresa']
    drop_cols = [c for c in drop_cols if c in df.columns]
    if drop_cols:
        print(f"Removendo colunas de texto: {drop_cols}")
        df = df.drop(columns=drop_cols)

    # one-hot de plano
    if 'plano' in df.columns:
        d1 = pd.get_dummies(df['plano'], prefix='plano')
        df = pd.concat([df, d1], axis=1).drop(columns=['plano'])

    # one-hot de tipoPlano
    if 'tipoPlano' in df.columns:
        d2 = pd.get_dummies(df['tipoPlano'], prefix='tipoPlano')
        df = pd.concat([df, d2], axis=1).drop(columns=['tipoPlano'])

    # converter qualquer object que não seja label/id para numérico
    for col in df.select_dtypes(include=['object']).columns:
        if col not in ['label', 'idPaciente']:
            print(f"Convertendo coluna {col} para numérico")
            df[col] = pd.to_numeric(df[col], errors='coerce')

    # drop NaNs
    before = df.shape[0]
    df = df.dropna()
    after = df.shape[0]
    print(f"Removidas {before - after} linhas com NaN")

    # FEATURE ENGINEERING
    df['taxa_realizacao'] = df['qtd_realizadas'] / (df['periodoTotalDias'] + 1)
    df['custo_medio_consulta'] = df['gastoTotal'] / (df['qtd_realizadas'] + 1)
    df['taxa_cancelamento'] = df['qtd_canceladas'] / (df['qtd_agendadas'] + 1)
    df['proceds_repetidos_por_dia'] = df['numProcedsRepetidos'] / (df['periodoTotalDias'] + 1)

    if 'count_Endodontia' in df.columns and 'count_PrótesesDentárias' in df.columns:
        df['count_reabilitacao'] = df['count_Endodontia'] + df['count_PrótesesDentárias']
    else:
        df['count_reabilitacao'] = 0
        print("Aviso: colunas de reabilitação ausentes")

    if 'count_Ortodontia' in df.columns:
        df['flag_muito_ortodontia'] = (df['count_Ortodontia'] > 2).astype(int)
    else:
        df['flag_muito_ortodontia'] = 0
        print("Aviso: coluna count_Ortodontia ausente")

    # label encoding
    le = LabelEncoder()
    df['label_encoded'] = le.fit_transform(df['label'])
    mapping = dict(zip(le.classes_, le.transform(le.classes_)))
    print(f"Mapeamento de classes: {mapping}")

    # selecionar features
    feature_cols = [c for c in df.columns if c not in ['idPaciente', 'label', 'label_encoded']]
    X = df[feature_cols]
    y = df['label_encoded']
    return X, y, le

def train_model(X, y, label_encoder=None):
    print("\n==== TREINAMENTO DO MODELO ====")
    print(f"Total de amostras: {len(X)}")
    if label_encoder:
        counts = pd.Series(y).value_counts().sort_index()
        for idx, cnt in counts.items():
            print(f"  {label_encoder.inverse_transform([idx])[0]}: {cnt}")

    # adicionar ruído leve
    print("\nAdicionando ruído controlado...")
    for col in ['gastoTotal', 'qtd_realizadas', 'qtd_agendadas', 'qtd_canceladas']:
        if col in X.columns:
            scale = X[col].mean() * 0.05
            X[col] += np.random.normal(0, scale, size=len(X))
            print(f"  Ruído em {col}")

    # split 80/20 sem SMOTE
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, stratify=y, random_state=42
    )
    print(f"Treino: {X_train.shape[0]}, Teste: {X_test.shape[0]}")

    # configurar RandomForest robusta
    rf = RandomForestClassifier(
        n_estimators=300,
        max_depth=18,
        min_samples_split=2,
        min_samples_leaf=1,
        max_features='sqrt',
        class_weight='balanced',
        random_state=42,
        n_jobs=-1
    )
    rf.fit(X_train, y_train)

    # cross-validation
    print("\nValidação cruzada:")
    cv = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    scores = cross_val_score(rf, X_train, y_train, cv=cv, scoring='accuracy', n_jobs=-1)
    print(f"  Accuracy: {scores.mean():.4f} ± {scores.std():.4f}")

    # calibragem isotônica
    print("\nCalibrando (Isotonic Regression)...")
    calib = CalibratedClassifierCV(rf, cv=5, method='isotonic', n_jobs=-1)
    calib.fit(X_train, y_train)

    # avaliação final
    y_pred = calib.predict(X_test)
    acc = accuracy_score(y_test, y_pred)
    print(f"\nAcurácia no teste: {acc * 100:.2f}%")
    if label_encoder:
        y_true = label_encoder.inverse_transform(y_test)
        y_pr = label_encoder.inverse_transform(y_pred)
        print("\nRelatório de classificação:")
        print(classification_report(y_true, y_pr, zero_division=0))

    # importância de features
    print("\nTop 10 features:")
    importances = rf.feature_importances_
    names = X.columns
    for i in np.argsort(importances)[::-1][:10]:
        print(f"  {names[i]}: {importances[i]:.4f}")

    # guardar lista de features no objeto calibrado
    calib.feature_names = list(X.columns)
    return calib

def main():
    print("==== START TRAINING ====")
    print(datetime.now().strftime("%d/%m/%Y %H:%M:%S"))

    # definir caminhos
    base = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
    ndjson_path = os.path.join(base, "data", "synthetic_patients.ndjson")
    if not os.path.exists(ndjson_path):
        print(f"ERRO: arquivo não encontrado em {ndjson_path}")
        return

    # carregar e preprocessar
    df = load_dataset_from_ndjson(ndjson_path)
    X, y, le = preprocess_data(df)
    print(f"Features selecionadas ({X.shape[1]}): {X.columns.tolist()}")

    # treinar modelo
    model = train_model(X, y, le)
    if model is None:
        print("Falha no treinamento")
        return

    # salvar artefatos
    art = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "artifacts")
    os.makedirs(art, exist_ok=True)
    dump(model, os.path.join(art, "model_rf.joblib"))
    dump(le, os.path.join(art, "label_encoder.joblib"))

    # salvar lista de features em txt
    feat_file = os.path.join(art, "feature_names.txt")
    with open(feat_file, 'w', encoding='utf-8') as f:
        f.write("\n".join(model.feature_names))
    print(f"Features salvas em: {feat_file}")

    print("Modelos e artefatos salvos em:", art)
    print("==== DONE ====")

if __name__ == "__main__":
    main()
