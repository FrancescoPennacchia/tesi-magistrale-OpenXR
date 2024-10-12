using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public event Action<GameObject> OnObjectAttached;
    public event Action<GameObject> OnObjectDetached;

    public void AttachObject(GameObject obj)
    {
        // Logica per agganciare l'oggetto
        OnObjectAttached?.Invoke(obj);
    }

    public void DetachObject(GameObject obj)
    {
        // Logica per sganciare l'oggetto
        OnObjectDetached?.Invoke(obj);
    }
}

