using System.Collections;
using UnityEngine;

public class FlamableVial : MonoBehaviour, IInteractable, IItem
{
    private bool _isPicked = false;
    private GameObject player = null;
    public InteractType Type => InteractType.PickItem;
    public ItemType ItemType => ItemType.Flamable;

    public bool CanInteract()
    {
        return !_isPicked;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // Grab item if not picked
        if (!_isPicked)
        {
            player = interactor.gameObject;
            _isPicked = true;
        }
    }

    private void Update()
    {
        // Move the item on the top of the player body if picked
        if (_isPicked)
        {
            transform.position = player.transform.position + Vector3.up;
        }
    }

    public void Throw(Vector2 dir, float throwPower)
    {
        if (_isPicked)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(dir.x, 1.0f, dir.y) * throwPower, ForceMode.Impulse);
            _isPicked = false;
            player = null;
        }
    }

    public void RemoveInteractable()
    {
        Destroy(gameObject);
    }
}
