using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	private Vector3 targetPos;

	public float smoothing = 10f;

	private void Awake()
	{
		targetPos = transform.position;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothing);
	}

	public void MoveToPosition(Vector3 position)
	{
		targetPos = position;
	}
}
