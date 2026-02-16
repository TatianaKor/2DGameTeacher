using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private float invincibleTime = 1;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction launchProjectileAction;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 move;
    private int currentHP;
    private float invincibleCoolDownTimer;
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHP = maxHP;

        moveAction.Enable();
        launchProjectileAction.Enable();
    }

    // Update is called once per framess
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();

        animator.SetFloat("Speed", move.sqrMagnitude);
        animator.SetFloat("SpeedX", move.x);
        animator.SetFloat("SpeedY", move.y);

        spriteRenderer.flipX = move.x > 0;

        if(invincibleCoolDownTimer > 0)
        {
            isInvincible = true;
            invincibleCoolDownTimer -= Time.deltaTime;
        }
        else
        {
            isInvincible = false;
        }

        if(launchProjectileAction.WasPressedThisFrame())
        {
            LaunchProjectile();
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(move.x, move.y, 0) * speed * Time.deltaTime;
    }

    private void LaunchProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.Launch(move, 300);

        animator.SetTrigger("Launch");
    }

    public bool ChangeHP(int amount)
    {
        if(amount < 0)
        {
            if(isInvincible)
            {
                return false;
            }
            else
            {
                invincibleCoolDownTimer = invincibleTime;
            }
        }

        int oldHP = currentHP;
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);

        Debug.Log("Current HP: " + currentHP + "/" + maxHP);

        return oldHP != currentHP;
    }
}
