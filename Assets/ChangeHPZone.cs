using UnityEngine;

public class ChangeHPZone : MonoBehaviour
{
    [SerializeField] private int changeHPAmount;
    [SerializeField] private bool destroyAfterUse = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckPlayer(other);
    }

    private void CheckPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController.ChangeHP(changeHPAmount) && destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
