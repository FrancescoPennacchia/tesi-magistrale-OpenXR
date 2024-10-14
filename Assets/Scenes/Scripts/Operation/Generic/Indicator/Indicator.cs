using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject currentArrow;
    public GameObject gameTarget;
    public Vector3 initialPosition;
    private float oscillationSpeed = 2f; // Velocità dell'oscillazione
    private float oscillationAmplitude = 1f; // Ampiezza dell'oscillazione (in unità di posizione)

    // Start is called before the first frame update
    void Start()
    {
        if (currentArrow != null)
        {
            currentArrow.transform.localPosition = initialPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentArrow != null)
        {
            // Calcola lo spostamento laterale utilizzando una funzione sinusoidale
            float offset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
            // Aggiorna la posizione locale della freccia
            currentArrow.transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
        }
    }
}
