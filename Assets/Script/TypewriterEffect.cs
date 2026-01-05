using UnityEngine;
using TMPro;
using System.Collections;
using System.Text;

public class StoryTypewriter : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text storyText;
    [TextArea(5, 20)]
    public string fullStory;

    [Header("Typing Settings")]
    public float typingSpeed = 0.025f;
    public float sentenceDelay = 0.5f;

    [Header("Audio")]
    public AudioSource typingAudio;
    public bool playSound = true;

    [Header("UI")]
    public GameObject nextButton;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        storyText.text = "";
        nextButton.SetActive(false);

        typingCoroutine = StartCoroutine(TypeStory());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTyping)
        {
            SkipTyping();
        }
    }

    IEnumerator TypeStory()
    {
        isTyping = true;

        string[] sentences = fullStory.Split('.');

        foreach (string sentence in sentences)
        {
            if (string.IsNullOrWhiteSpace(sentence)) continue;

            StringBuilder currentSentence = new StringBuilder(sentence.Trim() + ". ");

            foreach (char c in currentSentence.ToString())
            {
                storyText.text += c;

                if (playSound && typingAudio != null && !char.IsWhiteSpace(c))
                {
                    typingAudio.PlayOneShot(typingAudio.clip);
                }

                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(sentenceDelay);
        }

        isTyping = false;
        nextButton.SetActive(true);
    }

    public void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        storyText.text = fullStory;
        isTyping = false;
        nextButton.SetActive(true);
    }
}
