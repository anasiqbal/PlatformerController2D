// ----------------------------------------------------------------------------
// Author: Anas Iqbal
// Date:   12/05/18
// ----------------------------------------------------------------------------

using UnityEngine;

namespace CustomVariableTypes
{
	[CreateAssetMenu(menuName = "Custom Type/ Vector2 Variable")]
	public class Vector2Variable : ScriptableObject
	{
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
		public Vector2 value;

		public float x
		{
			get { return this.value.x; }
			set { SetValue(value, this.value.y); }
		}

		public float y
		{
			get { return this.value.y; }
			set { SetValue(this.value.x, value); }
		}

		public void SetValue(Vector2 value)
		{
			this.value = value;
		}

		public void SetValue(float x, float y)
		{
			this.value = new Vector2(x, y);
		}

		public void SetValue(Vector3Variable value)
		{
			this.value = value.value;
		}

		public void ApplyChange(Vector2 amount)
		{
			value += amount;
		}

		public void ApplyChange(Vector2Variable amount)
		{
			value += amount.value;
		}
	}
}