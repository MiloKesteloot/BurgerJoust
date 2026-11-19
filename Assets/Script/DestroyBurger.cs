using UnityEngine;

public class DestroyBurger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
