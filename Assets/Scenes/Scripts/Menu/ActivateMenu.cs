using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject menu; // Assegna il menu da disattivare/attivare
    [SerializeField] private InputActionReference toggleMenuLeft;  // Input per il pulsante Y
    [SerializeField] private InputActionReference toggleMenuRight; // Input per il pulsante B

    private void OnEnable()
    {
        toggleMenuLeft.action.performed += ToggleMenu;
        toggleMenuRight.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        toggleMenuLeft.action.performed -= ToggleMenu;
        toggleMenuRight.action.performed -= ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        // Cambia lo stato del menu (attivo/disattivo)
        menu.SetActive(!menu.activeSelf);
    }
}