using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OperationDisableXRGrab : BaseOperation
{
    public GameObject targetObject;
    public bool isDisable = false;

    public override void StartOperation()
    {

        if (targetObject != null)
        {
            Rigidbody targetObjectRigidbody = targetObject.GetComponent<Rigidbody>();
            targetObjectRigidbody.isKinematic = false;
            targetObjectRigidbody.useGravity = false;

            Collider targetObjectCollider = targetObject.GetComponent<Collider>();
            targetObjectCollider.isTrigger = false;


            XRGrabInteractable grabInteractable = targetObject.GetComponent<XRGrabInteractable>();
            if (grabInteractable == null)
            {
                grabInteractable.enabled = false;
            }

                isDisable = true;
        }
        else
        {
            Debug.LogWarning("targetObject è null");
        }
    }

    public override bool IsOperationComplete()
    {
        return isDisable;
    }
}
