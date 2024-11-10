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
        SerializedProperty rotationBolt = property.FindPropertyRelative("rotationBolt");
        SerializedProperty directionBolt = property.FindPropertyRelative("directionBolt");

    // Display each field
    EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), operationType);
        position.y += EditorGUIUtility.singleLineHeight + 2;

        switch ((OperationType)operationType.enumValueIndex)
        {
            case OperationType.AttachObject:
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
                break;

            case OperationType.UnscrewBolt:
            case OperationType.ScrewBolt:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), instructionMessage);
                position.y += EditorGUIUtility.singleLineHeight + 2;

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), targetObject);
                position.y += EditorGUIUtility.singleLineHeight + 2;

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorEnabled);
                position.y += EditorGUIUtility.singleLineHeight + 2;

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorPrefab);
                position.y += EditorGUIUtility.singleLineHeight + 2;

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), directionBolt);
                position.y += EditorGUIUtility.singleLineHeight + 2;

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), rotationBolt);
                position.y += EditorGUIUtility.singleLineHeight + 2;
                break;

            case OperationType.Destroy:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), targetObject);
                position.y += EditorGUIUtility.singleLineHeight + 2;
                break;

            case OperationType.OperationSocketDisable:
            case OperationType.OperationSocketActivation:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), socketInteractor);
                position.y += EditorGUIUtility.singleLineHeight + 2;
                break;

            case OperationType.OperationDisableXRGrab:
            case OperationType.OperationEnableXRGrab:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), grabInteractable);
                position.y += EditorGUIUtility.singleLineHeight + 2;
                break;

            case OperationType.LightIndicator:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), indicatorPrefab);
                position.y += EditorGUIUtility.singleLineHeight + 2;
                break;

            default:
                break;
        }


        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate the height dynamically based on the fields displayed
        float height = EditorGUIUtility.singleLineHeight * 9 + 10; // Default height for 6 fields

        SerializedProperty operationType = property.FindPropertyRelative("operationType");
        if (operationType.enumValueIndex == (int)OperationType.OperationDisableXRGrab ||
            operationType.enumValueIndex == (int)OperationType.OperationEnableXRGrab)
        {
            height += EditorGUIUtility.singleLineHeight + 2; // Add height for grabInteractable
        }

        return height;
    }
}