using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Tambahkan ini jika Anda menggunakan Restart/Game Over

public class Fox : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    float dirX;
    float moveSpeed = 5f;

    // Variabel Respawn dan Checkpoint
    private Vector3 respawnPosition; // Posisi Checkpoint terakhir
    public float deathYCoordinate = -10f; // Batas Y untuk respawn jika jatuh
    
    // Variabel Health dan UI
    public GameManager gameManager; 
    int healthPoints = 3;
    bool isHurting;
    
    // Variabel Gerakan
    bool facingRight = true;
    Vector3 localScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;

        // 1. SET CHECKPOINT AWAL: Posisi awal adalah checkpoint pertama
        respawnPosition = transform.position; 
        
        // Inisialisasi awal Life UI (3 nyawa)
        if (gameManager != null)
        {
            gameManager.UpdateHealthUI(healthPoints);
        }
    }

    void Update()
    {
        // Input movement
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Jump
        if (Input.GetButtonDown("Jump") && rb.velocity.y == 0 && !isHurting)
            rb.AddForce(Vector2.up * 600f);

        // Running (speed only, no anim)
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 7f;
        else
            moveSpeed = 5f;

        SetAnimationState();

        // --- DETEKSI JATUH KE VOID ---
        if (transform.position.y < deathYCoordinate)
        {
            if (healthPoints > 0)
            {
                Debug.Log("FALL DETECTED: Memanggil Respawn(). HP sisa: " + (healthPoints - 1));
                Respawn(); 
            }
            // Jika HP <= 0, Game Over sudah dipanggil di Respawn() atau OnCollisionEnter2D
        }
    }

    void FixedUpdate()
    {
        if (!isHurting)
            rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void SetAnimationState()
    {
        anim.SetBool("isWalking", dirX != 0 && rb.velocity.y == 0 && !isHurting);
        anim.SetBool("isJumping", rb.velocity.y > 0);
        anim.SetBool("isFalling", rb.velocity.y < 0);
    }

    void CheckWhereToFace()
    {
        if (dirX > 0) facingRight = true;
        else if (dirX < 0) facingRight = false;

        if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    // Dipanggil saat Fox terkena Frog dari samping/bawah
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (isHurting || healthPoints <= 0) return;

            // Logika Hurt dan pengurangan HP
            healthPoints -= 1;

            if (healthPoints > 0)
            {
                anim.SetTrigger("isHurting");
                StartCoroutine(Hurt());
                if (gameManager != null)
                {
                    gameManager.UpdateHealthUI(healthPoints);
                }
            }
            else // Mati karena tabrakan
            {
                anim.SetTrigger("isDeath");
                StopDeathProcedures();
            }
        }
    }

    // Dipanggil saat Fox menyentuh Trigger (Cherry, Checkpoint, atau Head Stomping Frog)
    void OnTriggerEnter2D(Collider2D col)
    {
        // --- LOGIKA CHECKPOINT ---
        if (col.gameObject.CompareTag("Checkpoint"))
        {
            // Pastikan posisi respawn diperbarui hanya jika posisi checkpoint berbeda
            if (respawnPosition != col.transform.position) 
            {
                respawnPosition = col.transform.position; 
                Debug.Log("CHECKPOINT SAVED: Posisi respawn baru: " + respawnPosition); // <--- CEK CONSOLE INI!
            }
            // Opsional: Lakukan animasi Checkpoint di sini
            return; 
        }
        
        // Cek tabrakan dengan item koleksi (Collectable)
        if (col.gameObject.CompareTag("Collectable"))
        {
            return;
        }
    }

    IEnumerator Hurt()
    {
        isHurting = true;
        rb.velocity = Vector2.zero;

        // Beri dorongan kebal
        if (facingRight)
            rb.AddForce(new Vector2(-200f, 200f));
        else
            rb.AddForce(new Vector2(200f, 200f));

        yield return new WaitForSeconds(0.5f);
        isHurting = false;
    }

  void StopDeathProcedures()
    {
        dirX = 0;
        isHurting = true;
        StopAllCoroutines();
        GetComponent<Collider2D>().enabled = false;
        
        if (gameManager != null)
        {
            anim.SetTrigger("isDeath"); // Panggil animasi di sini
            gameManager.GameOver();
        }
    }

    // --- FUNGSI RESPAWN UTAMA (Dipanggil saat jatuh ke void) ---
    void Respawn()
    {
        // Cegah double respawn saat sedang terluka atau sudah mati
        if (isHurting || healthPoints <= 0) return;

        // Kurangi nyawa
        healthPoints -= 1;
        
        // Pindahkan Fox ke posisi respawn (Checkpoint/Awal)
        rb.velocity = Vector2.zero; 
        transform.position = respawnPosition; 
        
        // Update UI Life
        if (gameManager != null)
        {
            gameManager.UpdateHealthUI(healthPoints);
        }

        if (healthPoints > 0)
        {
            // Pindah ke state Hurt (masa kebal)
            anim.SetTrigger("isHurting");
            StartCoroutine(Hurt()); 
        }
        else
        {
            // Game Over karena kehabisan nyawa
            StopDeathProcedures();
        }
    }
}