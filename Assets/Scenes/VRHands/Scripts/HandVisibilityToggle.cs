using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandVisibilityToggle : MonoBehaviour
{
    [SerializeField] private XRBaseInteractor handInteractor; // Utilizza XRBaseInteractor o XRDirectInteractor

    private SkinnedMeshRenderer handModel;
    private bool isGrabbed = false;

    private void Start()
    {
        handModel = GetComponentInChildren<SkinnedMeshRenderer>();

        // Aggiungi listener per eventi di selezione (grab) e rilascio
        handInteractor.selectEntered.AddListener(OnGrab);
        handInteractor.selectExited.AddListener(OnLetGo);
    }

    private void Update()
    {
        // Se l'oggetto è stato preso (grabbed)
        if (isGrabbed)
        {
            // Nascondi il modello della mano quando sta interagendo (vicino)
            if (handModel.enabled)
            {
                handModel.enabled = false;
            }
        }
        else
        {
            // Mostra il modello della mano se non sta interagendo
            if (!handModel.enabled)
            {
                handModel.enabled = true;
            }
        }
    }

    private void OnLetGo(SelectExitEventArgs arg0)
    {
        isGrabbed = false; // Oggetto rilasciato
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        isGrabbed = true;  // Oggetto preso
    }
}

