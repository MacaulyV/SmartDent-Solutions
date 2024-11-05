package com.daniel.smartdent

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.ImageButton
import android.widget.TextView
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.firestore.FirebaseFirestore

class LoginActivity : AppCompatActivity() {

    private lateinit var auth: FirebaseAuth
    private val db = FirebaseFirestore.getInstance()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        auth = FirebaseAuth.getInstance()

        val emailInput = findViewById<EditText>(R.id.username)
        val passwordInput = findViewById<EditText>(R.id.password)
        val btnEntrar = findViewById<Button>(R.id.btnEntrar)
        val btnCriarConta: TextView = findViewById(R.id.btnCriarConta)

        btnCriarConta.setOnClickListener {
            val intent = Intent(this, CadastroActivity::class.java)
            startActivity(intent)
        }

        btnEntrar.setOnClickListener {
            val email = emailInput.text.toString().trim()
            val password = passwordInput.text.toString().trim()

            if (email.isEmpty()) {
                showAlertDialog("Erro", "O campo de e-mail é obrigatório.")
                emailInput.requestFocus()
                return@setOnClickListener
            }

            if (password.isEmpty()) {
                showAlertDialog("Erro", "O campo de senha é obrigatório.")
                passwordInput.requestFocus()
                return@setOnClickListener
            }

            auth.signInWithEmailAndPassword(email, password)
                .addOnCompleteListener(this) { task ->
                    if (task.isSuccessful) {
                        db.collection("usuarios")
                            .whereEqualTo("email", email)
                            .get()
                            .addOnSuccessListener { documents ->
                                if (!documents.isEmpty) {
                                    for (document in documents) {
                                        val nome = document.getString("nome") ?: "Nome não disponível"
                                        val telefone = document.getString("telefone") ?: "Telefone não disponível"
                                        val senha = document.getString("senha") ?: "Senha não disponível"
                                        
                                        showAlertDialog("Sucesso", "Login realizado com sucesso!") {
                                            val intentMenu = Intent(this, MenuActivity::class.java)
                                            startActivity(intentMenu)
                                            finish()
                                        }
                                    }
                                } else {
                                    showAlertDialog("Erro", "Usuário não encontrado.")
                                }
                            }
                            .addOnFailureListener { e ->
                                showAlertDialog("Erro", "Falha ao buscar dados do usuário: ${e.message}")
                            }
                    } else {
                        showAlertDialog("Erro de Login", "Falha no login: ${task.exception?.message}")
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

    private fun showAlertDialog(titulo: String, mensagem: String, acaoPositiva: (() -> Unit)? = null) {
        val builder = AlertDialog.Builder(this)
        builder.setTitle(titulo)
        builder.setMessage(mensagem)
        builder.setCancelable(false)
        builder.setPositiveButton("OK") { dialog, _ ->
            dialog.dismiss()
            acaoPositiva?.invoke()
        }
        builder.create().show()
    }
}
