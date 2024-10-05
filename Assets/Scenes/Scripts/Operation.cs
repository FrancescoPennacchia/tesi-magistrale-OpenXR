/*using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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

    // Mappa per un accesso più rapido ai SocketMessage corrispondenti
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
            interactor.StartManualInteraction(interactable);
            Debug.Log("Non puoi scollegare l'oggetto");
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
            // Utilizza le funzionalità del XRSocketInteractor per attaccare l'oggetto
            interactor.socketInteractor.StartManualInteraction(attachedObject.GetComponent<IXRSelectInteractable>());
        }
    }
}*/
