using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OperationData", menuName = "ScriptableObjects/OperationData")]
public class OperationData : ScriptableObject
{
    public List<SocketInstruction> instructions;
}

[Serializable]
public class SocketInstruction
{
    public string message;
    public GameObject attachedGameObject;
}
