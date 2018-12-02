using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerController
{
	[RequireComponent(typeof(PlatformerController2D))]
	public class Player : MonoBehaviour
	{
		[Header("Movement Controls")]
		[SerializeField] float moveSpeed = 6f;
		[SerializeField] float accelerationTimeGrounded = 0.1f;

		[Header ("Jump Controls")]
		[SerializeField] float accelerationTimeAirborne = 0.2f;
		[SerializeField] float jumpHeight = 4f;
		[SerializeField] bool useSeparateGavityForFalling = false;
		[SerializeField] float timeToReachJumpHeight = 0.5f;
		[SerializeField] float timeToReachGround = 0.5f;

		float jumpVelocity;
		float jumpGravity;
		float fallGravity;
		Vector3 velocity;
		float velocityXSmoothing;

		PlatformerController2D controller2D;

		private void Start()
		{
			controller2D = GetComponent<PlatformerController2D>();

			jumpGravity = -(2 * jumpHeight) / Mathf.Pow(timeToReachJumpHeight, 2);
			fallGravity = useSeparateGavityForFalling ? -(2 * jumpHeight) / Mathf.Pow(timeToReachGround, 2) : jumpGravity;
			
			jumpVelocity = Mathf.Abs(jumpGravity) * timeToReachJumpHeight;
		}

		private void Update()
		{
			if(controller2D.collisionInfo.above || controller2D.collisionInfo.below)
			{
				velocity.y = 0;
			}

			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if(Input.GetKeyDown(KeyCode.Space) && controller2D.collisionInfo.below)
			{
				velocity.y = jumpVelocity;
			}

			float targetVelocity = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller2D.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += jumpGravity * Time.deltaTime;
			controller2D.Move(velocity * Time.deltaTime);
		}
	}
}

