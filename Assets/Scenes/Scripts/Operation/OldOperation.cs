/*using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;
using Operation.SocketMessage;

[Serializable]
public class SocketMessage
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    public string message;
    public bool isLocked = false;
    public GameObject attachedGameObject;
}

public class OldOperation : MonoBehaviour
{
    [SerializeField] private List<SocketMessage> socketMessages;

    [SerializeField] private TMP_Text canvasText;

    private int currentIndex = 0;

    void Start()
    {
        UpdateMessage();
        EnableCurrentGrabInteractable();
    }

    private void OnEnable()
    {
        foreach (var interactor in socketMessages)
        {
            interactor.socketInteractor.selectEntered.AddListener(OnAttach);
            interactor.socketInteractor.selectExited.AddListener(OnDetach);
        }
    }

    private void OnDisable()
    {
        foreach (var interactor in socketMessages)
        {
            interactor.socketInteractor.selectEntered.RemoveListener(OnAttach);
            interactor.socketInteractor.selectExited.RemoveListener(OnDetach);
        }
    }

    private void OnDetach(SelectExitEventArgs args)
    {
        foreach (var interactor in socketMessages)
        {
            if (ReferenceEquals(interactor.socketInteractor, args.interactorObject))
            {
                Debug.Log("Non puoi scollegare l'oggetto");
            }
        }
    }

    private void OnAttach(SelectEnterEventArgs args)
    {
        foreach (var interactor in socketMessages)
        {
            if (ReferenceEquals(interactor.socketInteractor, args.interactorObject))
            {
                if (currentIndex < socketMessages.Count)
                {
                    if (!interactor.isLocked)
                    {
                        interactor.isLocked = true;
                        AttachObject(interactor);

                        currentIndex++;
                        if (currentIndex < socketMessages.Count)
                        {
                            UpdateMessage();
                            EnableCurrentGrabInteractable();
                        }
                        else
                        {
                            canvasText.text = "Hai completato le istruzioni da eseguire!";
                        }
                    }
                }
            }
        }
    }

    private void UpdateMessage()
    {
        if (currentIndex < socketMessages.Count)
        {
            canvasText.text = socketMessages[currentIndex].message;
        }
    }

    private void EnableCurrentGrabInteractable()
    {
        if (currentIndex < socketMessages.Count)
        {
            var currentObject = socketMessages[currentIndex].attachedGameObject;
            if (currentObject != null)
            {
                var grabInteractable = currentObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                if (grabInteractable != null)
                {
                    grabInteractable.enabled = true;
                }
            }
        }
    }

    private void AttachObject(SocketMessage interactor)
    {
        var attachedObject = interactor.attachedGameObject;
        if (attachedObject != null)
        {
            var fixedJoint = attachedObject.AddComponent<FixedJoint>();
            var connectedBody = interactor.socketInteractor.GetComponent<Rigidbody>();
            if (connectedBody != null)
            {
                fixedJoint.connectedBody = connectedBody;
            }
            else
            {
                Debug.LogError("Il GameObject collegato non ha un Rigidbody!");
            }
        }
    }
}*/

/*
 * using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

[Serializable]
public class SocketMessage
{
    public XRSocketInteractor socketInteractor;
    public string message;
    public bool isLocked = false;
    public GameObject attachedGameObject;
}

public class Operation : MonoBehaviour
{
    [SerializeField] private List<SocketMessage> socketMessages;
    [SerializeField] private TMP_Text canvasText;

    private int currentIndex = 0;

    // Mappa per un accesso pi� rapido ai SocketMessage corrispondenti
    private Dictionary<XRBaseInteractor, SocketMessage> interactorToSocketMessageMap;

    private void Start()
    {
        // Inizializza la mappa
        interactorToSocketMessageMap = new Dictionary<XRBaseInteractor, SocketMessage>();
        foreach (var socketMessage in socketMessages)
        {
            interactorToSocketMessageMap[socketMessage.socketInteractor] = socketMessage;
        }

        UpdateMessage();
        EnableCurrentGrabInteractable();
    }

    private void OnEnable()
    {
        foreach (var interactor in socketMessages)
        {
            interactor.socketInteractor.selectEntered.AddListener(OnAttach);
            interactor.socketInteractor.selectExited.AddListener(OnDetach);
        }
    }

    private void OnDisable()
    {
        foreach (var interactor in socketMessages)
        {
            interactor.socketInteractor.selectEntered.RemoveListener(OnAttach);
            interactor.socketInteractor.selectExited.RemoveListener(OnDetach);
        }
    }

    private void OnDetach(SelectExitEventArgs args)
    {
        // Impedisce lo scollegamento dell'oggetto ricollegandolo immediatamente
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        IXRSelectInteractable interactable = args.interactableObject;

        if (interactor != null && interactable != null)
        {
            // Disattiva temporaneamente il parent per evitare errori
            var parentGameObject = interactable.transform.parent.gameObject;
            parentGameObject.SetActive(false);

            interactor.StartManualInteraction(interactable);
            Debug.Log("Non puoi scollegare l'oggetto");

            // Riattiva il parent dopo aver ricollegato l'oggetto
            parentGameObject.SetActive(true);
        }
    }

    private void OnAttach(SelectEnterEventArgs args)
    {
        if (interactorToSocketMessageMap.TryGetValue(args.interactorObject as XRBaseInteractor, out SocketMessage interactor))
        {
            if (currentIndex < socketMessages.Count && !interactor.isLocked)
            {
                interactor.isLocked = true;
                AttachObject(interactor);

                currentIndex = Mathf.Min(currentIndex + 1, socketMessages.Count);

                if (currentIndex < socketMessages.Count)
                {
                    UpdateMessage();
                    EnableCurrentGrabInteractable();
                }
                else
                {
                    canvasText.text = "Hai completato le istruzioni da eseguire!";
                    // Puoi aggiungere ulteriori azioni qui
                }
            }
        }
    }

    private void UpdateMessage()
    {
        if (currentIndex < socketMessages.Count)
        {
            canvasText.text = socketMessages[currentIndex].message;
        }
    }

    private void EnableCurrentGrabInteractable()
    {
        foreach (var socketMessage in socketMessages)
        {
            var grabInteractable = socketMessage.attachedGameObject.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                // Abilita solo l'oggetto corrente
                grabInteractable.enabled = (socketMessage == socketMessages[currentIndex]);
            }
        }
    }

    private void AttachObject(SocketMessage interactor)
    {
        var attachedObject = interactor.attachedGameObject;
        if (attachedObject != null)
        {
            // Utilizza le funzionalit� del XRSocketInteractor per attaccare l'oggetto
            interactor.socketInteractor.StartManualInteraction(attachedObject.GetComponent<IXRSelectInteractable>());
        }
    }
}

 */