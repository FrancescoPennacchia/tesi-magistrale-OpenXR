using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketSound : MonoBehaviour
{
    public AudioClip enterClip;
    public AudioClip exitClip;
    private AudioSource audioSource;
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
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
        socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        socketInteractor.selectExited.RemoveListener(OnSelectExited);
    }
}
