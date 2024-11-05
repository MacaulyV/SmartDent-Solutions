package com.daniel.smartdent

import android.content.Intent
import android.graphics.Color
import android.os.Bundle
import android.text.Spannable
import android.text.SpannableString
import android.text.style.ForegroundColorSpan
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.WindowCompat

class InicioActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        enableEdgeToEdge()

        setContentView(R.layout.activity_inicio)

        val textView = findViewById<TextView>(R.id.textTitulo)
        val spannable = SpannableString("SmartDent")

        val blueColor = ForegroundColorSpan(Color.parseColor("#00bfff"))
        spannable.setSpan(blueColor, 0, 5, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)

        val pinkColor = ForegroundColorSpan(Color.parseColor("#ff1493"))
        spannable.setSpan(pinkColor, 5, spannable.length, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)
        textView.text = spannable

        val btnCadastrar = findViewById<Button>(R.id.btnCadastrar)
        val btnLogin = findViewById<Button>(R.id.btnLogin)

        btnCadastrar.setOnClickListener {
            val intent = Intent(this, CadastroActivity::class.java)
            startActivity(intent)
        }

        btnLogin.setOnClickListener {
            val intent = Intent(this, LoginActivity::class.java)
            startActivity(intent)
        }
    }

    private fun enableEdgeToEdge() {
        WindowCompat.setDecorFitsSystemWindows(window, false)
    }
}
