using UnityEngine;

public class BoltTriggerHandler : MonoBehaviour
{
    public UnscrewBoltOperation unscrewBoltOperation;
    public ScrewBoltOperation screwBoltOperation;

    private void OnTriggerEnter(Collider other)
    {
        if (unscrewBoltOperation != null && !unscrewBoltOperation.IsOperationComplete())
        {
            unscrewBoltOperation.HandleTriggerEnter(other);
        }
        else if (screwBoltOperation != null && !screwBoltOperation.IsOperationComplete())
        {
            screwBoltOperation.HandleTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (unscrewBoltOperation != null && !unscrewBoltOperation.IsOperationComplete())
        {
           unscrewBoltOperation.HandleTriggerExit(other);
        }
        else if (screwBoltOperation != null && !screwBoltOperation.IsOperationComplete())
        {
           screwBoltOperation.HandleTriggerExit(other);
        }
    }
}
