using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class TagSelectFilter : XRSocketInteractor
{
    [Tooltip("Il tag degli oggetti che questo filtro permetterà.")]
    public string allowedTag;

    // Verifica se l'interactible può essere hoverato (usando il tag specificato)
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && MatchUsingTag(interactable);
    }

    // Verifica se l'interactible può essere selezionato (usando il tag specificato)
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && MatchUsingTag(interactable);
    }

    // Controlla se l'oggetto interagibile ha il tag specificato
    private bool MatchUsingTag(object interactable)
    {
        // Ottieni il GameObject associato e verifica il tag
        if (interactable is IXRInteractable interactableObj)
        {
            return interactableObj.transform.CompareTag(allowedTag);
        }
        return false;
    }

    /*
    public bool CanSelect(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        // Controlla se l'oggetto interagibile ha il tag specificato
        return interactable.transform.CompareTag(allowedTag);
    }

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        bool result = interactable.transform.CompareTag(allowedTag);
        Debug.Log($"TagSelectFilter: Interagibile '{interactable.transform.name}' con tag '{interactable.transform.tag}' - Risultato: {result}");
        return CanSelect(interactor, interactable);
    }

    public bool canProcess => true;*/
}
