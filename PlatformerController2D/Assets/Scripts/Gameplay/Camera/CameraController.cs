using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator> ();
	}

	public void LightShake()
	{
		animator.SetTrigger ("lightShake");
	}
}
