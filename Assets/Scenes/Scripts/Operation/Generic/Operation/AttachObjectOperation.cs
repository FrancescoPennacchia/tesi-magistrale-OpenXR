using Unity.VisualScripting;
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


        if (!socketInteractor.isActiveAndEnabled)
        {
            if (!socketInteractor.gameObject.activeSelf)
            {
                socketInteractor.gameObject.SetActive(true);
                Debug.Log("Attivo il GameObject del Socket Interactor: " + socketInteractor.gameObject.name);
            }
            socketInteractor.enabled = true;
            Debug.Log("Attivo il Socket Interactor: " + socketInteractor);
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

        BoxCollider boxCollider = socketInteractor.gameObject.GetComponent<BoxCollider>();
        if(boxCollider != null)
        {
            boxCollider.enabled = true;
        }

   

        // Aggiungi listener all'evento selectEntered del socketInteractor
        socketInteractor.selectEntered.AddListener(OnObjectAttached);
        Debug.Log("Listener aggiunto al socketInteractor " + socketInteractor.name);

        // Opzionale: Disabilita il XRGrabInteractable sul socketInteractor per impedire la rimozione
        //socketInteractor.allowSelect = false;
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
            
            // Allinea la posizione e la rotazione dell'oggetto con quella del socket interactor
            Transform attachTransform = socketInteractor.attachTransform != null ? socketInteractor.attachTransform : socketInteractor.transform;
      
            targetObject.transform.SetParent(socketInteractor.transform, false);
            targetObject.transform.localPosition = attachTransform.localPosition;
            targetObject.transform.localRotation = attachTransform.localRotation;
            targetObject.transform.localScale = attachTransform.localScale;



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
                rigidbody.detectCollisions = false;
                Debug.Log("Rigidbody configurato per " + targetObject.name);
            }
            else
            {
                Debug.LogWarning("Rigidbody non trovato su " + targetObject.name);
            }


            
            Collider Collider = targetObject.GetComponent<Collider>();
            if(Collider != null)
            {
                Collider.isTrigger = true;
                Debug.Log("Collider impostato su IsTrigger per " + targetObject.name);
            } else
            {
                Debug.LogWarning("Collider non trovato su " + targetObject.name);
            }


            
            // Genitorizza l'oggetto al socket interactor
            //targetObject.transform.SetParent(socketInteractor.transform, true);
            //Debug.Log(targetObject.name + " genitorizzato al socketInteractor.");
            socketInteractor.enabled = false;

            // Rimuovi listener
            socketInteractor.selectEntered.RemoveListener(OnObjectAttached);
            Debug.Log("Listener rimosso dal socketInteractor " + socketInteractor.name);


            // Disabilita il socket interactor
            /*
            socketInteractor.enabled = false;
            socketInteractor.gameObject.SetActive(false);
            BoxCollider box = socketInteractor.gameObject.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.enabled = false;
               Debug.Log("Socket disabilitato correttamente.");
            }
           
            */

            isCompleted = true;
            Debug.Log("Oggetto " + targetObject.name + " inserito correttamente nel socket.");
        }
        else
        {
            Debug.LogWarning("L'oggetto inserito non è quello previsto.");
        }
    }
}
