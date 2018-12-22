// ----------------------------------------------------------------------------
// Author: Anas Iqbal
// Date:   12/05/18
// ----------------------------------------------------------------------------

using UnityEngine;

namespace CustomVariableTypes
{
	[CreateAssetMenu(menuName = "Custom Type/ Int Variable")]
	public class IntVariable : ScriptableObject
	{
#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
#endif
		public int value;

		public void SetValue(int value)
		{
			this.value = value;
		}

		public void SetValue(IntVariable value)
		{
			this.value = value.value;
		}

		public void ApplyChange(int amount)
		{
			value += amount;
		}

		public void ApplyChange(IntVariable amount)
		{
			value += amount.value;
		}
	}
}