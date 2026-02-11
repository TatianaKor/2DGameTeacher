using UnityEngine;

public class ChangeHPZone : MonoBehaviour
{
    [SerializeField] private int changeHPAmount;
    [SerializeField] private bool destroyAfterUse = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.ChangeHP(changeHPAmount);

            if(destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
