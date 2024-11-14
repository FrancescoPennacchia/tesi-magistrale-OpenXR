using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class TagSocketFilter : XRSocketInteractor
{
    public string[] tagsToAccept; // Lista dei tag accettati

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        // Verifica le condizioni base
        if (!base.CanHover(interactable))
            return false;

        // Ottieni il GameObject dall'interfaccia IXRHoverInteractable
        GameObject interactableObject = null;

        // Metodo 1: Usando transform (se disponibile)
        if (interactable.transform != null)
        {
            interactableObject = interactable.transform.gameObject;
        }
        else
        {
            // Metodo 2: Cast a MonoBehaviour
            interactableObject = (interactable as MonoBehaviour)?.gameObject;
        }

        // Verifica che il GameObject sia valido
        if (interactableObject == null)
            return false;

        // Controlla se l'interactable ha uno dei tag accettati
        foreach (string tag in tagsToAccept)
        {
            if (interactableObject.CompareTag(tag))
                return true;
        }

        // Se nessun tag corrisponde, ritorna false
        return false;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // Verifica le condizioni base
        if (!base.CanSelect(interactable))
            return false;

        // Ottieni il GameObject dall'interfaccia IXRSelectInteractable
        GameObject interactableObject = null;

        // Metodo 1: Usando transform (se disponibile)
        if (interactable.transform != null)
        {
            interactableObject = interactable.transform.gameObject;
        }
        else
        {
            // Metodo 2: Cast a MonoBehaviour
            interactableObject = (interactable as MonoBehaviour)?.gameObject;
        }

        // Verifica che il GameObject sia valido
        if (interactableObject == null)
            return false;

        // Controlla se l'interactable ha uno dei tag accettati
        foreach (string tag in tagsToAccept)
        {
            if (interactableObject.CompareTag(tag))
                return true;
        }

        // Se nessun tag corrisponde, ritorna false
        return false;
    }
}
