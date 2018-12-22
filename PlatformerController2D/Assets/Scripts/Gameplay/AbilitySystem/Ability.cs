using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
	[Header("General")]
	public string abilityName;
	public float abilityCooldown;

	[Header("Visual")]
	public Sprite abilitySprite;

	[Header("Audio")]
	public AudioClip abilitySound;

	public abstract void Initialize(GameObject obj);
	public abstract void TriggerAbility();
}
