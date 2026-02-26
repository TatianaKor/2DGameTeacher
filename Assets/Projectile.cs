using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
        lifeTimer = 20;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile hit " + other.gameObject);
        Destroy(gameObject);
    }
}
