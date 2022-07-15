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
		transform.localPosition = originalPos + direction * ((Mathf.Sin(Time.time * speed) + 1) / 2);
	}
}
