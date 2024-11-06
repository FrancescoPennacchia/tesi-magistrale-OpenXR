using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class OperationData
{
    //[Header("Required input")]
    public OperationType operationType;
    public string instructionMessage;
    public GameObject targetObject;

    //[Header("Required for Attach e XRSocket Menager")]
    public XRSocketInteractor socketInteractor;

    //[Header("If require indicator")]
    public bool indicatorEnabled;
    public GameObject indicatorPrefab;

    //[Header("Rquired for XRGrabInteractable")]
    public XRGrabInteractable grabInteractable;
}

public enum OperationType
{
    AttachObject,
    UnscrewBolt,
    ScrewBolt,
    Destroy,
    OperationSocketDisable,
    OperationSocketActivation,
    OperationDisableXRGrab,
    OperationEnableXRGrab
}
