using System.Collections;
using UnityEngine;

public class Poster : MonoBehaviour, IInteractable
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
        // If have a flamable object, burn the poster
        if ((interactor.GetPickedItem() != null) && (interactor.GetPickedItem().ItemType == ItemType.Flamable))
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
        StartCoroutine(BurnPoster());
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

    public IEnumerator BurnPoster()
    {
        // Enable the particle
        particle.SetActive(true);

        // Slowly burn black the poster
        float elapsedTime = 0.0f;
        while (elapsedTime < 5.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.Lerp(Color.white, Color.black, (elapsedTime / 5.0f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Slowly turn transparent and then be destroyed
        elapsedTime = 0.0f;
        while (elapsedTime < 2.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", (2.0f - elapsedTime) / 2.0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
