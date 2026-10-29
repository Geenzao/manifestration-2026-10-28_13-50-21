public enum InteractType
{
    Failed,
    Interact,
    PickItem,
}

public interface IInteractable
{
    // Can it be interacted with
    public bool CanInteract();
    // Do the interaction
    public bool Interact(PlayerInteractor interactor, IInteractable playerItem, out InteractType type);
}
