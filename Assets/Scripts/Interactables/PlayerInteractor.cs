using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private float _interactDistance = 1.0f;
    private IInteractable _pickedItem;

    // On interact key pressed, do stuff based on the type of the interactable
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            // Interact if something in range
            if (ClosestInteractable(out IInteractable interactable))
            {
                bool hasInteracted = interactable.Interact(this, _pickedItem, out InteractType type);
                Debug.Log(type);
                switch (type)
                {
                    case InteractType.Failed:
                        break;
                    case InteractType.Interact:
                        break;
                    case InteractType.PickItem:
                        _pickedItem = interactable;
                        break;
                }
                // If no succesfull interaction and got something to put down
                if (!hasInteracted && _pickedItem != null)
                {
                    _pickedItem.Interact(this, _pickedItem, out InteractType _);
                    _pickedItem = null;
                }
            }
            // If no interaction and got something to put down
            else if (_pickedItem != null)
            {
                _pickedItem.Interact(this, _pickedItem, out InteractType _);
                _pickedItem = null;
            }
        }
    }

    // Check for interactables around the player, and get the closest one
    private bool ClosestInteractable(out IInteractable interactable)
    {
        float distance = Mathf.Infinity;
        interactable = null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _interactDistance, _interactableMask);

        foreach (Collider hit in hitColliders)
        {
            if (hit.TryGetComponent(out IInteractable component))
            {
                // If component found check
                if (component.CanInteract() && distance > Vector3.Distance(transform.position, hit.transform.position))
                {
                    interactable = component;
                    distance = Vector3.Distance(transform.position, hit.transform.position);
                }
            }
        }
        return interactable != null;
    }
}
