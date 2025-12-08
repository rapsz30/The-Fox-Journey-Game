using UnityEngine;

public class Cherry : MonoBehaviour
{
    Animator anim;
    bool collected = false;
    Collider2D col;

    // Tambahkan referensi ke Game Manager
    public GameManager gameManager; 

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>(); // collider di object ini
        
        // Coba cari GameManager secara otomatis
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    // Di Cherry.cs:
    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            Debug.Log("Collected!");
            col.enabled = false;

            // Panggil UI Manager untuk menambah score
            if (gameManager != null)
            {
                gameManager.AddScore(10); // Tambah 1 poin
            }

            // Panggil Trigger animasi collect
            anim.SetTrigger("collect"); 

            Destroy(gameObject, 0.4f);
        }
    }
}