using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/*
[CustomEditor(typeof(OperationData))]
public class OperationDataEditor : Editor
{
    public override void OnInspectorGUI()
    { /*
        // Usa serializedObject.targetObject per ottenere l'istanza corretta
        OperationData operationData = (OperationData)serializedObject.targetObject;

        // Renderizza l'Inspector personalizzato
        operationData.operationType = (OperationType)EditorGUILayout.EnumPopup("Operation Type", operationData.operationType);
        operationData.instructionMessage = EditorGUILayout.TextField("Instruction Message", operationData.instructionMessage);
        operationData.targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", operationData.targetObject, typeof(GameObject), true);

        if (operationData.operationType == OperationType.AttachObject)
        {
            operationData.socketInteractor = (XRSocketInteractor)EditorGUILayout.ObjectField("Socket Interactor", operationData.socketInteractor, typeof(XRSocketInteractor), true);
        }

        operationData.indicatorEnabled = EditorGUILayout.Toggle("Indicator Enabled", operationData.indicatorEnabled);

        if (operationData.indicatorEnabled)
        {
            operationData.indicatorPrefab = (GameObject)EditorGUILayout.ObjectField("Indicator Prefab", operationData.indicatorPrefab, typeof(GameObject), true);
        }

        if (operationData.operationType == OperationType.OperationEnableXRGrab || operationData.operationType == OperationType.OperationDisableXRGrab)
        {
            operationData.grabInteractable = (XRGrabInteractable)EditorGUILayout.ObjectField("Grab Interactable", operationData.grabInteractable, typeof(XRGrabInteractable), true);
        }

        // Applica eventuali modifiche fatte nell'Inspector
        if (GUI.changed)
        {
            EditorUtility.SetDirty(operationData);
        }
    }
}*/

