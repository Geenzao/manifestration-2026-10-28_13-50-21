using UnityEngine;

public class Ball : MonoBehaviour, IInteractable
{
    private bool _isPicked = false;
    private GameObject player = null;
    
    public bool CanInteract()
    {
        return !_isPicked;
    }

    public bool Interact(PlayerInteractor interactor, IInteractable playerItem, out InteractType type)
    {
        type = InteractType.Failed;

        // If picked item then drop it
        if (_isPicked)
        {
            transform.position = interactor.transform.position;
            player = null;
            type = InteractType.PickItem;
        }
        // Else grab the item if the player has nothing in hand
        else if (playerItem == null)
        {
            player = interactor.gameObject;
            type = InteractType.PickItem;
        }

        // If couldn't interact properly with it, then failed
        if (type == InteractType.Failed) return false;
        _isPicked = !_isPicked;
        return true;
    }

    private void Update()
    {
        // Move the item on the top of the player body if picked
        if (_isPicked)
        {
            transform.position = player.transform.position + Vector3.up;
        }
    }
}
