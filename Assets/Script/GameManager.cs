using UnityEngine;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.SceneManagement; 
public class GameManager : MonoBehaviour
{
    private MusicButtonController musicButtonController;
    public List<GameObject> livesUI; 
    
    public TextMeshProUGUI scoreText; 
    public GameObject pausePanel; 
    public GameObject pauseButtonIcon;
    private bool isPaused = false; 
    public GameObject gameOverPanel; 

    private int score = 0;

    void Awake() 
    {Time.timeScale = 1f; 
    }

    void Start()
    {
        musicButtonController = FindObjectOfType<MusicButtonController>();
        UpdateScoreUI();
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        if (gameOverPanel != null) 
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
        for (int i = 0; i < livesUI.Count; i++)
        {
            livesUI[i].SetActive(false);
        }
        for (int i = 0; i < currentHealth; i++)
        {
            if (i < livesUI.Count)
            {
                livesUI[i].SetActive(true);
            }
        }
    }
    public void TogglePause()
    {
        if (Time.timeScale == 0f && !isPaused) 
        {
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            if (pauseButtonIcon != null) 
            {
                pauseButtonIcon.SetActive(false);
            }

            Time.timeScale = 0f; 
        }
        else
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            if (pauseButtonIcon != null) 
            {
                pauseButtonIcon.SetActive(true);
            }
            
            Time.timeScale = 1f; 
        }
    }
    public void GameOver()
    {
        Debug.Log("GAME OVER! Fox has died.");

        Time.timeScale = 0f; 

        if (gameOverPanel != null)
        {
            if (pausePanel != null && pausePanel.activeSelf)
            {
                 pausePanel.SetActive(false);
            }
            if (pauseButtonIcon != null)
            {
                pauseButtonIcon.SetActive(false);
            }
            
            gameOverPanel.SetActive(true);
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); 
    }

    public void ToggleMute()
    {
       AudioListener.volume = (AudioListener.volume == 0) ? 1 : 0;
    
       if (musicButtonController != null)
       {
           musicButtonController.UpdateSprite(AudioListener.volume > 0);
       }
    }
}