using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isVertical;
    [SerializeField] private float changeDirectionTime;

    private Rigidbody2D rb;
    private float changeDirectionCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        changeDirectionCooldownTimer = changeDirectionTime;
    }

    void Update()
    {
        changeDirectionCooldownTimer -= Time.deltaTime;
        if(changeDirectionCooldownTimer < 0)
        {
            speed *= -1;
            changeDirectionCooldownTimer = changeDirectionTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 nextPosition = rb.position;

        if(isVertical)
        {
            nextPosition.y += speed * Time.deltaTime;
        }
        else
        {
            nextPosition.x += speed * Time.deltaTime;
        }


        rb.MovePosition(nextPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.ChangeHP(-1);
        }
    }
}
