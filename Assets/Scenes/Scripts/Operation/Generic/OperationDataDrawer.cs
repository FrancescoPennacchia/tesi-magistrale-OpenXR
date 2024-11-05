using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(OperationData))]
public class OperationDataDrawer : PropertyDrawer
{
    private const float VerticalSpacing = 2f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate the required height for the property drawer
        int fieldCount = 7; // Total number of fields to display
        return (EditorGUIUtility.singleLineHeight + VerticalSpacing) * fieldCount;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin property drawing
        EditorGUI.BeginProperty(position, label, property);

        // Set up positions for each property field
        Rect fieldPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Draw each property field
        SerializedProperty operationType = property.FindPropertyRelative("operationType");
        EditorGUI.PropertyField(fieldPosition, operationType);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        SerializedProperty instructionMessage = property.FindPropertyRelative("instructionMessage");
        EditorGUI.PropertyField(fieldPosition, instructionMessage);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        SerializedProperty targetObject = property.FindPropertyRelative("targetObject");
        EditorGUI.PropertyField(fieldPosition, targetObject);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        SerializedProperty socketInteractor = property.FindPropertyRelative("socketInteractor");
        EditorGUI.PropertyField(fieldPosition, socketInteractor);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        SerializedProperty indicatorEnabled = property.FindPropertyRelative("indicatorEnabled");
        EditorGUI.PropertyField(fieldPosition, indicatorEnabled);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        SerializedProperty indicatorPrefab = property.FindPropertyRelative("indicatorPrefab");
        EditorGUI.PropertyField(fieldPosition, indicatorPrefab);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        SerializedProperty grabInteractable = property.FindPropertyRelative("grabInteractable");
        EditorGUI.PropertyField(fieldPosition, grabInteractable);
        fieldPosition.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

        // End property drawing
        EditorGUI.EndProperty();
    }
}
