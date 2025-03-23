using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 20f;
	public int damage = 40;
	public Rigidbody2D rb;
	public GameObject impactEffect;

	// Use this for initialization
	void Start () {
		rb.linearVelocity = transform.right * speed;
	}

	void OnTriggerEnter2D (Collider2D collision)
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

		Instantiate(impactEffect, transform.position, transform.rotation);

		Destroy(gameObject);
	}
	
}
