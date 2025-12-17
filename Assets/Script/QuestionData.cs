using UnityEngine;

[System.Serializable]
public class QuestionData
{
    [TextArea] public string question;
    public bool correctAnswer;
    [TextArea] public string answerExplanation;
}
