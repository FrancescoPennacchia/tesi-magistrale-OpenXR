using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OperationSocketDisable : BaseOperation
{
    public XRSocketInteractor socketInteractor;

    private bool isDisable = false;

    public override void StartOperation()
    {

        if (socketInteractor != null)
        {
            socketInteractor.enabled = false;
            isDisable = true;  
        }
        else
        {
            Debug.LogWarning("socketInteractor � null");
        }
    }

    public override bool IsOperationComplete()
    {
        return isDisable;
    }
}
