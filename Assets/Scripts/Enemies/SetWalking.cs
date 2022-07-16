using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SetWalking : MonoBehaviour
{
	private float walkingThreashold = 0.025f;

	private Rigidbody2D rb;
	private Animator animCtrl;

	private void Awake()
	{
		rb = GetComponentInParent<Rigidbody2D>();
		animCtrl = GetComponent<Animator>();
	}

	private void Update()
	{
		animCtrl.SetBool("walking", rb.velocity.sqrMagnitude > walkingThreashold * walkingThreashold);
	}
}
