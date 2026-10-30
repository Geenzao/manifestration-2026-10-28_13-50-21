using System.Collections;
using UnityEngine;

public class Bag : MonoBehaviour, IInteractable
{
    [SerializeField] private Objectif objectifSO;
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject particle;
    [SerializeField] private AudioClip clip;
    public InteractType Type => InteractType.Interact;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // If have a tool, put it in the bag
        if ((interactor.GetPickedItem() != null) && (interactor.GetPickedItem().ItemType == ItemType.Tool))
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
        ObjectifManager.Instance.Progresser(objectifSO.objectifID, 1);
        SFXManager.instance.PlaySFX(clip);
        StartCoroutine(CutCable());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IItem>(out IItem item))
        {
            if (item.ItemType == ItemType.Tool)
            {
                (item as IInteractable).RemoveInteractable();
                RemoveInteractable();
            }
        }
    }

    public IEnumerator CutCable()
    {
        // Switch model
        particle.SetActive(true);

        // 5 second of being cut
        yield return new WaitForSeconds(5.0f);

        // Slowly turn transparent and then be destroyed
        float elapsedTime = 0.0f;
        while (elapsedTime < 2.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", (2.0f - elapsedTime) / 2.0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
