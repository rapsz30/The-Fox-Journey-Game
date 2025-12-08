using System.Collections;
using UnityEngine;

public class Fox : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    float dirX;
    float moveSpeed = 5f;

    // Tambahkan referensi ke Game Manager
    public GameManager gameManager; 
    
    // Fox Health (3 Nyawa)
    int healthPoints = 3;
    bool isHurting;
    bool facingRight = true;
    Vector3 localScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;

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
    }

    void FixedUpdate()
    {
        // Fox hanya bisa bergerak jika TIDAK sedang terluka atau mati
        if (!isHurting)
            rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void SetAnimationState()
    {
        // WALKING = ada input horizontal + sedang tidak lompat/jatuh + tidak terluka
        anim.SetBool("isWalking", dirX != 0 && rb.velocity.y == 0 && !isHurting);

        // Jumping
        anim.SetBool("isJumping", rb.velocity.y > 0);

        // Falling
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

    // --- LOGIKA TABRAKAN MUSUH DAN LUKA ---
    // Menggunakan OnCollisionEnter2D karena tabrakan musuh bersifat fisik
    void OnCollisionEnter2D(Collision2D col)
    {
        // Cek tabrakan dengan musuh (Enemy)
        if (col.gameObject.CompareTag("Enemy"))
        {
            // 1. Pencegahan Re-hit/Tumpang Tindih:
            if (isHurting || healthPoints <= 0) return;

            // 2. Kurangi nyawa
            healthPoints -= 1;

            if (healthPoints > 0)
            {
                // Fox Terluka
                anim.SetTrigger("isHurting");
                StartCoroutine(Hurt());

                // Panggil UI Manager untuk update Life
                if (gameManager != null)
                {
                    gameManager.UpdateHealthUI(healthPoints);
                }
            }
            else // Fox Mati (healthPoints <= 0)
            {
                // Logika Kematian
                dirX = 0;
                isHurting = true;
                StopAllCoroutines();
                
                // Matikan Collider Fox segera
                GetComponent<Collider2D>().enabled = false; 

                anim.SetTrigger("isDeath"); 

                // Panggil UI Manager untuk Game Over
                if (gameManager != null)
                {
                    gameManager.UpdateHealthUI(healthPoints); // Tampilkan 0 life
                    gameManager.GameOver();
                }
            }
        }
    }

    // Menggunakan OnTriggerEnter2D HANYA untuk item Trigger seperti Collectable
    void OnTriggerEnter2D(Collider2D col)
    {
        // Cek tabrakan dengan item koleksi (Collectable)
        if (col.gameObject.CompareTag("Collectable"))
        {
            return;
        }
        // Collider Trigger musuh (untuk stomping) diurus oleh EnemyStomping.cs
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

        // Tunggu sampai periode kebal (invulnerability) selesai
        yield return new WaitForSeconds(0.5f);

        isHurting = false;
    }
}