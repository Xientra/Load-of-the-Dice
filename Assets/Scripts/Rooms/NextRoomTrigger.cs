using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NextRoomTrigger : MonoBehaviour
{
	public event EventHandler<Collider2D> OnTriggerEnter;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			OnTriggerEnter?.Invoke(this, collision);
	}
}
