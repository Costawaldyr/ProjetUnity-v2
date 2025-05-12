using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 20f;
	public int damage = 40;
	public Rigidbody2D rb;
	public GameObject impactEffect;
	public AudioClip impactSound;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rb.linearVelocity = transform.right * speed;
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
			// Infliger des dégâts
			if (collision.CompareTag("keeper"))
					collision.GetComponent<KeeperControler>().life--;

			if (collision.CompareTag("Gizmo"))
					collision.GetComponent<GizmoControler>().life--;

			if (collision.CompareTag("Dragon"))
					collision.GetComponent<DragonControler>().life--;

			// Jouer le son d'impact
			if (impactSound != null && audioSource != null)
					audioSource.PlayOneShot(impactSound);

			// Effet visuel
			Instantiate(impactEffect, transform.position, transform.rotation);

			// Détruire la balle
			Destroy(gameObject);
	}
}
