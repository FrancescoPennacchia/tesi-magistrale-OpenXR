using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ScriptDistanza : MonoBehaviour
{
    private Vector3 firstPoint;
    private Vector3 secondPoint;
    private bool isFirstPointSet = false;

    void Update()
    {
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool buttonAPressed = false;
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out buttonAPressed) && buttonAPressed)
        {
            if (!isFirstPointSet)
            {
                // Registra la posizione corrente come primo punto
                rightController.TryGetFeatureValue(CommonUsages.devicePosition, out firstPoint);
                isFirstPointSet = true;
                Debug.Log("Primo punto registrato: " + firstPoint);
            }
            else
            {
                // Registra la posizione corrente come secondo punto
                rightController.TryGetFeatureValue(CommonUsages.devicePosition, out secondPoint);
                isFirstPointSet = false; // Resetta per la prossima misurazione
                Debug.Log("Secondo punto registrato: " + secondPoint);

                // Calcola la distanza
                float distanza = Vector3.Distance(firstPoint, secondPoint);
                Debug.Log("La distanza tra i due punti è: " + distanza + " metri");
            }

            // Implementa un debounce per evitare registrazioni multiple
            StartCoroutine(ButtonDebounce());
        }
    }

    private System.Collections.IEnumerator ButtonDebounce()
    {
        enabled = false;
        yield return new WaitForSeconds(0.2f);
        enabled = true;
    }
}
