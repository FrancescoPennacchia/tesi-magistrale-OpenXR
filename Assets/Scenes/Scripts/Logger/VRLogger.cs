using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VRLogger : MonoBehaviour
{
    public TMP_Text logText;  // Assegna il Text UI dall'Editor (all'interno della Scroll View)
    public ScrollRect scrollRect;  // Assegna il componente ScrollRect della Scroll View
    private string logMessages = "";  // Stringa per memorizzare i log

    void OnEnable()
    {
        // Sottoscrivi l'evento di log per catturare i messaggi
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Annulla la sottoscrizione all'evento di log quando l'oggetto è disabilitato
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Aggiungi il tipo di log (info, warning, error)
        string newLogMessage = "";

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                newLogMessage = $"<color=red>[ERROR]</color> {logString}\n<color=red>{stackTrace}</color>\n";
                break;
            case LogType.Warning:
                newLogMessage = $"<color=yellow>[WARNING]</color> {logString}\n";
                break;
            case LogType.Log:
                newLogMessage = $"<color=white>[LOG]</color> {logString}\n";
                break;
        }

        // Aggiorna il log
        logMessages += newLogMessage;

        // Limita la lunghezza del log totale per evitare sovraccarico
        if (logMessages.Length > 10000)
        {
            logMessages = logMessages.Substring(logMessages.Length - 10000);
        }

        // Aggiorna il testo nel Text UI
        logText.text = logMessages;

        // Scroll automatico alla fine
        Canvas.ForceUpdateCanvases();  // Forza l'aggiornamento del Canvas prima di scorrere
        scrollRect.verticalNormalizedPosition = 0f;  // Imposta la scrollbar in fondo per visualizzare il nuovo log
    }
}

