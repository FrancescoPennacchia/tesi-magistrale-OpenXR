using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScrewBoltOperation : BaseOperation
{
    public GameObject bolt;            // The bolt to screw
    private bool isScrewed = false;    // Operation state
    public Asse rotationBolt;          // Axis of rotation
    public Asse directionBolt;         // Direction of lowering
    private float totalRotation = 0f;  // Accumulated rotation
    private float requiredRotation = 300f; // Rotation needed to screw
    private float rotationSpeed = 200f;    // Rotation speed in degrees per second
    private float liftSpeed = 0.01f;       // Lowering speed per second

    private bool shouldRotateAndLift = false; // Flag to enable rotation and lowering

    private Vector3 rotationAxis;
    private Vector3 liftDirection;

    // Store initial position and rotation
    private Vector3 initialBoltPosition;
    private Quaternion initialBoltRotation;

    public override void StartOperation()
    {
        if (bolt == null)
        {
            Debug.LogError("Bolt not assigned in ScrewBoltOperation.");
            return;
        }

        // Store the initial position and rotation
        initialBoltPosition = bolt.transform.position;
        initialBoltRotation = bolt.transform.rotation;

        // Reset bolt's position and rotation
        bolt.transform.position = initialBoltPosition;
        bolt.transform.rotation = initialBoltRotation;

        // Set up BoltTriggerHandler
        BoltTriggerHandler triggerHandler = bolt.GetComponent<BoltTriggerHandler>();
        if (triggerHandler == null)
        {
            triggerHandler = bolt.AddComponent<BoltTriggerHandler>();
        }
        triggerHandler.screwBoltOperation = this;
        triggerHandler.unscrewBoltOperation = null;

        // Reset Collider settings
        Collider boltCollider = bolt.GetComponent<Collider>();
        if (boltCollider != null)
        {
            boltCollider.enabled = true;
            boltCollider.isTrigger = true;
        }

        // Reset Rigidbody settings
        Rigidbody boltRigidbody = bolt.GetComponent<Rigidbody>();
        if (boltRigidbody != null)
        {
            boltRigidbody.isKinematic = true;
            boltRigidbody.useGravity = false;
            boltRigidbody.constraints = RigidbodyConstraints.None;
        }

        // Disable XR Grab Interactable
        XRGrabInteractable grabInteractable = bolt.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // Configure rotation and lift axes
        rotationAxis = GetAxis(rotationBolt);
        liftDirection = GetAxis(directionBolt);

        // Reset rotation tracker
        totalRotation = 0f;
    }

    public override bool IsOperationComplete()
    {
        return isScrewed;
    }

    public void HandleTriggerEnter(Collider other)
    {
        Debug.Log("Object entered bolt trigger: " + other.gameObject.name);

        // Check if the entering object is the key
        if (other.CompareTag("chiave"))
        {
            shouldRotateAndLift = true;
            Debug.Log("Key entered bolt collider.");
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        // Check if the exiting object is the key
        if (other.CompareTag("chiave"))
        {
            shouldRotateAndLift = false;
            Debug.Log("Key exited bolt collider.");
        }
    }

    private void Update()
    {
        if (shouldRotateAndLift && totalRotation < requiredRotation)
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // Rotation per frame
            float liftStep = liftSpeed * Time.deltaTime;         // Lowering per frame

            // Rotate and lower the bolt
            bolt.transform.Rotate(rotationAxis, -rotationStep, Space.Self); // Inverse rotation
            bolt.transform.Translate(-liftDirection * liftStep, Space.Self); // Inverse lifting
            totalRotation += rotationStep;

            // Check if required rotation is achieved
            if (totalRotation >= requiredRotation)
            {
                isScrewed = true;
                OnOperationComplete();
            }
        }
    }

    private void OnOperationComplete()
    {
        // Reset BoltTriggerHandler references
        BoltTriggerHandler triggerHandler = bolt.GetComponent<BoltTriggerHandler>();
        if (triggerHandler != null)
        {
            triggerHandler.screwBoltOperation = null;
            triggerHandler.unscrewBoltOperation = null;
        }

        // Enable Rigidbody physics
        Rigidbody boltRigidbody = bolt.GetComponent<Rigidbody>();
        if (boltRigidbody != null)
        {
            boltRigidbody.isKinematic = false;
            boltRigidbody.useGravity = true;
            // Optionally constrain movement if needed
            // boltRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Disable trigger on collider
        Collider boltCollider = bolt.GetComponent<Collider>();
        if (boltCollider != null)
        {
            boltCollider.isTrigger = false;
        }

        // Enable XR Grab Interactable
        XRGrabInteractable grabInteractable = bolt.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }

        Debug.Log("Bolt screwed successfully!");
    }

    // Helper method to get the axis vector based on Asse enum
    private Vector3 GetAxis(Asse axis)
    {
        switch (axis)
        {
            case Asse.XRight: return Vector3.right;
            case Asse.XLeft: return Vector3.left;
            case Asse.YUp: return Vector3.up;
            case Asse.YDown: return Vector3.down;
            case Asse.ZForward: return Vector3.forward;
            case Asse.ZBack: return Vector3.back;
            default: return Vector3.up;
        }
    }
}
