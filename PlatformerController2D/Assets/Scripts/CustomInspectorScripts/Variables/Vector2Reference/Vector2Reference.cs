// ----------------------------------------------------------------------------
// Author: Anas Iqbal
// Date:   12/05/18
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CustomVariableTypes
{
	[Serializable]
	public class Vector2Reference
	{
		public bool useConstant = true;
		public Vector2 constantValue;
		public Vector2Variable variable;

		public Vector2Reference()
		{ }

		public Vector2Reference(Vector2 value)
		{
			useConstant = true;
			constantValue = value;
		}

		public Vector2 Value
		{
			get { return useConstant ? constantValue : variable.value; }
			set
			{
				if (useConstant)
					constantValue = value;
				else
					variable.SetValue(value);
			}
		}

		public static implicit operator Vector2(Vector2Reference reference)
		{
			return reference.Value;
		}

		public static implicit operator Vector3(Vector2Reference reference)
		{
			return new Vector3(reference.Value.x, reference.Value.y, 0);
		}
	}
}