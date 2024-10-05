using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class screwDriving : MonoBehaviour
{
    private int timeScrew = 0;
    private Rigidbody rb;
    private GameObject screw;
    private bool onscrew = false;
    private bool trigger = false;
    [SerializeField]
    public XRGrabInteractable Drill;
    public AudioSource audio;
    private int screwCount = 0;
    private bool riddleFinished = false;

    private XRInputActions inputActions;

    private void OnEnable()
    {
        inputActions = new XRInputActions();
        inputActions.XRController.Enable();
    }

    private void OnDisable()
    {
        inputActions.XRController.Disable();
    }

    void Update()
    {
        // If grabbed, looks if the index trigger is pressed
        if (Drill.isSelected)
        {
            float rightTriggerValue = inputActions.XRController.RightTrigger.ReadValue<float>();
            float leftTriggerValue = inputActions.XRController.LeftTrigger.ReadValue<float>();
            //Debug.Log("Selezionato Drill");

            if (rightTriggerValue > 0.5f || leftTriggerValue > 0.5f)
            {//Debug.Log("Trigger Premuto");

                trigger = true;
            }
            else
            {
                trigger = false;
            }
        }
        else
        {
            trigger = false;
        }

        // If the index trigger is pressed, starts the drill and play sound and vibration
        if (trigger)
        {
            transform.Rotate(0, 0, 15, Space.Self);
            if (audio.time > 3f)
            {
                audio.Stop();
            }
            if (!audio.isPlaying)
            {
                audio.time = 1f;
                audio.Play();
            }
            // If on the screw, rotate the screw
            if (onscrew)
            {
                rb.transform.position = rb.transform.position + new Vector3(0, 0.002f, 0);
                rb.transform.Rotate(0, 0, -15, Space.Self);
                timeScrew += 1;
            }
        }
        else
        {
            audio.Stop();
        }

        // After the screw has been descrewed enough, it removes it with addforce and make it kinematic
        if (timeScrew >= 120)
        {
            if (onscrew)
            {
                rb.isKinematic = false;
                rb.AddForce(1, 1, 0);
                onscrew = false;
                if (screwCount < 2)
                {
                    screwCount += 1;
                }
                else if (!riddleFinished)
                {
                    riddleFinished = true;
                    //GameManager.Instance.UpdateGameState(RiddlesProgress.ScrewsRemoved);
                }
                timeScrew = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "screw")
        {
            screw = other.gameObject;
            rb = other.GetComponent<Rigidbody>();
            onscrew = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == screw)
        {
            rb = null;
            onscrew = false;
            screw = null; // Reset the screw variable
        }
    }
}
