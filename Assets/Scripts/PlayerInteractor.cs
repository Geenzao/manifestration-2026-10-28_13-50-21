using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private float _interactDistance = 1.5f;
    [SerializeField] private float _throwPower = 15.0f;
    private IInteractable _heldItem;

    // On interact key pressed, do stuff based on the type of the interactable
    public void Interact()
    {
        // Interact if something in range
        if (ClosestInteractable(out IInteractable interactable))
        {
            switch (interactable.Type)
            {
                case InteractType.Interact:
                    interactable.Interact(this);
                    break;
                case InteractType.PickItem:
                    if ((_heldItem == null) && (interactable is IItem))
                    {
                        interactable.Interact(this);
                        _heldItem = interactable;
                    }
                    else
                    {
                        ThrowItem();
                    }
                    break;
            }
        }
        // If no interaction and got something to put down
        else if (_heldItem != null) ThrowItem();
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

    // Return the held item
    public IItem GetPickedItem()
    {
        return _heldItem as IItem;
    }

    // Use the held item (destroy it)
    public void UsePickedItem()
    {
        _heldItem.RemoveInteractable();
        _heldItem = null;
    }

    // Throw the held item
    private void ThrowItem()
    {
        _heldItem.Throw(transform.forward, _throwPower);
        _heldItem = null;
    }
}
