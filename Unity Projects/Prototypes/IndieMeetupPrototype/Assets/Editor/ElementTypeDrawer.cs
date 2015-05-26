using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ElementType), true)]
public class ElementTypeDrawer : PropertyDrawer
{
	const int textHeight = 16;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		int initialIndentLevel;
		SerializedProperty elementProp;

		elementProp = property.FindPropertyRelative("_element");

		EditorGUI.BeginProperty(position, label, property);
		initialIndentLevel = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		EditorGUI.PropertyField(position, elementProp, GUIContent.none);

		EditorGUI.indentLevel = initialIndentLevel;
		EditorGUI.EndProperty();

	}

	//	ESSENTIAL!!
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return textHeight;
	}
}
