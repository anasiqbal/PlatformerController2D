using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed;
	private float moveInput;

	[Header("Jumping")]
	[SerializeField] private float jumpForce;
	[SerializeField] private int extraJumps;
	[SerializeField] private float maxJumpDuration;
	[SerializeField] private float extraJumpsDuration;

	private int extraJumpsRemaining;
	private float jumpTimer;
	private bool isJumping;

	[Header ("Dash")]
	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashDuration;
	[SerializeField] private float waitBetweenDash;

	private bool isDashing;
	private float dashTimer;
	private float dashWaitTimer;

	[Header ("Ground Collisions")]
	[SerializeField] private Transform headCheckPostion;
	[SerializeField] private float headCheckDistance;
	[SerializeField] private LayerMask headCollisionMask;
	[SerializeField] private Transform groundCheckPosition;
	[SerializeField] private float groundCheckDistance;
	[SerializeField] private LayerMask groundCollisionMask;

	private bool isMidAir;			// character can go off ground when falling from platforms... so to keep check if player was mid air in the last frame
	private bool headBumped;		// character may hit an object above it while jumping
	public bool IsGrounded { get; private set; }

	[Header ("References")]
	[SerializeField] private Animator animator;
	private Rigidbody2D rb2D;
	private CameraController cameraController;
	private Animator cameraAnimator;

	private bool facingRight = true;

	private void Awake()
	{
		rb2D = GetComponent<Rigidbody2D> ();
		cameraController = Camera.main.gameObject.GetComponent<CameraController> ();
	}

	private void Start()
	{
		extraJumpsRemaining = extraJumps;
	}

	private void FixedUpdate()
	{
		// check if character is grounded
		IsGrounded = Physics2D.Raycast (groundCheckPosition.position, -Vector2.up, groundCheckDistance, groundCollisionMask);
		headBumped = Physics2D.Raycast (headCheckPostion.position, Vector2.up, headCheckDistance, headCollisionMask);

		// move character
		if (!isDashing)
		{
			moveInput = Input.GetAxisRaw ("Horizontal");
			rb2D.velocity = new Vector2 (moveInput * moveSpeed, rb2D.velocity.y);
			animator.SetBool ("isRunning", IsGrounded && moveInput != 0);
		}

		// flip the character facing direction
		if (!facingRight && moveInput > 0)
			Flip ();
		else if (facingRight && moveInput < 0)
			Flip ();
	}

	private void Update()
	{
		if(IsGrounded)
		{
			extraJumpsRemaining = extraJumps;

			// character was mid air earlier... perform landing actions
			if(isMidAir)
			{
				Landed ();
			}
		}
		else
		{
			isMidAir = true;
		}

		// jump + double jump
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))
		{
			if(IsGrounded)
			{
				Jump (false);
			}
			else if (extraJumpsRemaining > 0)
			{
				Jump (true);
				extraJumpsRemaining--;
			}
		}

		// variable jump height on long press
		if ((Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) && isJumping)
		{
			if(jumpTimer > 0 && !headBumped)
			{
				rb2D.velocity = Vector2.up * jumpForce;
				jumpTimer -= Time.deltaTime;
			}
			else
			{
				isJumping = false;
			}
		}

		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.W))
		{
			isJumping = false;
		}

		// Dash
		if ((Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) && !isDashing)
		{
			if (dashWaitTimer >= waitBetweenDash)
			{
				isJumping = false;
				isDashing = true;
				dashTimer = dashDuration;
			}
		}

		if (isDashing)
		{
			if (dashTimer > 0)
			{
				dashTimer -= Time.deltaTime;
				rb2D.velocity = Vector2.right * moveInput * dashSpeed;
			}
			else
			{
				dashWaitTimer = 0;
				isDashing = false;
			}
		}
		else
		{
			if(dashWaitTimer < waitBetweenDash)
				dashWaitTimer += Time.deltaTime;
		}
	}

	private void Jump(bool isExtraJump)
	{
		isDashing = false;

		isJumping = true;
		jumpTimer = isExtraJump ? extraJumpsDuration : maxJumpDuration;
		rb2D.velocity = Vector2.up * jumpForce;

		animator.SetTrigger ("jump");
	}

	private void Landed()
	{
		isMidAir = false;
		animator.SetTrigger ("landed");
		cameraController.LightShake ();
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 scaler = transform.localScale;
		scaler.x *= -1;
		transform.localScale = scaler;
	}
}
