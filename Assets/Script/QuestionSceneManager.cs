using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionSceneManager : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup questionPanel;
    public CanvasGroup pausePanel;
    public CanvasGroup gameOverPanel;

    [Header("Texts (TMP)")]
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answerText;
    public TextMeshProUGUI scoreText;

    [Header("Buttons")]
    public Button buttonTrue;
    public Button buttonFalse;
    public GameObject buttonPause;
    public Button musicButton;

    [Header("Audio")]
    public AudioSource bgmSource;

    [Header("Story")]
    [TextArea] public string openingStory;

    [Header("Ending Story")]
    [TextArea] public string endingStoryWin;
    [TextArea] public string endingStoryLose;

    [Header("Questions")]
    public QuestionData[] questions;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.035f;
    public float commaDelayMultiplier = 3f;
    public float dotDelayMultiplier = 6f;
    public float specialDelayMultiplier = 5f;

    int currentIndex = 0;
    int score = 0;
    bool isTyping = false;
    bool musicOn = true;

    void Start()
    {
        ShowOnly(questionPanel);
        HideAllTexts();
        StartCoroutine(PlayOpeningStory());
    }

    // ================= PANEL CONTROL =================

    void ShowOnly(CanvasGroup panel)
    {
        SetPanel(questionPanel, false);
        SetPanel(pausePanel, false);
        SetPanel(gameOverPanel, false);

        SetPanel(panel, true);
    }

    void SetPanel(CanvasGroup panel, bool show)
    {
        if (panel == null) return;
        panel.alpha = show ? 1 : 0;
        panel.interactable = show;
        panel.blocksRaycasts = show;
    }

    // ================= STORY =================

    IEnumerator PlayOpeningStory()
    {
        storyText.gameObject.SetActive(true);
        yield return StartCoroutine(TypeWriter(storyText, openingStory));
        yield return new WaitForSeconds(0.5f);

        storyText.gameObject.SetActive(false);
        StartCoroutine(ShowQuestion());
    }

    IEnumerator PlayEndingStory()
    {
        HideQuestionUI();
        storyText.gameObject.SetActive(true);

        if (score > 6)
        {
            yield return StartCoroutine(TypeWriter(storyText, endingStoryWin));
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("FinalScore");
        }
        else
        {
            yield return StartCoroutine(TypeWriter(storyText, endingStoryLose));
            yield return new WaitForSeconds(0.5f);
            storyText.gameObject.SetActive(false);
            ShowOnly(gameOverPanel);
        }
    }

    // ================= QUESTION FLOW =================

    IEnumerator ShowQuestion()
    {
        if (currentIndex >= questions.Length)
        {
            StartCoroutine(PlayEndingStory());
            yield break;
        }

        questionText.gameObject.SetActive(true);
        answerText.gameObject.SetActive(false);
        buttonTrue.gameObject.SetActive(false);
        buttonFalse.gameObject.SetActive(false);

        yield return StartCoroutine(TypeWriter(
            questionText,
            questions[currentIndex].question
        ));

        buttonTrue.gameObject.SetActive(true);
        buttonFalse.gameObject.SetActive(true);

        buttonTrue.onClick.RemoveAllListeners();
        buttonFalse.onClick.RemoveAllListeners();

        buttonTrue.onClick.AddListener(() => OnAnswer(true));
        buttonFalse.onClick.AddListener(() => OnAnswer(false));
    }

    void OnAnswer(bool playerAnswer)
    {
        if (isTyping) return;

        if (playerAnswer == questions[currentIndex].correctAnswer)
        {
            score++;
            scoreText.text = "Score:      x " + score;
        }

        StartCoroutine(ShowAnswer());
    }

    IEnumerator ShowAnswer()
    {
        isTyping = true;

        questionText.gameObject.SetActive(false);
        buttonTrue.gameObject.SetActive(false);
        buttonFalse.gameObject.SetActive(false);

        answerText.gameObject.SetActive(true);
        yield return StartCoroutine(TypeWriter(
            answerText,
            questions[currentIndex].answerExplanation
        ));

        yield return new WaitForSeconds(0.8f);
        answerText.gameObject.SetActive(false);

        currentIndex++;
        isTyping = false;

        StartCoroutine(ShowQuestion());
    }

    // ================= MUSIC =================

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        bgmSource.mute = !musicOn;
    }

    // ================= PAUSE =================

    public void PauseGame()
    {
        buttonPause.SetActive(false);
        ShowOnly(pausePanel);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        buttonPause.SetActive(true);
        ShowOnly(questionPanel);
    }

    // ================= GAME OVER =================

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomePage");
    }

    // ================= UTIL =================

    void HideAllTexts()
    {
        storyText.gameObject.SetActive(false);
        questionText.gameObject.SetActive(false);
        answerText.gameObject.SetActive(false);
        buttonTrue.gameObject.SetActive(false);
        buttonFalse.gameObject.SetActive(false);
    }

    void HideQuestionUI()
    {
        questionText.gameObject.SetActive(false);
        answerText.gameObject.SetActive(false);
        buttonTrue.gameObject.SetActive(false);
        buttonFalse.gameObject.SetActive(false);
    }

    // ================= TYPEWRITER (IMPROVED) =================

    IEnumerator TypeWriter(TextMeshProUGUI textUI, string text)
    {
        isTyping = true;
        textUI.text = "";

        foreach (char c in text)
        {
            textUI.text += c;

            float delay = typingSpeed;

            if (c == ',')
                delay *= commaDelayMultiplier;
            else if (c == '.')
                delay *= dotDelayMultiplier;
            else if (c == '!' || c == '?')
                delay *= specialDelayMultiplier;

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }
}
