// ----------------------------------------------------------------------------
// Author: Anas Iqbal
// Date:   12/05/18
// ----------------------------------------------------------------------------

using UnityEngine;

namespace CustomVariableTypes
{
	[CreateAssetMenu(menuName = "Custom Type/ Vector3 Variable")]
	public class Vector3Variable : ScriptableObject
	{
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
		public Vector3 value;

		public float x
		{
			get { return this.value.x; }
			set { SetValue(value, this.value.y, this.value.z); }
		}

		public float y
		{
			get { return this.value.y; }
			set { SetValue(this.value.x, value, this.value.z); }
		}

		public float z
		{
			get { return this.value.z; }
			set { SetValue(this.value.x, this.value.y, value); }
		}

		public void SetValue(Vector3 value)
		{
			this.value = value;
		}

		public void SetValue(float x, float y, float z)
		{
			this.value = new Vector3(x, y, z);
		}

		public void SetValue(Vector3Variable value)
		{
			this.value = value.value;
		}

		public void ApplyChange(Vector3 amount)
		{
			value += amount;
		}

		public void ApplyChange(Vector3Variable amount)
		{
			value += amount.value;
		}
	}
}