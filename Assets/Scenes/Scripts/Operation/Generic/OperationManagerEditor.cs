#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(OperationManager))]
public class OperationManagerEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        SerializedProperty operationsData = serializedObject.FindProperty("operationsData");

        reorderableList = new ReorderableList(serializedObject, operationsData, true, true, true, true)
        {
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Operations Data"),
            elementHeightCallback = index =>
            {
                SerializedProperty element = operationsData.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element);
            },
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = operationsData.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, new GUIContent($"Element {index}"), true);
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // Draw the reorderable list
        reorderableList.DoLayoutList();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("instructionText"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif