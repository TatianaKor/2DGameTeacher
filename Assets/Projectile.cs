using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile hit " + other.gameObject);
        Destroy(gameObject);
    }
}
