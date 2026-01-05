using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Fox : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    float dirX;
    float moveSpeed = 5f;

    private Vector3 respawnPosition; 
    public float deathYCoordinate = -10f; 

    public GameManager gameManager; 
    int healthPoints = 3;
    bool isHurting;

    bool facingRight = true;
    Vector3 localScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;

        respawnPosition = transform.position; 

        if (gameManager != null)
        {
            gameManager.UpdateHealthUI(healthPoints);
        }
    }

void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            && rb.velocity.y == 0 && !isHurting)
        {
            rb.AddForce(Vector2.up * 600f);
        }

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 7f;
        else
            moveSpeed = 5f;

        SetAnimationState();

        if (transform.position.y < deathYCoordinate)
        {
            Respawn();
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (isHurting || healthPoints <= 0) return;

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
            else 
            {
                anim.SetTrigger("isDeath");
                StopDeathProcedures();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Checkpoint"))
        {
            if (respawnPosition != col.transform.position) 
            {
                respawnPosition = col.transform.position; 
                Debug.Log("CHECKPOINT SAVED: Posisi respawn baru: " + respawnPosition); 
            }
            return; 
        }
        if (col.gameObject.CompareTag("Collectable"))
        {
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

  void StopDeathProcedures()
    {
        dirX = 0;
        isHurting = true;
        StopAllCoroutines();
        GetComponent<Collider2D>().enabled = false;
        
        if (gameManager != null)
        {
            anim.SetTrigger("isDeath"); 
            gameManager.GameOver();
        }
    }

    void Respawn()
    {
        if (isHurting || healthPoints <= 0) return;

        healthPoints -= 1;
        
        rb.velocity = Vector2.zero; 
        transform.position = respawnPosition; 

        if (gameManager != null)
        {
            gameManager.UpdateHealthUI(healthPoints);
        }

        if (healthPoints > 0)
        {
            anim.SetTrigger("isHurting");
            StartCoroutine(Hurt()); 
        }
        else{

            StopDeathProcedures();
        }
    }
}