using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;
using System.IO;

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


    [SerializeField] private string operationName = "robot";

    //Timer operazioni
    private float totalOperationTimer = 0f;
    private List<float> stepsTimer;
    private bool startTimer = false;
    private string filePath;


    public void NewStart()
    {
        startTimer = true;
        stepsTimer = new List<float>(new float[socketMessages.Count]);
        filePath = Application.persistentDataPath + "/" + operationName + "/timerData.txt";
        stepsTimer[currentIndex] = 0f;

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
                            stepsTimer[currentIndex] = 0f;
                            UpdateMessage();
                            AddArrow(currentIndex);
                            EnableCurrentGrabInteractable();
                        }
                        else
                        {
                            filePath = Application.persistentDataPath + "/" + operationName + "/timerData.txt";

                            startTimer = false;

                            // Ensure file exists
                            if (!File.Exists(filePath))
                            {
                                Debug.Log("File non trovato, lo creo...");
                                CreateFile();
                            }

                            // Prepare data to save
                            string saveData = PrepareTimerDataForSaving();
                            SaveData(saveData);


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

    private string PrepareTimerDataForSaving()
    {
        // Create a formatted string with total and individual operation times
        string timersData = "Tempo totale: " + totalOperationTimer.ToString("F2") + " secondi\n";

        for (int i = 0; i < stepsTimer.Count; i++)
        {
            timersData += $"Operazione {i}: {stepsTimer[i].ToString("F2")} secondi\n";
        }

        return timersData;
    }

    private void CreateFile()
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log("Directory creata: " + directory);
        }

        File.WriteAllText(filePath, string.Empty); // Crea un file vuoto
        Debug.Log("File creato con successo.");
    }


    private void SaveData(string data)
    {
        string timestampedData = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {data}\n";
        File.AppendAllText(filePath, timestampedData);
    }

    public void StartTimer()
    {
        startTimer = true;
    }

    public void StopTimer()
    {
        startTimer = false;
    }

    private void Update()
    {
        if (startTimer && currentIndex < socketMessages.Count)
        {
            totalOperationTimer += Time.deltaTime;
            stepsTimer[currentIndex] += Time.deltaTime;
        }

        if (currentArrow != null)
        {
            // Calcola lo spostamento laterale utilizzando una funzione sinusoidale
            float offset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;

            // Aggiorna la posizione locale della freccia
            currentArrow.transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
        }
    }
}

