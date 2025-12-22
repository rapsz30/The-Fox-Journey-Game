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
        // Mengambil nilai dari variabel static di scene sebelumnya
        int cherryScore = GameManager.finalCherryScore;
        int gemScore = QuestionSceneManager.finalGemScore;

        // Tampilkan ke teks
        if (scoreCherryText != null)
            scoreCherryText.text = cherryScore.ToString();

        if (scoreGemsText != null)
            scoreGemsText.text = gemScore.ToString();
            
            
        // Opsional: Reset score ke 0 jika ingin memulai game baru dari awal
        // GameManager.finalCherryScore = 0;
        // QuestionSceneManager.finalGemScore = 0;
    }

    public void PlayAgain()
    {
        // Pastikan nama scene sesuai dengan yang ada di Build Settings
        SceneManager.LoadScene("HomePage");
    }

    public void QuitGame()
    {
        // Keluar dari aplikasi
        Application.Quit();
        
        // Catatan: Application.Quit() tidak bekerja di dalam editor Unity, 
        // jadi kita tambahkan log untuk memastikan fungsi terpanggil saat testing.
        Debug.Log("Game exited");
    }
}