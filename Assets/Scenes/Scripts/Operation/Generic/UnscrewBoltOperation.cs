using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class UnscrewBoltOperation : BaseOperation
{
    public GameObject bolt;              // Il bullone da svitare
    private bool isUnscrewed = false;
    private XRGrabInteractable interactable;  // Interactable per l'oggetto chiave
    private float totalRotation = 0f;
    private float requiredRotation = 360f;    // Rotazione necessaria per svitare il bullone
    private Quaternion lastRotation;
    private bool isWrenchInCollider = false;  // Verifica se la chiave è nel collider del bullone

    public override void StartOperation()
    {
        // Non serve più specificare l'oggetto chiave manualmente
    }

    public override bool IsOperationComplete()
    {
        return isUnscrewed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Controlla se l'oggetto entrante ha il tag "chiave"
        if (other.CompareTag("chiave"))
        {
            isWrenchInCollider = true;
            interactable = other.GetComponent<XRGrabInteractable>(); // Ottieni il componente XRGrabInteractable dall'oggetto con il tag "chiave"

            if (interactable != null)
            {
                lastRotation = interactable.transform.rotation; // Memorizza la rotazione iniziale
            }

            Debug.Log("Chiave nel collider del bullone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Controlla se l'oggetto uscente ha il tag "chiave"
        if (other.CompareTag("chiave"))
        {
            isWrenchInCollider = false;
            interactable = null; // Resetta il riferimento all'oggetto interattivo
            Debug.Log("Chiave fuori dal collider del bullone.");
        }
    }

    private void Update()
    {
        // Controlla se l'oggetto interattivo è stato selezionato e se è nel collider
        if (interactable != null && interactable.isSelected && isWrenchInCollider)
        {
            Quaternion currentRotation = interactable.transform.rotation;
            float angle = Quaternion.Angle(lastRotation, currentRotation);

            // Calcola la direzione della rotazione della chiave
            Vector3 rotationAxis;
            float rotationAngle;
            (Quaternion.FromToRotation(lastRotation * Vector3.forward, currentRotation * Vector3.forward)).ToAngleAxis(out rotationAngle, out rotationAxis);

            // Considera solo la rotazione se avviene sull'asse giusto (asse Z ad esempio per svitare)
            if (Mathf.Abs(rotationAxis.z) > Mathf.Abs(rotationAxis.x) && Mathf.Abs(rotationAxis.z) > Mathf.Abs(rotationAxis.y))
            {
                totalRotation += rotationAngle;
                lastRotation = currentRotation; // Aggiorna la rotazione precedente

                // Trasferisci la rotazione al bullone
                bolt.transform.Rotate(Vector3.forward, rotationAngle); // Rotazione lungo l'asse Z del bullone

                // Controlla se la rotazione ha raggiunto la soglia richiesta
                if (totalRotation >= requiredRotation)
                {
                    isUnscrewed = true;
                    OnOperationComplete();
                }
            }
        }
    }


    private void OnOperationComplete()
    {
        // Disabilita l'interazione con la chiave
        if (interactable != null)
        {
            interactable.enabled = false;
        }

        XRGrabInteractable boltInteractable = bolt.GetComponent<XRGrabInteractable>();
        if (boltInteractable != null)
        {
            boltInteractable.enabled = true;
        }

        bolt.SetActive(false); // Potresti gestirlo con un'animazione invece di disattivare immediatamente l'oggetto
        Debug.Log("Bullone svitato con successo!");
    }
}