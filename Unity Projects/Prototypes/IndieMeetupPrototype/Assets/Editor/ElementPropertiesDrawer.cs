using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ElementProperties),true)]
public class ElementPropertiesDrawer : PropertyDrawer {
	const int textHeight = 16;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		int initialIndentLevel;
		Rect elementRect;
		Rect levelRect;
		SerializedProperty elementProp;
		SerializedProperty levelProp;

		elementProp = property.FindPropertyRelative("_element").FindPropertyRelative("_element");
		levelProp = property.FindPropertyRelative("_level");

		EditorGUI.BeginProperty(position, label, property);
		initialIndentLevel = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		elementRect = new Rect(position.x, position.y, (int)Mathf.Max(position.width * 0.5f, 70), textHeight);
		levelRect = new Rect(position.x + elementRect.width + 5, position.y, position.width - (int)Mathf.Max(position.width * 0.5f, 30) - 5, textHeight);

		EditorGUI.PropertyField(elementRect, elementProp, GUIContent.none);
		EditorGUI.PropertyField(levelRect, levelProp, GUIContent.none);

		EditorGUI.indentLevel = initialIndentLevel;
		EditorGUI.EndProperty();

	}

	//	ESSENTIAL!!
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return textHeight;
	}
}
