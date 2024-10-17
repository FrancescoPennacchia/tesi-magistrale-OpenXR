using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OperationDisableXRGrab : BaseOperation
{
    public XRGrabInteractable grabInteractable;
    public bool isDisable = false;

    public override void StartOperation()
    {

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            isDisable = true;
        }
        else
        {
            Debug.LogWarning("grabInteractable è null");
        }
    }

    public override bool IsOperationComplete()
    {
        return isDisable;
    }
}
