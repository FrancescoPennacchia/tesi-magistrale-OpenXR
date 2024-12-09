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
        // Controlla che questo oggetto abbia il tag "Bolt"
        if (CompareTag("Bolt") || CompareTag("bolt") || CompareTag("drill") || CompareTag("chiave"))
        {
            // Controlla se l'oggetto colpito ha il tag "Floor"
            if (collision.gameObject.CompareTag("Floor") && !hasLanded)
            {
                // Calcola la velocità relativa della collisione
                float impactVelocity = collision.relativeVelocity.magnitude;

                // Riproduce il suono se la velocità è sufficiente
                if (impactVelocity >= minVelocity)
                {
                    PlayFallSound(impactVelocity);
                    hasLanded = true;
                }
            }
        }
    }

    private void PlayFallSound(float impactVelocity)
    {
        float volume = Mathf.Clamp01(impactVelocity / 10f) * maxVolume;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
