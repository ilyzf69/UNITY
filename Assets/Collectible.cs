using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            GameController gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.CollectItem();
            }
            Destroy(gameObject);
        }
    }
}
