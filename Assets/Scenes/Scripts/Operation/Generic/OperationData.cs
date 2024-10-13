using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class OperationData
{
    public string instructionMessage;
    [SerializeField, Tooltip("Object to attach")]
    public GameObject targetObject;
    public XRSocketInteractor socketInteractor;
    public OperationType operationType;
}

public enum OperationType
{
    AttachObject,
    UnscrewBolt
}
