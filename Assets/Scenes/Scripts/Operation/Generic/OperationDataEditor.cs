using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/*
[CustomEditor(typeof(OperationData))]
public class OperationDataEditor : Editor
{
    #region Serialized Properties
    SerializedProperty operationType;
    SerializedProperty instructionMessage;
    SerializedProperty targetObject;
    SerializedProperty socketInteractor;
    SerializedProperty indicatorEnabled;
    SerializedProperty indicatorPrefab;
    SerializedProperty grabInteractable;
    #endregion

    private void OnEnable()
    {
        Debug.Log("OnEnable called");
        operationType = serializedObject.FindProperty("operationType");
        instructionMessage = serializedObject.FindProperty("instructionMessage");
        targetObject = serializedObject.FindProperty("targetObject");
        socketInteractor = serializedObject.FindProperty("socketInteractor");
        indicatorEnabled = serializedObject.FindProperty("indicatorEnabled");
        indicatorPrefab = serializedObject.FindProperty("indicatorPrefab");
        grabInteractable = serializedObject.FindProperty("grabInteractable");
    }

    public override void OnInspectorGUI()
    {
        Debug.Log("OnInspectorGUI called");
        serializedObject.Update();

        // Display properties
        EditorGUILayout.PropertyField(operationType);
        EditorGUILayout.PropertyField(instructionMessage);
        EditorGUILayout.PropertyField(targetObject);
        EditorGUILayout.PropertyField(socketInteractor);
        EditorGUILayout.PropertyField(indicatorEnabled);
        EditorGUILayout.PropertyField(indicatorPrefab);

        // Conditional display for grabInteractable
        OperationType currentOperationType = (OperationType)operationType.enumValueIndex;
        Debug.Log("Current Operation Type: " + currentOperationType);

        if (currentOperationType == OperationType.OperationDisableXRGrab ||
            currentOperationType == OperationType.OperationEnableXRGrab)
        {
            EditorGUILayout.PropertyField(grabInteractable);
        }

        serializedObject.ApplyModifiedProperties();
    }
}*/


