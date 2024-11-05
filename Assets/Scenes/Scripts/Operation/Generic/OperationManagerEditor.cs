using UnityEditor;
using UnityEngine;

/*
[CustomEditor(typeof(OperationManager))]
public class OperationManagerEditor : Editor
{
    SerializedProperty operationsData;

    private void OnEnable()
    {
        operationsData = serializedObject.FindProperty("operationsData");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(operationsData, new GUIContent("Operations Data"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
*/