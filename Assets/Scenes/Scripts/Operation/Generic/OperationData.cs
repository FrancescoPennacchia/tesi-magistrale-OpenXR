using UnityEngine;

[CreateAssetMenu(fileName = "OperationData", menuName = "ScriptableObjects/OperationData")]
public class OperationData : ScriptableObject
{
    public string instructionMessage;
    public GameObject targetObject;
    public OperationType operationType;
}

public enum OperationType
{
    AttachObject,
    UnscrewBolt
}

