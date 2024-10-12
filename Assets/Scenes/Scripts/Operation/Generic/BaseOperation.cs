using UnityEngine;


public abstract class BaseOperation : MonoBehaviour
{
    public string instructionMessage;

    // Metodo per avviare l'operazione
    public abstract void StartOperation();

    // Metodo per verificare se l'operazione è stata completata
    public abstract bool IsOperationComplete();
}