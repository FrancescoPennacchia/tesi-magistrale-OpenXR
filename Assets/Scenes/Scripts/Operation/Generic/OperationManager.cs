using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class OperationManager : MonoBehaviour
{
    [SerializeField] private List<OperationData> operationsData;
    [SerializeField] private TMP_Text instructionText;

    private BaseOperation currentOperation;
    private int currentOperationIndex = 0;

    private void Start()
    {
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

            Debug.Log("Operazione " + currentOperationIndex + " avviata: " + data.instructionMessage);
        }
        else
        {
            instructionText.text = "Hai completato tutte le operazioni!";
            Debug.Log("Tutte le operazioni sono state completate.");
        }
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


    private void Update()
    {
        if (currentOperation != null && currentOperation.IsOperationComplete())
        {
            Indicator indicator = currentOperation.gameObject.GetComponent<Indicator>();
            if (indicator != null)
            {
                indicator.DestroyArrow(); // Distrugge la freccia e rimuove il componente
            }

            Destroy(currentOperation.gameObject); // Distrugge l'operationObject intero
            currentOperation = null;
            currentOperationIndex++;
            StartNextOperation();
        }
    }
}