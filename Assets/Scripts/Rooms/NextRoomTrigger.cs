using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NextRoomTrigger : MonoBehaviour
{
	public event EventHandler<Collider2D> OnTriggerEnter;

	private float activationDelay = 0.0f;
	private float activationTimestamp;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (activationTimestamp < Time.time)
			if (collision.CompareTag("Player"))
				OnTriggerEnter?.Invoke(this, collision);
	}

	private void OnEnable()
	{
		activationTimestamp = Time.time + activationDelay;
	}
}
