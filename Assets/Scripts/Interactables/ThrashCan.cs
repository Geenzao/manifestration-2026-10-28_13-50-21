using System.Collections;
using UnityEngine;

public class ThrashCan : MonoBehaviour, IInteractable
{
    public InteractType Type => InteractType.Interact;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // If have a flamable object, destroy the thrash can
        if (interactor.GetPickedItem().ItemType == ItemType.Flamable)
        {
            interactor.UsePickedItem();
            RemoveInteractable();
        }
    }

    public void Throw(Vector2 dir, float throwPower)
    {
        return;
    }

    public void RemoveInteractable()
    {
        StartCoroutine(BurnTrashcan());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IItem>(out IItem item))
        {
            if (item.ItemType == ItemType.Flamable)
            {
                (item as IInteractable).RemoveInteractable();
                RemoveInteractable();
            }
        }
    }

    public IEnumerator BurnTrashcan()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
