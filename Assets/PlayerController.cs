using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private float invincibleTime = 1;
    [SerializeField] private InputAction moveAction;

    private Vector2 move;
    private int currentHP;
    private float invincibleCoolDownTimer;
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;

        moveAction.Enable();
    }

    // Update is called once per framess
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();

        if(invincibleCoolDownTimer > 0)
        {
            isInvincible = true;
            invincibleCoolDownTimer -= Time.deltaTime;
        }
        else
        {
            isInvincible = false;
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(move.x, move.y, 0) * speed * Time.deltaTime;
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
