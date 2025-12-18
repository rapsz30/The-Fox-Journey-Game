using UnityEngine;

public class EnemyStomping : MonoBehaviour
{
    public Rigidbody2D playerRB; 
    public Animator enemyAnim; 

    void Start()
    {
        if (enemyAnim == null)
        {
            enemyAnim = GetComponent<Animator>();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRB != null && playerRB.velocity.y < 0)
            {
                if (enemyAnim != null)
                {
                    enemyAnim.SetTrigger("isDeath");
                }
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0); 
                playerRB.AddForce(Vector2.up * 400f); 

                Destroy(gameObject, 0.4f); 
            }
        }
    }
}