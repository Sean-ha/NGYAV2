using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private DisplayObject stackedSprite;

	[SerializeField] private float moveSpeed;
	[SerializeField] private float turnSpeed;

	private Rigidbody2D rb;
	private Vector2 input;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		// TODO: Set somewhere else
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
		input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");
		if (input.x != 0)
		{
			stackedSprite.rotation.z -= input.x * turnSpeed;
		}

		Vector2 facingDirection = HelperFunctions.GetDirectionFromDAngle(stackedSprite.rotation.z);

		rb.velocity = facingDirection * moveSpeed;
	}
}
