using UnityEngine;

public class VisualIndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    private GameObject currentIndicator;

    public void ShowIndicator(GameObject targetObject)
    {
        if (arrowPrefab == null || targetObject == null)
            return;

        // Instanzia l'indicatore e lo attacca all'oggetto target
        currentIndicator = Instantiate(arrowPrefab, targetObject.transform);
        currentIndicator.transform.localPosition = Vector3.up * 0.5f;
    }

    public void HideIndicator()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
            currentIndicator = null;
        }
    }
}