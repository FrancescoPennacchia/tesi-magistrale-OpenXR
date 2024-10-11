using TMPro;
using UnityEngine;

public class OperationManager : MonoBehaviour
{
    [SerializeField] private OperationData operationData;
    [SerializeField] private TMP_Text canvasText;
    private InteractionManager interactionManager;
    private VisualIndicatorManager indicatorManager;
    private int currentIndex = 0;

    private void Start()
    {
        interactionManager = GetComponent<InteractionManager>();
        indicatorManager = GetComponent<VisualIndicatorManager>();

        interactionManager.OnObjectAttached += HandleObjectAttached;
        interactionManager.OnObjectDetached += HandleObjectDetached;

        UpdateInstruction();
        ShowNextIndicator();
    }

    private void HandleObjectAttached(GameObject obj)
    {
        // Logica per quando un oggetto viene agganciato
        currentIndex++;
        UpdateInstruction();
        ShowNextIndicator();
    }

    private void HandleObjectDetached(GameObject obj)
    {
        // Logica per quando un oggetto viene sganciato
    }

    private void UpdateInstruction()
    {
        if (currentIndex < operationData.instructions.Count)
        {
            canvasText.text = operationData.instructions[currentIndex].message;
        }
        else
        {
            canvasText.text = "Hai completato le istruzioni da eseguire!";
        }
    }

    private void ShowNextIndicator()
    {
        if (currentIndex < operationData.instructions.Count)
        {
            var nextObject = operationData.instructions[currentIndex].attachedGameObject;
            indicatorManager.ShowIndicator(nextObject);
        }
        else
        {
            indicatorManager.HideIndicator();
        }
    }
}
