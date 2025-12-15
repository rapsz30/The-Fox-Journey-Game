using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SimpleLoading : MonoBehaviour
{
    public float loadingTime = 5f;
    public string nextSceneName = "MainMenu";
    public TMP_Text loadingText;

    public float dotSpeed = 0.6f; 

    float timer;
    float dotTimer;
    int dotCount = 0;

    void Update()
    {
        timer += Time.deltaTime;
        dotTimer += Time.deltaTime;

        if (dotTimer >= dotSpeed)
        {
            dotTimer = 0f;
            dotCount = (dotCount + 1) % 4;
            loadingText.text = "Loading" + new string('.', dotCount);
        }

        if (timer >= loadingTime)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
