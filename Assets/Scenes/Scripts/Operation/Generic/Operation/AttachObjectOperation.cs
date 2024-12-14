using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class AttachObjectOperation : BaseOperation
{
    public GameObject targetObject;
    public XRSocketInteractor socketInteractor;

    public AttachMode attachMode;
    private FixedJoint joint;
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
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }

        // Aggiungi listener all'evento selectEntered del socketInteractor
        socketInteractor.selectEntered.AddListener(OnObjectAttached);
        Debug.Log("Listener aggiunto al socketInteractor " + socketInteractor.name);
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
            Transform attachTransform = socketInteractor.attachTransform != null ? socketInteractor.attachTransform : socketInteractor.transform;
            var rigidbody = targetObject.GetComponent<Rigidbody>();

            if (attachMode == AttachMode.RigidConnection)
            {
                targetObject.transform.SetParent(socketInteractor.transform, false);
                targetObject.transform.localPosition = attachTransform.localPosition;
                targetObject.transform.localRotation = attachTransform.localRotation;
                targetObject.transform.localScale = attachTransform.localScale;
            } else
            {
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                    rigidbody.useGravity = false;
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
                
                attachedObject.transform.SetPositionAndRotation(attachTransform.position, attachTransform.rotation);
                attachedObject.transform.SetParent(socketInteractor.transform, true);

                // Aggiungi un punto di ancoraggio (come una cerniera o un vincolo) tra l'oggetto e il socket interactor
                joint = attachedObject.AddComponent<FixedJoint>();
                joint.connectedBody = socketInteractor.GetComponent<Rigidbody>();

                // Aggiorna la posizione e la rotazione dell'oggetto relativamente al socket interactor a ogni frame
                void UpdateTransform()
                {
                    attachedObject.transform.SetPositionAndRotation(
                        attachTransform.position,
                        attachTransform.rotation
                    );
                }
                InvokeRepeating("UpdateTransform", 0f, Time.fixedDeltaTime);
            }

            // Disabilita ulteriori interazioni
            var interactable = targetObject.GetComponent<XRGrabInteractable>();
            if (interactable != null)
            {
                interactable.enabled = false;
                Debug.Log("XRGrabInteractable disabilitato per " + targetObject.name);
            }

            // Configura il Rigidbody dell'oggetto
            if (rigidbody != null)
            {
                if (attachMode == AttachMode.RigidConnection)
                {
                    rigidbody.isKinematic = true;
                    rigidbody.useGravity = false;
                    rigidbody.detectCollisions = true;
                }        
            }

            /*
            Collider collider = targetObject.GetComponent<Collider>();
            if (collider != null)
            {
                // Se vuoi tenerlo come solido, non impostare isTrigger a true.
                //collider.isTrigger = true; // opzionale se vuoi che non faccia collisioni fisiche
                Debug.Log("Collider configurato per " + targetObject.name);
            }
            else
            {
                Debug.LogWarning("Collider non trovato su " + targetObject.name);
            }*/

            socketInteractor.enabled = false;

            // Rimuovi listener
            socketInteractor.selectEntered.RemoveListener(OnObjectAttached);
            Debug.Log("Listener rimosso dal socketInteractor " + socketInteractor.name);

            // Non usiamo AttachWithJoint, nessun Joint viene creato

            isCompleted = true;
            Debug.Log("Oggetto " + targetObject.name + " inserito correttamente nel socket senza Joint.");
        }
        else
        {
            Debug.LogWarning("L'oggetto inserito non è quello previsto.");
        }
    }
}
