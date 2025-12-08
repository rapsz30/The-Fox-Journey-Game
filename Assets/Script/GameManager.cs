using UnityEngine;
using System.Collections.Generic;
using TMPro; // Wajib untuk TextMesh Pro

public class GameManager : MonoBehaviour
{
    // Seret Life, Life (1), dan Life (2) ke list ini di Inspector
    public List<GameObject> livesUI; 
    
    // Seret komponen TextMeshProUGUI dari objek Canvas Score ke slot ini
    public TextMeshProUGUI scoreText; 

    private int score = 0;

    void Start()
    {
        // Pastikan UI score terinisialisasi
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Update teks Score
            scoreText.text = "| Score: " + score.ToString(); 
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        // Nonaktifkan semua ikon Life yang ada di list
        for (int i = 0; i < livesUI.Count; i++)
        {
            livesUI[i].SetActive(false);
        }

        // Aktifkan ikon Life sesuai dengan sisa health Fox
        for (int i = 0; i < currentHealth; i++)
        {
            if (i < livesUI.Count)
            {
                livesUI[i].SetActive(true);
            }
        }
    }
    
    public void GameOver()
    {
        Debug.Log("GAME OVER! Fox has died.");
        // Hentikan waktu game (membuat game beku/freeze)
        Time.timeScale = 0f; 
        
        // Tambahkan kode untuk menampilkan UI Game Over di sini.
        // Untuk melanjutkan game, Anda HARUS memanggil Time.timeScale = 1f;
    }
}