using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicator : BaseOperation
{
    public GameObject indicatorPrefab;

    public override bool IsOperationComplete()
    {
        // Puoi aggiungere logica qui se necessario
        return true;
    }

    public override void StartOperation()
    {
        if (indicatorPrefab == null)
        {
            Debug.LogWarning("indicatorPrefab è null");
            return;
        }

        // Inverti lo stato attivo del prefab
        indicatorPrefab.SetActive(!indicatorPrefab.activeSelf);

        // Log per tracciare lo stato
        Debug.Log($"indicatorPrefab è ora {(indicatorPrefab.activeSelf ? "attivo" : "disattivo")}");
    }
}
