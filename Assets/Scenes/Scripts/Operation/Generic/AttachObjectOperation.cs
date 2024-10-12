using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
public class AttachObjectOperation : BaseOperation
{
    public GameObject targetObject;
    private bool isCompleted = false;
    private XRSocketInteractor socketInteractor;

    public override void StartOperation()
    {
        // Abilita l'interazione con l'oggetto target
        var interactable = targetObject.GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.enabled = true;
            interactable.selectEntered.AddListener(OnObjectAttached);
        }

        // Aggiungi un indicatore visivo se necessario
    }

    public override bool IsOperationComplete()
    {
        return isCompleted;
    }

    private void OnObjectAttached(SelectEnterEventArgs args)
    {
        isCompleted = true;

        // Disabilita ulteriori interazioni
        var interactable = targetObject.GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnObjectAttached);
            interactable.enabled = false;
        }

        // Rimuovi l'indicatore visivo se presente
    }
}
