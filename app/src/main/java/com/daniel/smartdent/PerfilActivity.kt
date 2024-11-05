package com.daniel.smartdent

import android.content.Intent
import android.os.Bundle
import android.widget.ImageButton
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class PerfilActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_perfil)

        val nome = intent.getStringExtra("nome") ?: "Nome não informado"
        val email = intent.getStringExtra("email") ?: "Email não informado"
        val telefone = intent.getStringExtra("telefone") ?: "Telefone não informado"
        val senha = intent.getStringExtra("senha") ?: "Senha não informada"

        val tvNome = findViewById<TextView>(R.id.tv_user_name)
        val tvEmail = findViewById<TextView>(R.id.tv_email_value)
        val tvTelefone = findViewById<TextView>(R.id.tv_phone_value)
        val tvSenha = findViewById<TextView>(R.id.tv_password_value)

        tvNome.text = nome
        tvEmail.text = email
        tvTelefone.text = telefone
        tvSenha.text = senha

        val btnVoltarCadastro = findViewById<ImageButton>(R.id.btnVoltarCadastro)
        btnVoltarCadastro.setOnClickListener {
            val intent = Intent(this, MenuActivity::class.java)
            startActivity(intent)
            finish()
        }
    }
}
