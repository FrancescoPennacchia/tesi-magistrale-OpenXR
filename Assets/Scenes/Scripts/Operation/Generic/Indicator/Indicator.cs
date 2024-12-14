using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject currentArrow;
    public GameObject gameTarget;
    private Vector3 initialPosition;
    private float oscillationSpeed = 0.2f; // Velocità dell'oscillazione
    private float oscillationAmplitude = 0.01f; // Ampiezza dell'oscillazione
    private float verticalOffset = 0.2f; // Offset verticale

    void Start()
    {
        if (arrowPrefab != null && gameTarget != null)
        {
            Vector3 arrowPosition = gameTarget.transform.position + new Vector3(0, verticalOffset, 0);
            currentArrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.identity);
            currentArrow.transform.rotation = Quaternion.Euler(0, 90, 90);
            currentArrow.transform.localScale = new Vector3(5f, 5f, 5f);
            currentArrow.transform.SetParent(gameTarget.transform);
            initialPosition = currentArrow.transform.localPosition;
        }
        else
        {
            Debug.LogError("arrowPrefab o gameTarget non assegnato nell'Indicator.");
        }
    }

    void Update()
    {
        if (currentArrow != null)
        {
            float offset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
            currentArrow.transform.localPosition = initialPosition + new Vector3(offset, 0, 0);
        }
    }

    public void DestroyArrow()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
        Destroy(this); 
    }
}
