using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using System.IO;


public class OperationManager : MonoBehaviour
{
    [SerializeField] private List<OperationData> operationsData;
    [SerializeField] private TMP_Text instructionText;

    private BaseOperation currentOperation;
    private int currentOperationIndex = 0;

    //Info_operazione
    [SerializeField] private string operationName;

    //Timer operazioni
    private float totalOperationTimer;
    private List<float> stepsTimer;
    private bool startTimer = false;
    private string filePath;

    

    private void Start()
    {
        stepsTimer = new List<float>(new float[operationsData.Count]);
        StartNextOperation();
    }

    private void StartNextOperation()
    {
        if (currentOperationIndex < operationsData.Count)
        {
            var data = operationsData[currentOperationIndex];
            instructionText.text = data.instructionMessage;

            currentOperation = CreateOperation(data);
            currentOperation.StartOperation();

            //Timer singola operazione
            stepsTimer[currentOperationIndex] = 0f;


            Debug.Log("Operazione " + currentOperationIndex + " avviata: " + data.instructionMessage);
        }
        else
        {
            // Prepare file path
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

            Debug.Log("Tempo totale trascorso: " + totalOperationTimer.ToString("F2") + " secondi");
            instructionText.text = "Hai completato tutte le operazioni!";
            Debug.Log("Tutte le operazioni sono state completate.");
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

    private BaseOperation CreateOperation(OperationData data)
    {
        // Crea un nuovo GameObject per l'operazione
        GameObject operationObject = new GameObject("Operation_" + data.operationType + "_" + currentOperationIndex);
        operationObject.transform.parent = this.transform; // Genitorizza all'OperationManager

        BaseOperation operation = null;

        switch (data.operationType)
        {
            case OperationType.AttachObject:
                var attachOp = operationObject.AddComponent<AttachObjectOperation>();
                attachOp.targetObject = data.targetObject;
                attachOp.socketInteractor = data.socketInteractor;
                attachOp.attachMode = data.attachMode;
                attachOp.body = data.body;
                operation = attachOp;
                break;

            case OperationType.UnscrewBolt:
                var unscrewOp = operationObject.AddComponent<UnscrewBoltOperation>();
                unscrewOp.bolt = data.targetObject;
                unscrewOp.rotationBolt = data.rotationBolt;
                unscrewOp.directionBolt = data.directionBolt;
                operation = unscrewOp;
                break;

            case OperationType.ScrewBolt:
                var screwOp = operationObject.AddComponent<ScrewBoltOperation>();
                screwOp.bolt = data.targetObject;
                screwOp.rotationBolt = data.rotationBolt;
                screwOp.directionBolt = data.directionBolt;
                operation = screwOp;
                break;

            case OperationType.Destroy:
                var destroyOp = operationObject.AddComponent<Destroy>();
                destroyOp.destroyObj = data.targetObject;
                operation = destroyOp;
                break;

            case OperationType.OperationSocketDisable:
                var socketDisableOp = operationObject.AddComponent<OperationSocketDisable>();
                socketDisableOp.socketInteractor = data.socketInteractor;
                operation = socketDisableOp;
                break;

            case OperationType.OperationSocketActivation:
                var socketActivationOp = operationObject.AddComponent<OperationSocketActivation>();
                socketActivationOp.socketInteractor = data.socketInteractor;
                operation = socketActivationOp;
                break;

            case OperationType.OperationDisableXRGrab:
                var disableGrabOp = operationObject.AddComponent<OperationDisableXRGrab>();
                disableGrabOp.targetObject = data.targetObject;
                operation = disableGrabOp;
                break;

            case OperationType.OperationEnableXRGrab:
                var enableGrabOp = operationObject.AddComponent<OperationEnableXRGrab>();
                enableGrabOp.targetObject = data.targetObject;
                operation = enableGrabOp;
                break;
            case OperationType.LightIndicator:
                var lightIndicatorOp = operationObject.AddComponent<LightIndicator>();
                lightIndicatorOp.indicatorPrefab = data.indicatorPrefab;
                operation = lightIndicatorOp;
                break;

            default:
                throw new NotImplementedException("Tipo di operazione non gestito.");
        }

        // Aggiungi l'Indicator all'operationObject se necessario
        if (data.indicatorEnabled && data.indicatorPrefab != null)
        {
            var indicator = operationObject.AddComponent<Indicator>();
            indicator.arrowPrefab = data.indicatorPrefab; // Assegna il prefab della freccia
            indicator.gameTarget = data.targetObject;     // Assegna l'oggetto target
        }

        return operation;
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

    /*
    private void SaveData(string data)
    {
        string timestampedData = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {data}\n";
        File.AppendAllText(filePath, timestampedData);
    }*/

    private void SaveData(string data)
    {
        var saveData = new
        {
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            totalOperationTime = totalOperationTimer,
            stepsTimer = stepsTimer
        };

        string jsonData = JsonUtility.ToJson(saveData, true);

        // Aggiungi un separatore opzionale (ad esempio, per distinguere i salvataggi)
        string separator = "\n---\n";

        // Aggiungi il JSON al file esistente
        File.AppendAllText(filePath, jsonData + separator);

        Debug.Log("Nuovi dati aggiunti al file in formato JSON.");
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
        if (startTimer && !currentOperation.IsOperationComplete())
        {
            totalOperationTimer += Time.deltaTime;
            stepsTimer[currentOperationIndex] += Time.deltaTime;
        }
        
        // Controlla se l'operazione corrente stata completata
        if (currentOperation != null && currentOperation.IsOperationComplete())
        {
            Indicator indicator = currentOperation.gameObject.GetComponent<Indicator>();
            if (indicator != null)
            {
                indicator.DestroyArrow(); // Distrugge la freccia e rimuove il componente
            }

            //Destroy(currentOperation.gameObject); // Distrugge l'operationObject intero
            currentOperation = null;
            currentOperationIndex++;
            StartNextOperation();
        }
    }
}