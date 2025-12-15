using UnityEngine;
using System.Collections.Generic;
using TMPro; // Wajib untuk TextMesh Pro
using UnityEngine.SceneManagement; // Wajib untuk fungsi Restart dan mengakses Scene
//loww
public class GameManager : MonoBehaviour
{
    private MusicButtonController musicButtonController;
    // Seret Life, Life (1), dan Life (2) ke list ini di Inspector
    public List<GameObject> livesUI; 
    
    // Seret komponen TextMeshProUGUI dari objek Canvas Score ke slot ini
    public TextMeshProUGUI scoreText; 
    
    // --- VARIABEL PAUSE ---
    public GameObject pausePanel; 
    public GameObject pauseButtonIcon;
    private bool isPaused = false; 
    
    // --- VARIABEL GAME OVER BARU ---
    // Seret objek GameOverPanel (dari Hierarchy) ke slot ini
    public GameObject gameOverPanel; //

    private int score = 0;

    void Awake() 
    {
        // Pastikan game dimulai dengan kecepatan normal
        Time.timeScale = 1f; 
    }

    void Start()
    {
        // Mendapatkan referensi MusicButtonController di Start
        musicButtonController = FindObjectOfType<MusicButtonController>();
        
        // Pastikan UI score terinisialisasi
        UpdateScoreUI();
        
        // Sembunyikan kedua panel di awal
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        if (gameOverPanel != null) // <-- LOGIKA BARU
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
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
    
    // Dipanggil oleh Button Pause (ikon) dan Button Resume
    public void TogglePause()
    {
        // Cegah toggle jika sudah Game Over (TimeScale = 0 dan bukan isPaused)
        if (Time.timeScale == 0f && !isPaused) 
        {
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            // 1. Tampilkan Panel Pause
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            
            // 2. Sembunyikan Ikon Pause
            if (pauseButtonIcon != null) 
            {
                pauseButtonIcon.SetActive(false);
            }

            Time.timeScale = 0f; 
        }
        else
        {
            // 1. Sembunyikan Panel Pause
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            
            // 2. Tampilkan Ikon Pause
            if (pauseButtonIcon != null) 
            {
                pauseButtonIcon.SetActive(true);
            }
            
            Time.timeScale = 1f; 
        }
    }

    // Dipanggil dari skrip Fox.cs saat HP <= 0
    public void GameOver()
    {
        Debug.Log("GAME OVER! Fox has died.");
        
        // 1. Hentikan waktu game (penting agar Fox beku)
        Time.timeScale = 0f; 
        
        // 2. Tampilkan Panel Game Over
        if (gameOverPanel != null)
        {
            // Pastikan panel pause disembunyikan jika Game Over terjadi saat pause
            if (pausePanel != null && pausePanel.activeSelf)
            {
                 pausePanel.SetActive(false);
            }
            // Sembunyikan ikon pause di Game Over
            if (pauseButtonIcon != null)
            {
                pauseButtonIcon.SetActive(false);
            }
            
            gameOverPanel.SetActive(true);
        }
    }

    // Dipanggil oleh Button Restart (Di Menu Pause/Game Over)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset waktu sebelum pindah scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // Dipanggil oleh Button Quit (Di Menu Pause/Game Over)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); 
    }
    
    // Dipanggil oleh Button Mute/Unmute
    public void ToggleMute()
    {
       AudioListener.volume = (AudioListener.volume == 0) ? 1 : 0;
    
       if (musicButtonController != null)
       {
           musicButtonController.UpdateSprite(AudioListener.volume > 0);
       }
    }
}