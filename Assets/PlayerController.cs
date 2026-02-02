using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
    }

    // Update is called once per framess
    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        Debug.Log(move);

        transform.position += new Vector3(move.x, move.y, 0) * speed * Time.deltaTime;
    }
}
