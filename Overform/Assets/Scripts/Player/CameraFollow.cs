using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Référence au transform du joueur
    public Vector3 offset;   // Décalage entre la caméra et le joueur
    public float smoothSpeed = 0.125f; // Vitesse de suivi de la caméra

  void LateUpdate()
  {
      if (player == null)
      {
          Debug.LogWarning("Player n'est pas assigné dans CameraFollow2D.");
          return;
      }

      Debug.Log("Position du joueur : " + player.position);
      Debug.Log("Position de la caméra : " + transform.position);

      Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
      Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
      transform.position = smoothedPosition;
  }
}