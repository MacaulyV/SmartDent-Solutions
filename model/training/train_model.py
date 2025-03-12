import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score, classification_report
from joblib import dump
import os

def load_dataset(csv_path):
    # Função pra carregar o dataset do arquivo CSV e remover colunas que não servem pra treinar o modelo.
    df = pd.read_csv(csv_path)

    # Aqui removo algumas colunas textuais que não vão ajudar no aprendizado de máquina
    cols_to_drop = ["dataMin_str", "dataMax_str", "procedimentosRepetidos_str"]
    df = df.drop(columns=[col for col in cols_to_drop if col in df.columns], errors="ignore")

    return df

def preprocess_data(df):
    # Converto a coluna gastoTotal, que pode estar no formato de string, pra float (tirando símbolos de R$ e etc).
    if df['gastoTotal'].dtype == object:
        df['gastoTotal'] = df['gastoTotal'].str.replace(r"R\$", "", regex=True) \
            .str.replace(r"\.", "", regex=True) \
            .str.replace(",", ".", regex=True) \
            .astype(float)

    # Separo a base de dados em features (X) e o alvo (y), que no caso é a coluna 'label'
    features = df.drop(columns=["idPaciente", "label"])
    target = df["label"]
    return features, target

def train_model(X, y):
    # Divido a base entre treino (80%) e teste (20%), pra avaliar se o modelo generaliza bem
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

    # Aqui eu uso um RandomForestClassifier, que costuma dar bons resultados em muitas situações
    clf = RandomForestClassifier(n_estimators=100, random_state=42)
    clf.fit(X_train, y_train)

    # Testo no conjunto de teste e vejo acurácia e relatório de classificação
    y_pred = clf.predict(X_test)
    acc = accuracy_score(y_test, y_pred)
    print("Acurácia no teste: {:.2f}%".format(acc * 100))
    print("\nRelatório de Classificação:\n", classification_report(y_test, y_pred, zero_division=0))

    return clf

def main():
    # Localizo o CSV e carrego o dataset
    csv_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "data", "dataset_treino.csv")
    df = load_dataset(csv_path)
    print("Dataset carregado. Número de registros:", len(df))

    # Preparo as variáveis e os rótulos
    X, y = preprocess_data(df)
    print("Número de features:", X.shape[1])

    # Treino o modelo e mostro métricas
    model = train_model(X, y)

    # Salvo o modelo treinado em um arquivo .joblib
    output_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "model_rf.joblib")
    dump(model, output_path)
    print("Modelo salvo em:", output_path)

if __name__ == "__main__":
    main()
