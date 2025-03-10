using UnityEngine;

public class DragonKitten : MonoBehaviour
{
    public Transform Dragon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Dragon.GetComponent<DragonControler>().enabled = true;
            Dragon.GetComponent<Animator>().SetBool("IsWalk", true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Dragon.GetComponent<DragonControler>().enabled = false;
            Dragon.GetComponent<Animator>().SetBool("IsWalk", false);
            
        }
    }
}
