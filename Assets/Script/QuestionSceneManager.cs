using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionSceneManager : MonoBehaviour
{
    public enum State { Intro, Question, AnswerFeedback, Outro }
    private State currentState;

    // Simpan skor agar bisa dibaca di scene FinalScore atau GameOverScreen
    public static int finalGemScore = 0;

    [Header("UI Panels")]
    public GameObject storyPanel;    
    public GameObject questionPanel; 
    public GameObject answerPanel;   
    public GameObject pausePanel;    

    [Header("UI Text Elements")]
    public TextMeshProUGUI storyText;    
    public TextMeshProUGUI questionText; 
    public TextMeshProUGUI feedbackText; 
    public TextMeshProUGUI scoreText;

    [Header("Buttons")]
    public Button trueButton;
    public Button falseButton;
    public Button pauseButton;       
    public Button resumeButton;      
    public Button restartButton;     
    public Button mainMenuButton;    

    [Header("Data Settings")]
    public LevelStoryData levelStory;      
    public List<QuestionData> questionsDB; 

    [Header("Typing Settings")]
    public float typeSpeed = 0.04f;

    private List<QuestionData> selectedQuestions = new List<QuestionData>();
    private int currentIdx = 0;
    private int score = 0;
    private bool isPaused = false;
    private bool isTyping = false;
    private string currentFullText = "";
    private Coroutine typingCoroutine;

    void Start()
    {
        Time.timeScale = 1f;
        finalGemScore = 0; // Reset skor static saat mulai
        PrepareQuestions();
        SetupButtonListeners();

        // Reset UI State
        pausePanel.SetActive(false);
        if(pauseButton != null) pauseButton.gameObject.SetActive(true);
        questionPanel.SetActive(false);
        answerPanel.SetActive(false);
        UpdateScoreUI();
        
        currentIdx = 0;
        score = 0;
        ShowIntro();
    }

    void SetupButtonListeners()
    {
        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();
        trueButton.onClick.AddListener(() => HandleAnswer(true));
        falseButton.onClick.AddListener(() => HandleAnswer(false));

        if (pauseButton != null) pauseButton.onClick.AddListener(PauseGame);
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void PrepareQuestions()
    {
        List<QuestionData> tmp = new List<QuestionData>(questionsDB);
        for (int i = 0; i < tmp.Count; i++) {
            int r = Random.Range(i, tmp.Count);
            QuestionData t = tmp[i]; tmp[i] = tmp[r]; tmp[r] = t;
        }
        selectedQuestions.Clear();
        int limit = Mathf.Min(5, tmp.Count);
        for (int i = 0; i < limit; i++) selectedQuestions.Add(tmp[i]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame(); else PauseGame();
        }

        if (isPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopTypingAndShowFull();
            }
            else
            {
                if (currentState == State.Intro) ShowQuestion();
                else if (currentState == State.AnswerFeedback) NextStep();
                else if (currentState == State.Outro) FinishLevel();
            }
        }
    }

    void ShowIntro()
    {
        currentState = State.Intro;
        storyPanel.SetActive(true);
        questionPanel.SetActive(false);
        answerPanel.SetActive(false);
        StartTyping(storyText, levelStory.introStory);
    }

    void ShowQuestion()
    {
        currentState = State.Question;
        storyPanel.SetActive(false);
        questionPanel.SetActive(true);
        answerPanel.SetActive(false);
        StartTyping(questionText, selectedQuestions[currentIdx].question);
    }

    void HandleAnswer(bool choice)
    {
        if (currentState != State.Question || isTyping || isPaused) return;
        
        bool isCorrect = (choice == selectedQuestions[currentIdx].correctAnswer);
        if (isCorrect) {
            score++;
            finalGemScore = score; // Update skor static
            UpdateScoreUI();
        }
        ShowAnswerFeedback(isCorrect);
    }

    void ShowAnswerFeedback(bool correct)
    {
        currentState = State.AnswerFeedback;
        questionPanel.SetActive(false);
        answerPanel.SetActive(true);
        string header = correct ? "<color=#00FF00>BENAR!</color>" : "<color=#FF0000>SALAH!</color>";
        StartTyping(feedbackText, header + "\n\n" + selectedQuestions[currentIdx].answerExplanation);
    }

    void NextStep()
    {
        currentIdx++;
        if (currentIdx < selectedQuestions.Count) ShowQuestion(); 
        else ShowOutro();
    }

    void ShowOutro()
    {
        currentState = State.Outro;
        answerPanel.SetActive(false);
        storyPanel.SetActive(true);
        string finalStory = (score >= 3) ? levelStory.winStory : levelStory.loseStory;
        StartTyping(storyText, finalStory);
    }

    void FinishLevel()
    {
        if (score >= 3) 
        {
            SceneManager.LoadScene("FinalScore");
        }
        else 
        {
            // Jika benar kurang dari 3, langsung pindah ke scene GameOverScreen
            SceneManager.LoadScene("GameOverScreen");
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }

    // --- TYPEWRITER CORE ---
    void StartTyping(TextMeshProUGUI element, string txt)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(element, txt));
    }

    IEnumerator TypeText(TextMeshProUGUI textElement, string fullText)
    {
        isTyping = true;
        currentFullText = fullText;
        textElement.text = "";
        int i = 0;
        while (i < fullText.Length)
        {
            if (fullText[i] == '<')
            {
                int endTag = fullText.IndexOf('>', i);
                if (endTag != -1)
                {
                    textElement.text += fullText.Substring(i, endTag - i + 1);
                    i = endTag + 1;
                    continue;
                }
            }
            textElement.text += fullText[i];
            i++;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    void StopTypingAndShowFull()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        isTyping = false;
        if (currentState == State.Intro || currentState == State.Outro) storyText.text = currentFullText;
        else if (currentState == State.Question) questionText.text = currentFullText;
        else if (currentState == State.AnswerFeedback) feedbackText.text = currentFullText;
    }

    // --- PAUSE SYSTEM ---
    public void PauseGame() 
    { 
        isPaused = true; 
        pausePanel.SetActive(true); 
        if(pauseButton != null) pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f; 
    }

    public void ResumeGame() 
    { 
        isPaused = false; 
        pausePanel.SetActive(false); 
        if(pauseButton != null) pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1f; 
    }

    public void RestartGame() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void GoToMainMenu() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("HomePage"); 
    }
}