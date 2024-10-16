using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class ScrewBoltOperation : BaseOperation
{
    public GameObject bolt;              // Il bullone da avvitare
    private bool isScrewed = false;
    private float totalRotation = 0f;
    private float requiredRotation = 120;    // Rotazione necessaria per avvitare il bullone
    private bool isWrenchInCollider = false;  // Verifica se la chiave è nel collider del bullone
    private float liftAmount = 0.1f;          // Quantità di abbassamento per ogni passo di rotazione
    private float rotationSpeed = 50f;        // Velocità di rotazione in gradi per secondo
    private float liftSpeed = 0.01f;          // Velocità di abbassamento per secondo

    private bool shouldRotateAndLift = false; // Flag per controllare la rotazione e l'abbassamento

    public override void StartOperation()
    {
        if (bolt == null)
        {
            Debug.LogError("Bolt non assegnato in ScrewBoltOperation.");
            return;
        }

        // Aggiungi dinamicamente il BoltTriggerHandler al bullone
        BoltTriggerHandler triggerHandler = bolt.AddComponent<BoltTriggerHandler>();
        triggerHandler.screwBoltOperation = this;

        // Assicura che il bullone abbia un Collider con Is Trigger attivato
        Collider boltCollider = bolt.GetComponent<Collider>();
        if (boltCollider == null)
        {
            Debug.LogError("Il bullone non ha un Collider.");
            return;
        }

        if (!boltCollider.isTrigger)
        {
            boltCollider.isTrigger = true;
            Debug.Log("Impostato IsTrigger su true per il Collider del bullone.");
        }
    }

    public override bool IsOperationComplete()
    {
        return isScrewed;
    }

    public void HandleTriggerEnter(Collider other)
    {
        Debug.Log("Oggetto entrato nel trigger del bullone: " + other.gameObject.name);
        // Controlla se l'oggetto entrante ha il tag "chiave"
        if (other.CompareTag("chiave"))
        {
            isWrenchInCollider = true;
            shouldRotateAndLift = true;
            Debug.Log("Chiave nel collider del bullone.");
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        // Controlla se l'oggetto uscente ha il tag "chiave"
        if (other.CompareTag("chiave"))
        {
            isWrenchInCollider = false;
            shouldRotateAndLift = false;
            Debug.Log("Chiave fuori dal collider del bullone.");
        }
    }

    private void Update()
    {
        if (shouldRotateAndLift && totalRotation < requiredRotation)
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // Calcola la rotazione per frame
            float liftStep = liftSpeed * Time.deltaTime;         // Calcola l'abbassamento per frame

            bolt.transform.Rotate(Vector3.left, rotationStep, Space.Self); // Rotazione lungo l'asse X locale del bullone
            bolt.transform.Translate(Vector3.down * liftStep, Space.World); // Abbassa il bullone lungo l'asse Y globale
            totalRotation += rotationStep;

            // Controlla se la rotazione ha raggiunto la soglia richiesta
            if (totalRotation >= requiredRotation)
            {
                isScrewed = true;
                OnOperationComplete();
            }
        }
    }

    private void OnOperationComplete()
    {
        // Abilita il bullone
        bolt.SetActive(true);
        Debug.Log("Bullone avvitato con successo!");
    }
}