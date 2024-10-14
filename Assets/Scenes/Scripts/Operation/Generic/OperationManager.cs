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
                if(data.indicatorEnabled && data.indicatorPrefab != null)
                {
                    var indicator = Instantiate(data.indicatorPrefab, gameObject.transform);
                    var indicatorScript = indicator.AddComponent<Indicator>();
                    indicatorScript.currentArrow = indicator; // Assegna l'indicatore prefab come freccia
                    indicatorScript.initialPosition = data.targetObject.transform.localPosition;
                }
                return attachOp;

            case OperationType.UnscrewBolt:
                var unscrewOp = gameObject.AddComponent<UnscrewBoltOperation>();
                unscrewOp.bolt = data.targetObject;
                if (data.indicatorEnabled && data.indicatorPrefab != null)
                {
                    var indicator = Instantiate(data.indicatorPrefab, gameObject.transform);
                    var indicatorScript = indicator.AddComponent<Indicator>();
                    indicatorScript.currentArrow = indicator; // Assegna l'indicatore prefab come freccia
                    indicatorScript.initialPosition = data.targetObject.transform.localPosition;
                }
                return unscrewOp;

            case OperationType.ScrewBolt:
                var screwOp = gameObject.AddComponent<ScrewBoltOperation>();
                screwOp.bolt = data.targetObject;
                if (data.indicatorEnabled && data.indicatorPrefab != null)
                {
                    var indicator = Instantiate(data.indicatorPrefab, gameObject.transform);
                    var indicatorScript = indicator.AddComponent<Indicator>();
                    indicatorScript.currentArrow = indicator; // Assegna l'indicatore prefab come freccia
                    indicatorScript.initialPosition = data.targetObject.transform.localPosition;
                }
                return screwOp;

            default:
                throw new NotImplementedException("Tipo di operazione non gestito.");
        }
    }

    private void Update()
    {
        if (currentOperation != null && currentOperation.IsOperationComplete())
        {
            Indicator indicator = currentOperation.GetComponent<Indicator>();
            // Cancella l'indicatore se presente
            if (indicator != null)
            {
                Destroy(indicator.currentArrow); // Cancella la freccia (GameObject)
                Destroy(indicator); // Cancella il componente Indicator
            }
            Destroy(currentOperation);
            currentOperation = null;
            currentOperationIndex++;
            StartNextOperation();
        }
    }
}

