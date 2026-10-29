using UnityEngine;

public enum InteractType
{
    Interact,
    PickItem,
}

public interface IInteractable
{
    // Type of the interactable
    public InteractType Type { get; }
    // Can it be interacted with
    public bool CanInteract();
    // Do the interaction
    public void Interact(PlayerInteractor interactor);
    // Throw an interactable
    public void Throw(Vector2 dir, float throwPower);
    public void RemoveInteractable();
}
