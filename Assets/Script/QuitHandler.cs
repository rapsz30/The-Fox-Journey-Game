using UnityEngine;

public class QuitHandler : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Game sedang ditutup...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}