using UnityEngine;

public class EnemyStomping : MonoBehaviour
{
    // Seret Rigidbody2D Fox ke slot ini di Inspector Frog
    public Rigidbody2D playerRB; 
    // Seret Animator Frog ke slot ini
    public Animator enemyAnim; 

    void Start()
    {
        // Mendapatkan Animator musuh jika belum diset di Inspector
        if (enemyAnim == null)
        {
            enemyAnim = GetComponent<Animator>();
        }
    }
    
    // Fungsi ini dipicu oleh Collider Trigger di atas kepala Frog
    void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang bertabrakan adalah Player
        if (other.CompareTag("Player"))
        {
            // Cek apakah tabrakan datang dari atas (Player bergerak ke bawah)
            // Asumsi: playerRB sudah diisi di Inspector dan Fox memiliki Tag "Player"
            if (playerRB != null && playerRB.velocity.y < 0)
            {
                // Musuh Mati (1-hit kill)
                
                // 1. Panggil Trigger Animasi Kematian
                if (enemyAnim != null)
                {
                    enemyAnim.SetTrigger("isDeath");
                }

                // 2. Beri dorongan ke atas pada Player (efek memantul)
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0); 
                playerRB.AddForce(Vector2.up * 400f); 

                // 3. Hancurkan objek Frog setelah jeda animasi (0.4 detik)
                Destroy(gameObject, 0.4f); 
            }
        }
    }
}