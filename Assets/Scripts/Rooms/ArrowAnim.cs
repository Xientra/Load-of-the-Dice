using UnityEngine;

public class ArrowAnim : MonoBehaviour
{
	private Vector3 originalPos;

	public Vector3 direction = new Vector3(1, 0, 0);
	public float speed = 4f;

	private void Start()
	{
		originalPos = transform.localPosition;
	}

	private void Update()
	{
	float moveValue = ((Mathf.Sin(Time.time * speed) + 1) / 2);
		transform.localPosition = originalPos + direction.x * moveValue * transform.right + direction.y * moveValue * transform.up;
	}
}
