using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimationType
{
	Idle,
	Jump,
	Crouch,
	Falling,
	Dash,
	Slide,
	WallSlide,
	WallClimb,
	Attack,
}

[RequireComponent (typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	#region Member Variables

	[Header ("References")]
	[SerializeField] GameObject defaultPlayerModel;
	private PlayerModelInfo playerModel;
	private Animator animator;
	private Rigidbody2D rb2D;
	private CameraController cameraController;
	private Animator cameraAnimator;

	[Header ("Movement")]
	[SerializeField] private float moveSpeed = 65;
	private float moveInput;

	[Header("Jumping")]
	[SerializeField] private float jumpForce = 10;
	[SerializeField] private int extraJumps = 1;
	[SerializeField] private float maxJumpDuration = 0.3f;
	[SerializeField] private float extraJumpDuration = 0.15f;

	private int extraJumpsRemaining;
	private float jumpTimer;
	private bool isJumping;

	[Header ("Dash")]
	[SerializeField] private float dashSpeed = 30;
	[SerializeField] private float dashDuration = 0.1f;
	[SerializeField] private float waitBetweenDash = 0.3f;

	private bool isDashing;
	private int dashDirection;
	private float dashTimer;
	private float dashWaitTimer;

	[Header ("Ground Collisions")]
	[SerializeField] private float headCheckDistance = 0.05f;
	[SerializeField] private LayerMask headCollisionMask;
	[SerializeField] private float groundCheckDistance = 0.05f;
	[SerializeField] private LayerMask groundCollisionMask;

	private Transform headCheckPostion;
	private Transform groundCheckPosition;

	private bool isMidAir;          // character can go off ground when jumping or falling from platforms... so to keep check if player was mid air in the last frame
	private bool isFalling;			// check if chracter has started to fall. character can be
	private bool headBumped;		// character may hit an object above it while jumping
	public bool IsGrounded { get; private set; }

	// Private members
	private bool facingRight = true;

	#endregion

	#region Unity Methods

	private void Awake()
	{
		rb2D = GetComponent<Rigidbody2D> ();
		cameraController = Camera.main.gameObject.GetComponent<CameraController> ();
	}

	private void OnEnable()
	{
		playerModel = GetComponentInChildren<PlayerModelInfo> ();
		if(playerModel == null)
		{
			var model = Instantiate (defaultPlayerModel, transform.position, Quaternion.identity, transform);
			playerModel = model.GetComponent<PlayerModelInfo> ();
		}

		headCheckPostion = playerModel.headCheckPosition;
		groundCheckPosition = playerModel.groundCheckPosition;
		animator = playerModel.animator;
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
		else
		{
			animator.SetBool ("isRunning", false);
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

			animator.SetBool ("isGrounded", true);
		}
		else
		{
			isMidAir = true;
			animator.SetBool ("isGrounded", false);

			if (!isJumping)
			{
				isFalling = true;
				animator.SetBool ("isFalling", isFalling);
			}
			else if(isJumping)
			{
				if(rb2D.velocity.y < 0)
				{
					isFalling = true;
					animator.SetBool ("isFalling", isFalling);
				}
			}
		}

		// jump + double jump
		if (Input.GetKeyDown (KeyCode.Space))
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
		if (Input.GetKey (KeyCode.Space) && isJumping)
		{
			if (jumpTimer > 0 && !headBumped)
			{
				rb2D.velocity = Vector2.up * jumpForce;
				jumpTimer -= Time.deltaTime;

				isFalling = false;
				animator.SetBool ("isFalling", isFalling);
			}
			else
			{
				isJumping = false;
			}
		}

		if (Input.GetKeyUp (KeyCode.Space))
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
				animator.SetTrigger ("dash");
			}
		}

		if (isDashing)
		{
			if (dashTimer > 0)
			{
				dashTimer -= Time.deltaTime;
				dashDirection = moveInput != 0 ? (int) Mathf.Sign (moveInput) : (facingRight ? 1 : -1);

				rb2D.velocity = Vector2.right * dashDirection * dashSpeed;
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

	private void OnDisable()
	{

	}

	#endregion

	#region Helper Methods

	private void Jump(bool isExtraJump)
	{
		isDashing = false;
		isFalling = false;

		isJumping = true;
		rb2D.velocity = Vector2.up * jumpForce;

		if(isExtraJump)
		{
			jumpTimer = extraJumpDuration;
			animator.SetTrigger ("extraJump");
		}
		else
		{
			jumpTimer = maxJumpDuration;
			animator.SetTrigger ("jump");
		}
	}

	private void Landed()
	{
		isMidAir = false;
		isFalling = false;

		animator.SetBool ("isFalling", isFalling);
		cameraController.LightShake ();
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 scaler = transform.localScale;
		scaler.x *= -1;
		transform.localScale = scaler;
	}

	#endregion
}
