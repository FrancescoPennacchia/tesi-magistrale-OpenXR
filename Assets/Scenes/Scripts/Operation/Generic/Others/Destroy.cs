using UnityEngine;

public class Destroy : BaseOperation
{
    public GameObject destroyObj;

    private bool isDestroyed = false;

    public override void StartOperation()
    {
        if (destroyObj != null)
        {
            Destroy(destroyObj);
            isDestroyed = true;
        }
        else
        {
            Debug.LogWarning("destroyObj è null, impossibile distruggere l'oggetto.");
        }
    }

    public override bool IsOperationComplete()
    {
        return isDestroyed;
    }
}