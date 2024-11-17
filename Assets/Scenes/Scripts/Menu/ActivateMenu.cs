using UnityEngine;
using UnityEngine.XR;

public class ActivateMenu : MonoBehaviour
{
    public GameObject menu;

    public void Update()
    {
        if (IsButtonPressed(XRNode.RightHand, CommonUsages.secondaryButton) 
            || IsButtonPressed(XRNode.LeftHand, CommonUsages.secondaryButton))
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    private bool IsButtonPressed(XRNode hand, InputFeatureUsage<bool> button)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(hand);
        bool isPressed = false;
        if (device.TryGetFeatureValue(button, out isPressed) && isPressed)
        {
            return true;
        }
        return false;
    }
}
