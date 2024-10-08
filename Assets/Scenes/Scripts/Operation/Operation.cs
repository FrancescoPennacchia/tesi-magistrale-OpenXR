using UnityEngine;
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

    [SerializeField] private GameObject arrow;

    private GameObject currentArrow;

    private int currentIndex = 0;

    /*void Start()
    {
        UpdateMessage();
        AddArrow(currentIndex);
        EnableCurrentGrabInteractable();
    }*/

    public void NewStart()
    {
        UpdateMessage();
        AddArrow(currentIndex);
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

                        // Rimuovi la freccia quando l'oggetto viene afferrato
                        RemoveArrow();

                        currentIndex++;
                        if (currentIndex < socketMessages.Count)
                        {
                            UpdateMessage();
                            AddArrow(currentIndex);
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
                var grabInteractable = currentObject.GetComponent<XRGrabInteractable>();
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
                if (currentArrow != null)
                {
                    Destroy(currentArrow);
                }
            }
            else
            {
                Debug.LogError("Il GameObject collegato non ha un Rigidbody!");
            }
        }
    }

    private void AddArrow(int index)
    {
        if (index < 0 || index >= socketMessages.Count)
        {
            Debug.LogError("Indice non valido per socketMessages.");
            return;
        }

        var currentObject = socketMessages[index].attachedGameObject;
        if (arrow != null && currentArrow == null && currentObject != null)
        {
            currentArrow = Instantiate(arrow, currentObject.transform.position + Vector3.up * 0.5f, arrow.transform.rotation);
            currentArrow.transform.SetParent(currentObject.transform);
        }
    }

    private void RemoveArrow()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }
}
