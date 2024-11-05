package com.daniel.smartdent

import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.widget.ImageButton
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.cardview.widget.CardView
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.firestore.FirebaseFirestore

class MenuActivity : AppCompatActivity() {

    private val db = FirebaseFirestore.getInstance()
    private lateinit var auth: FirebaseAuth

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_menu)

        auth = FirebaseAuth.getInstance()

        val btnBack: ImageButton = findViewById(R.id.btnVoltar)
        btnBack.setOnClickListener {
            finish()
        }

        val currentUser = auth.currentUser
        if (currentUser != null) {
            val userEmail = currentUser.email

            // Busca os dados do Firestore
            if (userEmail != null) {
                db.collection("usuarios").whereEqualTo("email", userEmail).get()
                    .addOnSuccessListener { documents ->
                        if (!documents.isEmpty) {
                            for (document in documents) {
                                val nome = document.getString("nome") ?: "Nome não disponível"
                                val telefone = document.getString("telefone") ?: "Telefone não disponível"
                                val senha = document.getString("senha") ?: "Senha não disponível"

                                val sharedPref = getSharedPreferences("userData", Context.MODE_PRIVATE)
                                with(sharedPref.edit()) {
                                    putString("nome", nome)
                                    putString("email", userEmail)
                                    putString("telefone", telefone)
                                    putString("senha", senha)
                                    apply()
                                }
                            }
                        } else {
                            Toast.makeText(this, "Dados do usuário não encontrados.", Toast.LENGTH_SHORT).show()
                        }
                    }
                    .addOnFailureListener { e ->
                        Toast.makeText(this, "Erro ao buscar dados: ${e.message}", Toast.LENGTH_SHORT).show()
                    }
            }
        } else {
            Toast.makeText(this, "Usuário não autenticado.", Toast.LENGTH_SHORT).show()
        }

        val sharedPref = getSharedPreferences("userData", Context.MODE_PRIVATE)
        val nome = sharedPref.getString("nome", "Nome não disponível")
        val email = sharedPref.getString("email", "Email não disponível")
        val telefone = sharedPref.getString("telefone", "Telefone não disponível")
        val senha = sharedPref.getString("senha", "Senha não disponível")

        val cardPerfil: CardView = findViewById(R.id.card_perfil)
        cardPerfil.setOnClickListener {
            val intentPerfil = Intent(this, PerfilActivity::class.java)
            intentPerfil.putExtra("nome", nome)
            intentPerfil.putExtra("email", email)
            intentPerfil.putExtra("telefone", telefone)
            intentPerfil.putExtra("senha", senha)
            startActivity(intentPerfil)
        }
    }
}
