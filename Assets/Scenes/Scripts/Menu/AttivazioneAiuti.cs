using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttivazioneAiuti : MonoBehaviour
{
    [SerializeField] private GameObject aiuti; // Assegna il menu da disattivare/attivare
    [SerializeField] private InputActionReference toggleAiutiLeft;  // Input per il pulsante X
    [SerializeField] private InputActionReference toggleAiutiRight; // Input per il pulsante A

    private void OnEnable()
    {
        toggleAiutiLeft.action.performed += ToggleAiuti;
        toggleAiutiRight.action.performed += ToggleAiuti;
    }

    private void OnDisable()
    {
        toggleAiutiLeft.action.performed -= ToggleAiuti;
        toggleAiutiRight.action.performed -= ToggleAiuti;
    }

    private void ToggleAiuti(InputAction.CallbackContext context)
    {
        // Cambia lo stato del menu (attivo/disattivo)
        aiuti.SetActive(!aiuti.activeSelf);
    }
}
