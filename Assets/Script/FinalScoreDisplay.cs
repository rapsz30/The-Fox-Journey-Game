using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreCherryText;
    public TextMeshProUGUI scoreGemsText;

    void Start()
    {
        int cherryScore = GameManager.finalCherryScore;
        int gemScore = QuestionSceneManager.finalGemScore;

        if (scoreCherryText != null)
            scoreCherryText.text = cherryScore.ToString();

        if (scoreGemsText != null)
            scoreGemsText.text = gemScore.ToString();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("HomePage");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}