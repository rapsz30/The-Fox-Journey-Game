using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestionData
{
    [TextArea] public string question;
    public bool correctAnswer;
    [TextArea] public string answerExplanation;
}

// Tambahkan class ini untuk menampung cerita Level
[System.Serializable]
public class LevelStoryData
{
    [TextArea] public string introStory;
    [TextArea] public string winStory;
    [TextArea] public string loseStory;
}