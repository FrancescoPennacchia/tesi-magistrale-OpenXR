using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnscrewBoltOperation : BaseOperation
{
    public GameObject bolt;
    private bool isUnscrewed = false;
    private XRGrabInteractable interactable;
    private float totalRotation = 0f;
    private float requiredRotation = 360f; // Rotazione necessaria per svitare il bullone
    private Quaternion lastRotation;

    public override void StartOperation()
    {
        if (bolt != null)
        {
            interactable = bolt.GetComponent<XRGrabInteractable>();
            if (interactable != null)
            {
                interactable.enabled = true;
                interactable.selectEntered.AddListener(OnBoltGrabbed);
                interactable.selectExited.AddListener(OnBoltReleased);
            }
        }
    }

    public override bool IsOperationComplete()
    {
        return isUnscrewed;
    }

    private void OnBoltGrabbed(SelectEnterEventArgs args)
    {
        lastRotation = bolt.transform.rotation;
    }

    private void OnBoltReleased(SelectExitEventArgs args)
    {
        // Puoi implementare la logica necessaria quando il bullone viene rilasciato
    }

    private void Update()
    {
        if (interactable != null && interactable.isSelected)
        {
            Quaternion currentRotation = bolt.transform.rotation;
            float angle = Quaternion.Angle(lastRotation, currentRotation);

            // Calcola la direzione della rotazione se necessario
            totalRotation += angle;
            lastRotation = currentRotation;

            if (totalRotation >= requiredRotation)
            {
                isUnscrewed = true;
                OnOperationComplete();
            }
        }
    }

    private void OnOperationComplete()
    {
        // Disabilita l'interazione
        if (interactable != null)
        {
            interactable.enabled = false;
            interactable.selectEntered.RemoveListener(OnBoltGrabbed);
            interactable.selectExited.RemoveListener(OnBoltReleased);
        }

        // Rimuovi il bullone o esegui un'animazione
        // Ad esempio, puoi disattivare il bullone
        bolt.SetActive(false);

        Debug.Log("Bullone svitato con successo!");
    }
}