using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (AnimationInfo))]
public class AnimationInfoDrawer : PropertyDrawer
{
	//private bool foldOut = true;
	private GUIContent paramLabel;
	private Rect labelRect;
	private int rows = 5; // 4 parameters + 1 heading
	private float yOffset = 2;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		int displayedRows = property.isExpanded ? rows : 1;
		return (base.GetPropertyHeight (property, label) * displayedRows) + (displayedRows * yOffset);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty (position, label, property);
		labelRect = position;

		// Get properties
		SerializedProperty name = property.FindPropertyRelative ("name");
		SerializedProperty parameterName = property.FindPropertyRelative ("parameterName");
		SerializedProperty parameterHash = property.FindPropertyRelative ("parameterHash");
		SerializedProperty parameterType = property.FindPropertyRelative ("parameterType");

		EditorGUI.BeginChangeCheck ();
		int indent = EditorGUI.indentLevel;

		if (property.isExpanded = EditorGUI.Foldout (position, property.isExpanded, label, true))
		{
			// Store old indent level and set it to 0, the PrefixLabel takes care of it
			//EditorGUI.indentLevel = 1;

			labelRect.height = (labelRect.height / rows) - yOffset;
			labelRect.y += labelRect.height + yOffset;

			// draw name field
			paramLabel = new GUIContent ("Animation Name");
			position = EditorGUI.PrefixLabel (labelRect, paramLabel);
			EditorGUI.PropertyField (position, name, GUIContent.none);

			labelRect.y += labelRect.height + yOffset;

			// draw parameter name field
			paramLabel = new GUIContent ("Parameter Name");
			position = EditorGUI.PrefixLabel (labelRect, paramLabel);
			EditorGUI.PropertyField (position, parameterName, GUIContent.none);

			labelRect.y += labelRect.height + yOffset;

			// draw Parameter Hash field
			int hash = Animator.StringToHash (parameterName.stringValue);

			position = labelRect;
			position.x += 64;
			paramLabel = new GUIContent ("Hash");
			position = EditorGUI.PrefixLabel (position, paramLabel);
			position.x -= 64;
			EditorGUI.LabelField (position, hash.ToString ());

			labelRect.y += labelRect.height + yOffset;

			// draw Parameter Type Field
			paramLabel = new GUIContent ("Parameter Type");
			position = EditorGUI.PrefixLabel (labelRect, paramLabel);
			EditorGUI.PropertyField (position, parameterType, GUIContent.none);
		}

		if (EditorGUI.EndChangeCheck ())
			property.serializedObject.ApplyModifiedProperties ();

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();
	}
}
