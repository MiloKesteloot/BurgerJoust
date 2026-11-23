using UnityEngine;

public class DestroyBurger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool caught;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (caught) return;

        if (other.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Joust"))
        {
            caught = true;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            this.transform.SetParent(other.transform);
            rb.bodyType = RigidbodyType2D.Kinematic;    
            Debug.Log("Hit Joust");

            if (OrderManager.Instance != null)
            {
                SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
                OrderManager.Instance.IngredientCaught(gameObject, sr != null ? sr.sprite : null);
            }
        }
    }
}
