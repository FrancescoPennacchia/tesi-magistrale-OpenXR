using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class OperationData
{
    [Header("Required input")]
    public OperationType operationType;
    public string instructionMessage;
    public GameObject targetObject;

    [Header("Required for Attach")]
    public XRSocketInteractor socketInteractor;

    [Header("If require indicator")]
    public bool indicatorEnabled;
    public GameObject indicatorPrefab;
}

public enum OperationType
{
    AttachObject,
    UnscrewBolt,
    ScrewBolt,
    Destroy
}
