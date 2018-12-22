using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Jump Ability")]
public class JumpAbility : Ability
{
	public float jumpForce = 10;
	public int extraJumps = 1;
	public float maxJumpDuration = 0.3f;
	public float extraJumpDuration = 0.2f;

	private int extraJumpsRemaining;
	private float jumpTimer;
	private bool isJumping;

	private GameObject target;
	private Rigidbody2D rb2D;

	public override void Initialize(GameObject targetObj)
	{
		target = targetObj;
		rb2D = target.GetComponent<Rigidbody2D> ();
	}

	public override void TriggerAbility()
	{
		throw new System.NotImplementedException ();
	}
}
