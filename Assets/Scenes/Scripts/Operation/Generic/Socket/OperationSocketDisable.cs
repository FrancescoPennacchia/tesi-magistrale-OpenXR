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

            //Aggiunto per evitare problemi con rigidbody però perdo il collider.
            BoxCollider box = socketInteractor.gameObject.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.enabled = false;
            }
            socketInteractor.gameObject.SetActive(false);
            isDisable = true;  
        }
        else
        {
            Debug.LogWarning("socketInteractor è null");
        }
    }

    public override bool IsOperationComplete()
    {
        return isDisable;
    }
}
