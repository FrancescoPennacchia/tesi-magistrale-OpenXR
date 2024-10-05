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
