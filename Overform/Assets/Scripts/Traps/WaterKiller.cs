using UnityEngine;

public class WaterKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // Détruit le joueur
            //other.GetComponent<PlayerController>().Die(); 
        }
    }
}

