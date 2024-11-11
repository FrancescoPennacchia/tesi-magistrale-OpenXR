using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OperationEnableXRGrab : BaseOperation
{
    public GameObject targetObject;
    public bool isEnable= false;

    public override void StartOperation()
    {
        Rigidbody targetObjectRigidbody = targetObject.GetComponent<Rigidbody>();
        targetObjectRigidbody.isKinematic = false;
        targetObjectRigidbody.useGravity = true;

        Collider targetObjectCollider = targetObject.GetComponent<Collider>();
        targetObjectCollider.isTrigger = false;


        XRGrabInteractable grabInteractable = targetObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = targetObject.AddComponent<XRGrabInteractable>();
        }
        grabInteractable.enabled = true;
        isEnable = true;
    }

    public override bool IsOperationComplete()
    {
        return isEnable;
    }
}
