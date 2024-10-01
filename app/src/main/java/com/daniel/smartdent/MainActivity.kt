package com.daniel.smartdent

import android.annotation.SuppressLint
import android.content.Intent
import android.graphics.Color
import android.os.Bundle
import android.text.Spannable
import android.text.SpannableString
import android.text.style.ForegroundColorSpan
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast

class MainActivity : AppCompatActivity() {
    @SuppressLint("MissingInflatedId")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)


        val textView = findViewById<TextView>(R.id.textView2)
        val button = findViewById<Button>(R.id.btnButton)
        val imageView = findViewById<ImageView>(R.id.imgLogo)

        val spannable = SpannableString("SmartDent")

        val blueColor = ForegroundColorSpan(Color.parseColor("#00bfff"))
        spannable.setSpan(blueColor, 0, 5, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)

        val pinkColor = ForegroundColorSpan(Color.parseColor("#ff1493"))
        spannable.setSpan(pinkColor, 5, spannable.length, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)

        button.setOnClickListener {
            val intent = Intent(this, TelaInicio::class.java)
            startActivity(intent)
        }

        textView.text = spannable

    }
}
