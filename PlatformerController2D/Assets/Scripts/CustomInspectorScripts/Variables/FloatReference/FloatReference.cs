// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;

namespace CustomVariableTypes
{
	[Serializable]
	public class FloatReference
	{
		public bool useConstant = true;
		public float constantValue;
		public FloatVariable variable;

		public FloatReference()
		{ }

		public FloatReference(float value)
		{
			useConstant = true;
			constantValue = value;
		}

		public float Value
		{
			get { return useConstant ? constantValue : variable.value; }
		}

		public static implicit operator float(FloatReference reference)
		{
			return reference.Value;
		}
	}
}