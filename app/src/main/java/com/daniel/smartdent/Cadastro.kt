package com.daniel.smartdent

import android.os.Bundle
import android.widget.Button
import android.widget.CheckBox
import android.widget.EditText
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class Cadastro : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_cadastro)

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

            if (nome.isEmpty() || !nome.matches(Regex("^[a-zA-ZÀ-ÿ\\s]+\$"))) {
                nameInput.error = "Nome inválido! Use apenas letras."
                nameInput.requestFocus()
                return@setOnClickListener
            }

            if (email.isEmpty() || !android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                emailInput.error = "E-mail inválido!"
                emailInput.requestFocus()
                return@setOnClickListener
            }

            if (telefone.isEmpty() || !telefone.matches(Regex("^[0-9]{9,}\$"))) {
                phoneInput.error = "Número de telefone inválido! Deve ter no mínimo 9 dígitos."
                phoneInput.requestFocus()
                return@setOnClickListener
            }

            if (senha.isEmpty() || senha.length < 6) {
                passwordInput.error = "Senha inválida! Deve conter no mínimo 6 caracteres."
                passwordInput.requestFocus()
                return@setOnClickListener
            }

            if (!termsCheckbox.isChecked) {
                Toast.makeText(
                    this,
                    "Você precisa aceitar os termos e condições.",
                    Toast.LENGTH_SHORT
                ).show()
                return@setOnClickListener
            }

            Toast.makeText(this, "Cadastro realizado com sucesso!", Toast.LENGTH_LONG).show()
        }
    }
}
