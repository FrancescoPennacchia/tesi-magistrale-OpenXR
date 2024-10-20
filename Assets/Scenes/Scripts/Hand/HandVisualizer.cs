using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandVisualizer : MonoBehaviour
{
    public XRController controller;
    public GameObject handModel;

    void Update()
    {
        /*
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            // Adatta la scala o la visibilità del modello in base al valore di grip
            // per simulare l'apertura e la chiusura della mano
            handModel.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, gripValue);
        }

        // Aggiorna la posizione e la rotazione del modello della mano
        handModel.transform.position = controller.transform.position;
        handModel.transform.rotation = controller.transform.rotation;*/
    }
}