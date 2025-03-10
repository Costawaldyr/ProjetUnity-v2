using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("keeper"))
        {
            collision.GetComponent<KeeperControler>().life--;
        }

        if (collision.CompareTag("Gizmo"))
        {
            collision.GetComponent<GizmoControler>().life--;
        }

        if (collision.CompareTag("Dragon"))
        {
            collision.GetComponent<DragonControler>().life--;
        }
    }
}
