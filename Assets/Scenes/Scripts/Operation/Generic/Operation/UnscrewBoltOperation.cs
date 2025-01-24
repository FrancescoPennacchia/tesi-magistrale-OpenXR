using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnscrewBoltOperation : BaseOperation
{
    public GameObject bolt;            // The bolt to unscrew
    public Asse rotationBolt;          // Axis of rotation
    public Asse directionBolt;         // Direction of lifting
    private bool isUnscrewed = false;
    private float totalRotation = 0f;
    private float requiredRotation = 300f;    // Rotation needed to unscrew
    private float rotationSpeed = 200f;       // Rotation speed in degrees per second
    private float liftSpeed = 0.01f;          // Lifting speed per second

    private bool shouldRotateAndLift = false; // Flag to control rotation and lifting

    private Vector3 rotationAxis;
    private Vector3 liftDirection;

    // Store initial position and rotation
    private Vector3 initialBoltPosition;
    private Quaternion initialBoltRotation;

    //Input Drill
    private XRInputActions inputActions;
    float rightTriggerValue;
    float leftTriggerValue;

    bool isDrill = false;

    public override void StartOperation()
    {

        inputActions = new XRInputActions();
        inputActions.XRController.Enable();

        if (bolt == null)
        {
            Debug.LogError("Bolt not assigned in UnscrewBoltOperation.");
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
        triggerHandler.unscrewBoltOperation = this;
        triggerHandler.screwBoltOperation = null;

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
        return isUnscrewed;
    }

    public void HandleTriggerEnter(Collider other)
    {
        Debug.Log("Object entered bolt trigger: " + other.gameObject.name);
        // Check if the entering object has the tag "chiave"
        if (other.CompareTag("chiave"))
        {
            isDrill = false;
            shouldRotateAndLift = true;
            Debug.Log("Key entered bolt collider.");
        } else if (other.CompareTag("drill"))
        {
            isDrill = true;
            shouldRotateAndLift = true;
            Debug.Log("Key entered drill collider.");     
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        // Check if the exiting object has the tag "chiave"
        if (other.CompareTag("chiave"))
        {
            isDrill = false;
            shouldRotateAndLift = false;
            Debug.Log("Key exited bolt collider.");
        } else if (other.CompareTag("drill"))
        {
            isDrill = false;
            shouldRotateAndLift = false;
            Debug.Log("Key entered drill collider.");      
        }
    }

    private void Update()
    {
        rightTriggerValue = inputActions.XRController.RightTrigger.ReadValue<float>();
        leftTriggerValue = inputActions.XRController.LeftTrigger.ReadValue<float>();

        bool isTriggerPressed = rightTriggerValue > 0.5f || leftTriggerValue > 0.5f;

        if (isDrill && isTriggerPressed && shouldRotateAndLift && totalRotation < requiredRotation)
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // Rotation per frame
            float liftStep = liftSpeed * Time.deltaTime;         // Lifting per frame

            bolt.transform.Rotate(rotationAxis, rotationStep, Space.Self); // Rotate bolt
            bolt.transform.Translate(liftDirection * liftStep, Space.Self); // Lift bolt
            totalRotation += rotationStep;

            // Check if required rotation is achieved
            if (totalRotation >= requiredRotation)
            {
                isUnscrewed = true;
                OnOperationComplete();
            }
        } else if (!isDrill && shouldRotateAndLift && totalRotation < requiredRotation)
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // Rotation per frame
            float liftStep = liftSpeed * Time.deltaTime;         // Lifting per frame

            bolt.transform.Rotate(rotationAxis, rotationStep, Space.Self); // Rotate bolt
            bolt.transform.Translate(liftDirection * liftStep, Space.Self); // Lift bolt
            totalRotation += rotationStep;

            // Check if required rotation is achieved
            if (totalRotation >= requiredRotation)
            {
                isUnscrewed = true;
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
            triggerHandler.unscrewBoltOperation = null;
            triggerHandler.screwBoltOperation = null;
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
        if (grabInteractable == null)
        {
            grabInteractable = bolt.AddComponent<XRGrabInteractable>();
        }
        grabInteractable.enabled = true;

        inputActions.XRController.Disable();

        Debug.Log("Bolt unscrewed successfully!");
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
