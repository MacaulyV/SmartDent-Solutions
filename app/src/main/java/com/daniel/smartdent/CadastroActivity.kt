package com.daniel.smartdent

import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.util.Patterns
import android.widget.Button
import android.widget.CheckBox
import android.widget.EditText
import android.widget.ImageButton
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.firestore.FirebaseFirestore

class CadastroActivity : AppCompatActivity() {

    private lateinit var auth: FirebaseAuth

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_cadastro)

        auth = FirebaseAuth.getInstance()
        val db = FirebaseFirestore.getInstance()

        val nameInput = findViewById<EditText>(R.id.name)
        val emailInput = findViewById<EditText>(R.id.email)
        val phoneInput = findViewById<EditText>(R.id.phone)
        val passwordInput = findViewById<EditText>(R.id.senha)
        val termsCheckbox = findViewById<CheckBox>(R.id.checkbox_terms)
        val btnCadastrar = findViewById<Button>(R.id.btnCadastrar)

        btnCadastrar.setOnClickListener {
            val nome = nameInput.text.toString().trim()
            val email = emailInput.text.toString().trim()
            val telefone = phoneInput.text.toString().trim()
            val senha = passwordInput.text.toString().trim()

            if (nome.isEmpty()) {
                nameInput.error = "Nome é obrigatório"
                nameInput.requestFocus()
                return@setOnClickListener
            }

            if (email.isEmpty() || !Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                emailInput.error = "E-mail inválido"
                emailInput.requestFocus()
                return@setOnClickListener
            }

            if (telefone.isEmpty() || telefone.length < 9 || !telefone.matches(Regex("^[0-9]+\$"))) {
                phoneInput.error = "Telefone inválido! Deve conter pelo menos 9 dígitos numéricos."
                phoneInput.requestFocus()
                return@setOnClickListener
            }

            if (senha.isEmpty() || senha.length < 6) {
                passwordInput.error = "A senha deve ter no mínimo 6 caracteres"
                passwordInput.requestFocus()
                return@setOnClickListener
            }

            if (!termsCheckbox.isChecked) {
                Toast.makeText(
                    this,
                    "É necessário aceitar os termos e condições.",
                    Toast.LENGTH_SHORT
                ).show()
                termsCheckbox.requestFocus()
                return@setOnClickListener
            }

            auth.createUserWithEmailAndPassword(email, senha)
                .addOnCompleteListener(this) { task ->
                    if (task.isSuccessful) {
                        val userId = auth.currentUser?.uid
                        val user = hashMapOf(
                            "nome" to nome,
                            "email" to email,
                            "telefone" to telefone,
                            "senha" to senha
                        )

                        userId?.let {
                            db.collection("usuarios").document(it)
                                .set(user)
                                .addOnSuccessListener {
                                    Toast.makeText(
                                        this,
                                        "Dados salvos no Firestore!",
                                        Toast.LENGTH_LONG
                                    ).show()
                                    // Redireciona para MenuActivity
                                    val intent = Intent(this, MenuActivity::class.java)
                                    startActivity(intent)
                                    finish()
                                }
                                .addOnFailureListener { e ->
                                    Toast.makeText(
                                        this,
                                        "Erro ao salvar no Firestore: ${e.message}",
                                        Toast.LENGTH_LONG
                                    ).show()
                                }
                        }

                        val sharedPref = getSharedPreferences("userData", Context.MODE_PRIVATE)
                        with(sharedPref.edit()) {
                            putString("nome", nome)
                            putString("email", email)
                            putString("telefone", telefone)
                            putString("senha", senha)
                            apply()
                        }
                    } else {
                        Toast.makeText(
                            this,
                            "Erro no cadastro: ${task.exception?.message}",
                            Toast.LENGTH_LONG
                        ).show()
                    }
                }
        }

        val btnVoltarInicio = findViewById<ImageButton>(R.id.btnVoltarInicio)
        btnVoltarInicio.setOnClickListener {
            val intent = Intent(this, InicioActivity::class.java)
            startActivity(intent)
            finish()
        }
    }
}
