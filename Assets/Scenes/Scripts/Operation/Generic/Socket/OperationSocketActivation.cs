using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OperationSocketActivation : BaseOperation
{
    public XRSocketInteractor socketInteractor;

    private bool isActive = false;

    public override void StartOperation()
    {

        if (socketInteractor != null)
        {
            socketInteractor.enabled = true;
            isActive = true;
        }
        else
        {
            Debug.LogWarning("socketInteractor è null");
        }
    }

    public override bool IsOperationComplete()
    {
        return isActive;
    }
}
