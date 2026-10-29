using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable, IItem
{
    [SerializeField] private ItemType type = ItemType.Normal;
    private bool _isPicked = false;
    private GameObject player = null;
    public InteractType Type => InteractType.PickItem;
    public ItemType ItemType => type;

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
            Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            _isPicked = true;
        }
    }

    private void Update()
    {
        // Move the item on the top of the player body if picked
        if (_isPicked)
        {
            transform.position = player.transform.position + Vector3.up * 1.0f;
        }
    }

    public void Throw(Vector3 dir, float throwPower)
    {
        if (_isPicked)
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(new Vector3(dir.x, 0.5f, dir.z) * throwPower, ForceMode.Impulse);
            StartCoroutine(RestoreCollision(player.GetComponent<Collider>()));
            _isPicked = false;
            player = null;
        }
    }

    // Quick slightly dirty way to wait a moment before restoring collsion to prevent player collision clipping bug
    private IEnumerator RestoreCollision(Collider col)
    {
        yield return new WaitForSeconds(0.1f);
        Physics.IgnoreCollision(GetComponent<Collider>(), col, false);
    }

    public void RemoveInteractable()
    {
        Destroy(gameObject);
    }
}