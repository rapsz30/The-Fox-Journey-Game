using System.Collections;
using UnityEngine;

public class Fox : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    float dirX;
    float moveSpeed = 5f;

    int healthPoints = 3;
    bool isHurting;
    bool facingRight = true;
    Vector3 localScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
    }

    void Update()
    {
        // Input movement
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Jump
        if (Input.GetButtonDown("Jump") && rb.velocity.y == 0)
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
        if (!isHurting)
            rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void SetAnimationState()
    {
        // WALKING = ada input horizontal + sedang tidak lompat/jatuh
        anim.SetBool("isWalking", dirX != 0 && rb.velocity.y == 0);

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

    void OnTriggerEnter2D(Collider2D col)
{
    // Cek tabrakan dengan musuh (Frog)
    if (col.gameObject.CompareTag("Enemy")) 
    {
        // PENTING: Untuk identifikasi yang lebih baik di masa depan,
        // gunakan TAG "Enemy" di Unity daripada membandingkan nama "Frog".
        
        healthPoints -= 1;
        
        if (healthPoints > 0)
        {
            // Jika masih hidup setelah ditabrak Frog
            anim.SetTrigger("isHurting");
            StartCoroutine("Hurt");
        }
        else
        {
            // Jika HP habis (Mati)
            dirX = 0;
            isHurting = true;
            anim.SetTrigger("isHurting");
            // Tambahkan logika kematian di sini (Destroy, GameOver, dll.)
        }
    }

    // Cek tabrakan dengan item koleksi (Cherry)
    // Script Cherry akan menangani apa yang terjadi pada Cherry, 
    // Fox hanya perlu mengabaikan tabrakan ini agar tidak memicu logika Hurt.
    if (col.gameObject.CompareTag("Collectable"))
    {
        // Tidak perlu melakukan apa-apa di skrip Fox, 
        // karena skrip Cherry sudah menangani 'collect' (dengan asumsi script Cherry bekerja)
        return; 
    }
}

    IEnumerator Hurt()
    {
        isHurting = true;
        rb.velocity = Vector2.zero;

        if (facingRight)
            rb.AddForce(new Vector2(-200f, 200f));
        else
            rb.AddForce(new Vector2(200f, 200f));

        yield return new WaitForSeconds(0.5f);
        isHurting = false;
    }
}
