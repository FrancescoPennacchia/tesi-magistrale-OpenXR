using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
/*
[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{
    private Animator handAnimator = null;
    private XRHandSubsystem handSubsystem;
    private XRHand leftHand;
    private XRHand rightHand;

    private void Start()
    {
        handAnimator = GetComponent<Animator>();
        handSubsystem = GetHandSubsystem();

        if (handSubsystem == null)
        {
            Debug.LogError("Hand Subsystem not found. Make sure hand tracking is enabled in your project settings.");
        }
    }

    private XRHandSubsystem GetHandSubsystem()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        return subsystems.Count > 0 ? subsystems[0] : null;
    }

    private void Update()
    {
        if (handSubsystem == null || !handSubsystem.running)
            return;

        leftHand = handSubsystem.leftHand;
        rightHand = handSubsystem.rightHand;

        if (rightHand.isTracked)
        {
            UpdateHandAnimation(rightHand);
        }

        if (leftHand.isTracked)
        {
            UpdateHandAnimation(leftHand);
        }
    }

    private void UpdateHandAnimation(XRHand hand)
    {
        if (!hand.isTracked)
            return;

        // Update for each finger
        UpdateFingerCurl(FingerType.Thumb, hand, XRHandJointID.ThumbTip);
        UpdateFingerCurl(FingerType.Index, hand, XRHandJointID.IndexTip);
        UpdateFingerCurl(FingerType.Middle, hand, XRHandJointID.MiddleTip);
        UpdateFingerCurl(FingerType.Ring, hand, XRHandJointID.RingTip);
        UpdateFingerCurl(FingerType.Pinky, hand, XRHandJointID.LittleTip);
    }

    private void UpdateFingerCurl(FingerType fingerType, XRHand hand, XRHandJointID jointID)
    {
        XRHandJoint rootJoint = hand.GetJoint(XRHandJointID.Wrist);
        XRHandJoint tipJoint = hand.GetJoint(jointID);

        if (!rootJoint.Equals(default(XRHandJoint)) && !tipJoint.Equals(default(XRHandJoint)))
        {
            Pose rootPose, tipPose;
            if (rootJoint.TryGetPose(out rootPose) && tipJoint.TryGetPose(out tipPose))
            {
                float curlValue = CalculateFingerCurl(rootPose, tipPose);
                handAnimator.SetFloat(fingerType.ToString(), curlValue);
            }
        }
    }

    private float CalculateFingerCurl(Pose rootPose, Pose tipPose)
    {
        Vector3 direction = tipPose.position - rootPose.position;
        float curl = Vector3.Angle(rootPose.forward, direction); // Migliora la direzione del calcolo
        float normalizedCurl = Mathf.InverseLerp(0, 90, curl);
        return normalizedCurl;
    }
}*/



[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{
    [SerializeField] private InputActionReference stickInput;  // Riferimento per il thumbstick
    [SerializeField] private InputActionReference triggerInput; // Riferimento per il trigger
    [SerializeField] private InputActionReference gripInput;    // Riferimento per il grip

    [SerializeField] private XRBaseInteractor pokeInteractor;

    private bool isUIAnimationPlaying = false;
    private Animator handAnimator = null;

    private readonly List<Finger> grippingFingers = new List<Finger>()
    {
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };

    private readonly List<Finger> pointingFingers = new List<Finger>()
    {
        new Finger(FingerType.Index)
    };

    private readonly List<Finger> thumbFinger = new List<Finger>()
    {
        new Finger(FingerType.Thumb)
    };

    private readonly List<Finger> uiFingers = new List<Finger>()
    {
        new Finger(FingerType.Thumb),
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };

    private void OnEnable()
    {
        pokeInteractor.hoverEntered.AddListener(ActivateUIHandPose);
        pokeInteractor.hoverExited.AddListener(DeactivateUIHandPose);

        // Abilita le azioni di input
        stickInput.action.Enable();
        triggerInput.action.Enable();
        gripInput.action.Enable();
    }

    private void OnDisable()
    {
        pokeInteractor.hoverEntered.RemoveListener(ActivateUIHandPose);
        pokeInteractor.hoverExited.RemoveListener(DeactivateUIHandPose);

        // Disabilita le azioni di input
        stickInput.action.Disable();
        triggerInput.action.Disable();
        gripInput.action.Disable();
    }

    private void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    private void ActivateUIHandPose(HoverEnterEventArgs arg0)
    {
        isUIAnimationPlaying = true;
        SetFingerAnimationValues(uiFingers, 1);
        AnimateActionInput(uiFingers);
    }

    private void DeactivateUIHandPose(HoverExitEventArgs arg0)
    {
        isUIAnimationPlaying = false;
        SetFingerAnimationValues(uiFingers, 0);
        AnimateActionInput(uiFingers);
    }

    private void Update()
    {
        if (isUIAnimationPlaying)
            return;

        var stickVal = stickInput.action.ReadValue<Vector2>();
        SetFingerAnimationValues(thumbFinger, stickVal.y);
        AnimateActionInput(thumbFinger);

        var triggerVal = triggerInput.action.ReadValue<float>();
        SetFingerAnimationValues(pointingFingers, triggerVal);
        AnimateActionInput(pointingFingers);

        var gripVal = gripInput.action.ReadValue<float>();
        SetFingerAnimationValues(grippingFingers, gripVal);
        AnimateActionInput(grippingFingers);
    }

    public void SetFingerAnimationValues(List<Finger> fingersToAnimate, float targetValue)
    {
        foreach (Finger finger in fingersToAnimate)
        {
            finger.target = targetValue;
        }
    }

    public void AnimateActionInput(List<Finger> fingersToAnimate)
    {
        foreach (Finger finger in fingersToAnimate)
        {
            var fingerName = finger.type.ToString();
            var animationBlendValue = finger.target;
            handAnimator.SetFloat(fingerName, animationBlendValue);
        }
    }
}
