using UnityEngine;

public class Cherry : MonoBehaviour
{
    Animator anim;
    bool collected = false;
    Collider2D col;

    public GameManager gameManager; 

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>(); 

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            Debug.Log("Collected!");
            col.enabled = false;

            if (gameManager != null)
            {
                gameManager.AddScore(10); 
            }

            anim.SetTrigger("collect"); 

            Destroy(gameObject, 0.4f);
        }
    }
}