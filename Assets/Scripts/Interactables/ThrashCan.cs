using System.Collections;
using UnityEngine;

public class ThrashCan : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject burningModel;
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

    public void Throw(Vector3 dir, float throwPower)
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
        // Switch model
        normalModel.SetActive(false);
        burningModel.SetActive(true);

        // 5 second of burn
        yield return new WaitForSeconds(5.0f);

        // Slowly turn transparent and then be destroyed
        float elapsedTime = 0.0f;
        Color baseColor = burningModel.GetComponent<MeshRenderer>().material.color;
        Color finalColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);
        while (elapsedTime < 2.0f)
        {
            burningModel.GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, finalColor, (elapsedTime / 2.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
