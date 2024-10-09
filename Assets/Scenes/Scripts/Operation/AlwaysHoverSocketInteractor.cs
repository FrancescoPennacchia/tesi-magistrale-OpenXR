using UnityEngine.XR.Interaction.Toolkit;

//Script per cambiare l'interazione di un socket in modo che possa sempre essere selezionato
public class AlwaysHoverSocketInteractor : XRSocketInteractor
{
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        // Consenti l'hover se l'interagibile è afferrato
        var selectInteractable = interactable as IXRSelectInteractable;
        if (selectInteractable != null && selectInteractable.isSelected)
        {
            return true;
        }
        // Mantieni il comportamento di default per la prossimità
        return base.CanHover(interactable);
    }
}
