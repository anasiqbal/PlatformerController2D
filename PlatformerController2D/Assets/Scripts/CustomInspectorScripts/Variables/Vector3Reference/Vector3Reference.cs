// ----------------------------------------------------------------------------
// Author: Anas Iqbal
// Date:   12/05/18
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CustomVariableTypes
{
	[Serializable]
	public class Vector3Reference
	{
		public bool useConstant = true;
		public Vector3 constantValue;
		public Vector3Variable variable;

		public Vector3Reference()
		{ }

		public Vector3Reference(Vector3 value)
		{
			useConstant = true;
			constantValue = value;
		}

		public Vector3 Value
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

		public static implicit operator Vector3(Vector3Reference reference)
		{
			return reference.Value;
		}
	}
}