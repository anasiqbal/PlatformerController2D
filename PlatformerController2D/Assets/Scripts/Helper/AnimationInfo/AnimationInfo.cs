using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationParameterType
{
	INTEGER,
	FLOAT,
	BOOLEAN,
	TRIGGER
}

[System.Serializable]
public class AnimationInfo
{
	public string name;

	public string parameterName;
	public int parameterHash;

	public AnimationParameterType parameterType;
}
