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
    [SerializeField] private InputAction interactionAction;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 move;
    private Vector2 lastMove = Vector2.down;
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
        interactionAction.Enable();
    }

    // Update is called once per framess
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();

        if (move.sqrMagnitude > 0.1f)
        {
            lastMove = move;
        }

        animator.SetFloat("Speed", move.sqrMagnitude);
        animator.SetFloat("SpeedX", lastMove.x);
        animator.SetFloat("SpeedY", lastMove.y);

        spriteRenderer.flipX = lastMove.x > 0;

        if(invincibleCoolDownTimer > 0)
        {
            isInvincible = true;
            invincibleCoolDownTimer -= Time.deltaTime;
        }
        else
        {
            isInvincible = false;
        }

        if(launchProjectileAction.WasPressedThisFrame() && !animator.GetCurrentAnimatorStateInfo(0).IsName("Launch Blend Tree"))
        {
            LaunchProjectile();
        }

        if(interactionAction.WasPressedThisFrame())
        {
            FindFriend();
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(move.x, move.y, 0) * speed * Time.deltaTime;
    }

    private void LaunchProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.75f, Quaternion.identity);
        projectile.Launch(lastMove, 300);

        animator.SetTrigger("Launch");
    }

    private void FindFriend()
    {
        Vector2 rayStart = transform.position + Vector3.up * 0.2f;
        Vector2 rayDirection = lastMove;
        float rayLenght = 1.5f;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDirection, rayLenght, LayerMask.GetMask("NPC"));
        //Debug.DrawRay(rayStart, rayDirection.normalized * rayLenght, Color.red, 2f);

        if(hit.collider != null)
        {
            Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
        }
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
