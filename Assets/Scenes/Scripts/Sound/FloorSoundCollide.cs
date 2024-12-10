using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSoundCollide : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip fallSound; // Clip audio da riprodurre
    public float minVelocity = 1f; // Velocità minima per riprodurre il suono
    public float maxVolume = 1f; // Volume massimo del suono

    private AudioSource audioSource;
    private bool hasLanded = false; // Per evitare doppie attivazioni

    void Start()
    {
        // Aggiunge un AudioSource se non è già presente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = fallSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Floor") && !hasLanded)
        {
            Debug.Log("Collided with Floor");

            float impactVelocity = collision.relativeVelocity.magnitude;
            Debug.Log("Impact Velocity: " + impactVelocity);

            if (impactVelocity >= minVelocity)
            {
                Debug.Log("Playing sound...");
                PlayFallSound(impactVelocity);
                hasLanded = true;
            }
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            hasLanded = false;
        }
    }

    private void PlayFallSound(float impactVelocity)
    {
        float volume = Mathf.Clamp01(impactVelocity / 10f) * maxVolume;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
