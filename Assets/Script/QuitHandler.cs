using UnityEngine;

public class QuitHandler : MonoBehaviour
{
    // Fungsi ini yang akan dihubungkan ke Button OnClick()
    public void ExitGame()
    {
        Debug.Log("Game sedang ditutup...");

        // Fungsi ini bekerja untuk game yang sudah di-build (.exe, .apk, dll)
        Application.Quit();

        // Kode di bawah ini hanya berjalan saat Anda mengetes di dalam Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}