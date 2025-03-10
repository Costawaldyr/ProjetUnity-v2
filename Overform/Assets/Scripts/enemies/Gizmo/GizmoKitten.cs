using UnityEngine;

public class GizmoKitten : MonoBehaviour
{
    public Transform Gizmo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Gizmo.GetComponent<GizmoControler>().enabled = true;
            Gizmo.GetComponent<Animator>().SetBool("isRun", true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Gizmo.GetComponent<GizmoControler>().enabled = false;
            Gizmo.GetComponent<Animator>().SetBool("isRun", false);
            
        }
    }
}
