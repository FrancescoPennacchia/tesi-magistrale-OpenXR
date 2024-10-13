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
        switch (data.operationType)
        {
            case OperationType.AttachObject:
                var attachOp = gameObject.AddComponent<AttachObjectOperation>();
                attachOp.targetObject = data.targetObject;
                attachOp.socketInteractor = data.socketInteractor;
                return attachOp;

            case OperationType.UnscrewBolt:
                var unscrewOp = gameObject.AddComponent<UnscrewBoltOperation>();
                unscrewOp.bolt = data.targetObject;
                return unscrewOp;

            default:
                throw new NotImplementedException("Tipo di operazione non gestito.");
        }
    }

    private void Update()
    {
        if (currentOperation != null && currentOperation.IsOperationComplete())
        {
            Destroy(currentOperation);
            currentOperation = null;
            currentOperationIndex++;
            StartNextOperation();
        }
    }
}

