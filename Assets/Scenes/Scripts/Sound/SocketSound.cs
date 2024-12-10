using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketSound : MonoBehaviour
{
    public AudioClip enterClip;
    public AudioClip exitClip;
    public float volume = 1f;
    public float pitch = 1f;

    private AudioSource audioSource;
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor == null)
        {
            Debug.LogError("XRSocketInteractor component is missing on " + gameObject.name);
            return;
        }

        SetupAudioSource();

        // Warning per clip mancanti
        if (enterClip == null)
        {
            Debug.LogWarning("Enter Clip is not assigned for " + gameObject.name);
        }
        if (exitClip == null)
        {
            Debug.LogWarning("Exit Clip is not assigned for " + gameObject.name);
        }

        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    private void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (enterClip != null)
        {
            audioSource.PlayOneShot(enterClip);
        }
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        if (exitClip != null)
        {
            audioSource.PlayOneShot(exitClip);
        }
    }

    void OnDestroy()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }
    }
}
