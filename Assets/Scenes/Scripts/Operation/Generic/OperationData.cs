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


    //[Header("Direction")]
    public Asse rotationBolt;
    public Asse directionBolt;


    public AttachMode attachMode;
    public GameObject body;
};

public enum Asse { XLeft, XRight, YUp, YDown, ZForward, ZBack, }

public enum AttachMode
{
    RigidConnection,     
    LooseConnection      
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
    OperationEnableXRGrab,
    LightIndicator
}
