using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabWeapon : MonoBehaviour 
{

	public Transform firePoint;
	public GameObject bulletPrefab;

	public AudioClip fireballSound;
	public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update () {
		if (Input.GetButtonDown("Fire2"))
		{
			Shoot();
		}
	}

	void Shoot ()
	{
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		if (fireballSound != null)
    audioSource.PlayOneShot(fireballSound);
	}
}
