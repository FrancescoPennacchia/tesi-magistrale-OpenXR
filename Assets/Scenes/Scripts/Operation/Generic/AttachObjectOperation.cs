using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachObjectOperation : BaseOperation
{
    public GameObject targetObject;
    public XRSocketInteractor socketInteractor;

    private bool isCompleted = false;

    public override void StartOperation()
    {
        if (targetObject == null)
        {
            Debug.LogError("TargetObject non assegnato in AttachObjectOperation.");
            return;
        }

        if (socketInteractor == null)
        {
            Debug.LogError("SocketInteractor non assegnato in AttachObjectOperation.");
            return;
        }

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
        socketInteractor.selectEntered.AddListener(OnObjectAttached);
        Debug.Log("Listener aggiunto al socketInteractor " + socketInteractor.name);

        // Opzionale: Disabilita il XRGrabInteractable sul socketInteractor per impedire la rimozione
        // socketInteractor.allowSelect = false;
    }

    public override bool IsOperationComplete()
    {
        return isCompleted;
    }

    private void OnObjectAttached(SelectEnterEventArgs args)
    {
        GameObject attachedObject = args.interactableObject.transform.gameObject;
        Debug.Log("OnObjectAttached chiamato con: " + attachedObject.name);

        // Verifica se l'oggetto inserito è quello target
        if (attachedObject == targetObject)
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

            // Configura il Rigidbody dell'oggetto
            var rigidbody = targetObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                //rigidbody.velocity = Vector3.zero;
                //rigidbody.angularVelocity = Vector3.zero;
                Debug.Log("Rigidbody configurato per " + targetObject.name);
            }

            // Allinea la posizione e la rotazione dell'oggetto con quella del socket interactor
            Transform attachTransform = socketInteractor.attachTransform != null ? socketInteractor.attachTransform : socketInteractor.transform;
            targetObject.transform.SetPositionAndRotation(attachTransform.position, attachTransform.rotation);
            Debug.Log("Posizione e rotazione di " + targetObject.name + " allineate al socket.");

            // Genitorizza l'oggetto al socket interactor
            targetObject.transform.SetParent(socketInteractor.transform, true);
            Debug.Log(targetObject.name + " genitorizzato al socketInteractor.");

            // Rimuovi listener
            socketInteractor.selectEntered.RemoveListener(OnObjectAttached);
            Debug.Log("Listener rimosso dal socketInteractor " + socketInteractor.name);

            // Eventuali azioni post-completamento
        }
        else
        {
            Debug.LogWarning("L'oggetto inserito non è quello previsto.");
        }
    }

    private void OnDestroy()
    {
        // Rimuovi il listener per evitare problemi
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnObjectAttached);
        }
    }
}
