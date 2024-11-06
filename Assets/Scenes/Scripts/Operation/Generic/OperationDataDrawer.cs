using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(OperationData))]
public class OperationDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Indent to make it look nested
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel++;

        // Set label width to reduce space between label and input field
        float originalLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 150;

        // Get properties of OperationData
        SerializedProperty operationType = property.FindPropertyRelative("operationType");
        SerializedProperty instructionMessage = property.FindPropertyRelative("instructionMessage");
        SerializedProperty targetObject = property.FindPropertyRelative("targetObject");
        SerializedProperty socketInteractor = property.FindPropertyRelative("socketInteractor");
        SerializedProperty indicatorEnabled = property.FindPropertyRelative("indicatorEnabled");
        SerializedProperty indicatorPrefab = property.FindPropertyRelative("indicatorPrefab");
        SerializedProperty grabInteractable = property.FindPropertyRelative("grabInteractable");

        // Display each field
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), operationType);
        position.y += EditorGUIUtility.singleLineHeight + 2;
      
        if (operationType.enumValueIndex == (int)OperationType.AttachObject)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), instructionMessage);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), targetObject);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), socketInteractor);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorEnabled);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorPrefab);
            position.y += EditorGUIUtility.singleLineHeight + 2;
        } else if (operationType.enumValueIndex == (int)OperationType.UnscrewBolt || operationType.enumValueIndex == (int)OperationType.ScrewBolt)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), instructionMessage);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), targetObject);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorEnabled);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorPrefab);
            position.y += EditorGUIUtility.singleLineHeight + 2;
        } else if (operationType.enumValueIndex == (int)OperationType.Destroy)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), targetObject);
            position.y += EditorGUIUtility.singleLineHeight + 2;
        } else if (operationType.enumValueIndex == (int)OperationType.OperationSocketDisable || operationType.enumValueIndex == (int)OperationType.OperationSocketActivation)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), socketInteractor);
            position.y += EditorGUIUtility.singleLineHeight + 2;
        } else if (operationType.enumValueIndex == (int)OperationType.OperationDisableXRGrab || operationType.enumValueIndex == (int)OperationType.OperationEnableXRGrab)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), grabInteractable);
            position.y += EditorGUIUtility.singleLineHeight + 2;
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate the height dynamically based on the fields displayed
        float height = EditorGUIUtility.singleLineHeight * 6 + 10; // Default height for 6 fields

        SerializedProperty operationType = property.FindPropertyRelative("operationType");
        if (operationType.enumValueIndex == (int)OperationType.OperationDisableXRGrab ||
            operationType.enumValueIndex == (int)OperationType.OperationEnableXRGrab)
        {
            height += EditorGUIUtility.singleLineHeight + 2; // Add height for grabInteractable
        }

        return height;
    }
}