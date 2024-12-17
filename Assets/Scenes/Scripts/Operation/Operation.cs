using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

public class Operation : MonoBehaviour
{
    [SerializeField] private List<SocketMessage> socketMessages;
    [SerializeField] private TMP_Text canvasText;
    [SerializeField] private GameObject arrow;

    // Variabili relative alla freccia
    private GameObject currentArrow;
    private Vector3 initialPosition;
    private float oscillationSpeed = 2f; // Velocità dell'oscillazione
    private float oscillationAmplitude = 1f; // Ampiezza dell'oscillazione (in unità di posizione)

    private int currentIndex = 0;


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

            // Add event listener to the attachedGameObject's XRGrabInteractable
            if (interactor.attachedGameObject != null)
            {
                var grabInteractable = interactor.attachedGameObject.GetComponent<XRGrabInteractable>();
                if (grabInteractable != null)
                {
                    // Ensure the XRGrabInteractable is disabled initially
                    grabInteractable.enabled = false;
                    grabInteractable.selectEntered.AddListener(OnObjectGrabbed);
                }

            }
        }
    }

    private void OnDisable()
    {
        foreach (var interactor in socketMessages)
        {
            interactor.socketInteractor.selectEntered.RemoveListener(OnAttach);
            interactor.socketInteractor.selectExited.RemoveListener(OnDetach);

            // Remove event listener from the XRGrabInteractable
            if (interactor.attachedGameObject != null)
            {
                var grabInteractable = interactor.attachedGameObject.GetComponent<XRGrabInteractable>();
                if (grabInteractable != null)
                {
                    grabInteractable.selectEntered.RemoveListener(OnObjectGrabbed);
                }
            }
        }
    }

    private void OnDetach(SelectExitEventArgs args)
    {
        foreach (var interactor in socketMessages)
        {
            if (ReferenceEquals(interactor.socketInteractor, args.interactorObject))
            {
                if (interactor.isLocked)
                {
                    // Prevent detachment by re-selecting the object
                    interactor.socketInteractor.StartManualInteraction(interactor.attachedGameObject.GetComponent<IXRSelectInteractable>());
                    Debug.Log("Non puoi scollegare l'oggetto");
                }
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

    private void DisableSocketInteractor(XRSocketInteractor socketInteractor)
    {
        if (socketInteractor != null)
        {
            socketInteractor.enabled = false;
        }
    }

    private void OnObjectGrabbed(SelectEnterEventArgs args)
    {
        // Remove the arrow when the object is grabbed
        RemoveArrow();
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

                var socketInteractor = socketMessages[currentIndex].socketInteractor;
                if (socketInteractor != null)
                {
                    socketInteractor.enabled = true;
                }
            }
        }
    }

    private void AttachObject(SocketMessage interactor)
    {
        var attachedObject = interactor.attachedGameObject;
        if (attachedObject != null)
        {
            // Disabilita XRGrabInteractable per prevenire ulteriori interazioni
            var grabInteractable = attachedObject.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
                grabInteractable.selectEntered.RemoveListener(OnObjectGrabbed);
            }

            // Configura il Rigidbody
            var rigidbody = attachedObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            // Allinea la posizione e la rotazione dell'oggetto con quella del socket interactor
            Transform attachTransform = interactor.socketInteractor.attachTransform != null ?
                interactor.socketInteractor.attachTransform : interactor.socketInteractor.transform;

            attachedObject.transform.SetPositionAndRotation(attachTransform.position, attachTransform.rotation);

            // Se necessario, genitorizza l'oggetto al socket interactor
            attachedObject.transform.SetParent(interactor.socketInteractor.transform, true);

            //DisableSocketInteractor(interactor.socketInteractor);
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
            initialPosition = currentArrow.transform.localPosition;
            currentArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
            currentArrow.transform.localScale = new Vector3(2000f, 2000f, 2000f);
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

    private void Update()
    {
        if (currentArrow != null)
        {
            // Calcola lo spostamento laterale utilizzando una funzione sinusoidale
            float offset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;

            // Aggiorna la posizione locale della freccia
            currentArrow.transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
        }
    }
}

