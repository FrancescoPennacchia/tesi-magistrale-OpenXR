using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

[Serializable]
public class SocketMessage
{
    public XRSocketInteractor socketInteractor;
    public string message;
    public bool isLocked = false;
    public GameObject attachedGameObject;
}
