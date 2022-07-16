using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	public GameObject deathEffect;

	public float speed = 10f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerLife player = collision.GetComponent<PlayerLife>();
		if (player != null)
			player.TakeDamage();
	}

	private void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}

	void Update()
	{
		transform.position += transform.right * Time.deltaTime * speed;
	}
}
