using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] private float oscillationSpeed = 2f;
    [SerializeField] private float oscillationAmplitude = 1f;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
        transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
    }
}