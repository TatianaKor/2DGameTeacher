using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isVertical;
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private AudioClip fixedSound;
    [SerializeField] private AudioClip[] hitSounds;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private float changeDirectionCooldownTimer;
    private bool isFixed = false;

    public event Action OnFixed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        changeDirectionCooldownTimer = changeDirectionTime;
    }

    void Update()
    {
        animator.SetFloat("SpeedX", isVertical ? 0 : speed);
        animator.SetFloat("SpeedY", isVertical ? speed : 0);

        spriteRenderer.flipX = animator.GetFloat("SpeedX") > 0;

        changeDirectionCooldownTimer -= Time.deltaTime;
        if(changeDirectionCooldownTimer < 0)
        {
            speed *= -1;
            changeDirectionCooldownTimer = changeDirectionTime;
        }
    }

    void FixedUpdate()
    {
        if(isFixed)
        {
            return;
        }

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
        if(isFixed)
        {
            return;
        }

        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.ChangeHP(-1);

            int randomIndex = UnityEngine.Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[randomIndex]);
        }
        else if(other.CompareTag("Projectile"))
        {
            animator.SetTrigger("Fixed");
            isFixed = true;

            audioSource.loop = false;
            audioSource.clip = null;
            audioSource.PlayOneShot(fixedSound);

            smokeEffect.Stop();

            OnFixed?.Invoke();
        }
    }
}
