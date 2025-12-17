using UnityEngine;
using UnityEngine.SceneManagement; // Dibutuhkan untuk pindah scene

public class TeleportScene : MonoBehaviour
{
    [SerializeField] private string namaSceneTujuan;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mengecek apakah yang menginjak adalah Player
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(namaSceneTujuan);
        }
    }
}