using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachObjectOperation : BaseOperation
{
    public GameObject targetObject;
    public XRSocketInteractor socketInteractor;
    private bool isCompleted = false;

    public override void StartOperation()
    {
        if (targetObject != null)
        {
            // Abilita l'interazione con l'oggetto target
            var interactable = targetObject.GetComponent<XRGrabInteractable>();
            if (interactable != null)
            {
                interactable.enabled = true;
                Debug.Log("XRGrabInteractable abilitato per " + targetObject.name);
            }
            else
            {
                Debug.LogError("XRGrabInteractable non trovato su " + targetObject.name);
            }

            // Aggiungi listener all'evento selectEntered del socketInteractor
            if (socketInteractor != null)
            {
                socketInteractor.selectEntered.AddListener(OnObjectAttached);
                Debug.Log("Listener aggiunto al socketInteractor " + socketInteractor.name);
            }
            else
            {
                Debug.LogError("SocketInteractor non assegnato in AttachObjectOperation.");
            }
        }
        else
        {
            Debug.LogError("TargetObject non assegnato in AttachObjectOperation.");
        }

        // Aggiungi un indicatore visivo se necessario
    }

    public override bool IsOperationComplete()
    {
        return isCompleted;
    }

    private void OnObjectAttached(SelectEnterEventArgs args)
    {
        // Verifica se l'oggetto inserito è quello target
        if (args.interactableObject.transform.gameObject == targetObject)
        {
            isCompleted = true;
            Debug.Log("Oggetto " + targetObject.name + " inserito correttamente nel socket.");

            // Disabilita ulteriori interazioni
            var interactable = targetObject.GetComponent<XRGrabInteractable>();
            if (interactable != null)
            {
                interactable.enabled = false;
                Debug.Log("XRGrabInteractable disabilitato per " + targetObject.name);
            }

            // Rimuovi listener
            if (socketInteractor != null)
            {
                socketInteractor.selectEntered.RemoveListener(OnObjectAttached);
                Debug.Log("Listener rimosso dal socketInteractor " + socketInteractor.name);
            }

            // Rimuovi l'indicatore visivo se presente

            // Eventuale logica aggiuntiva al completamento dell'operazione
        }
    }
}
