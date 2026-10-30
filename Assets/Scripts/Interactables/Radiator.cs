using System.Collections;
using UnityEngine;

public class Radiator : MonoBehaviour, IInteractable
{
    [SerializeField] private Objectif objectifSO;
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject particle;
    public InteractType Type => InteractType.Interact;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // If have a tool, damage the radiator
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
        StartCoroutine(SabotageRadiator());
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

    public IEnumerator SabotageRadiator()
    {
        // Switch model and activate stuff
        water.SetActive(true);
        particle.SetActive(true);

        // Water leaking for 5s
        float elapsedTime = 0.0f;
        while (elapsedTime < 5.0f)
        {
            water.transform.localScale = Vector3.one * (elapsedTime / 5.0f) * 0.5f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Slowly turn transparent and then be destroyed
        elapsedTime = 0.0f;
        Color baseWater = water.GetComponent<SpriteRenderer>().color;
        Color transparentWater = new Color(baseWater.r, baseWater.g, baseWater.b, 0.0f);
        while (elapsedTime < 2.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", (2.0f - elapsedTime) / 2.0f);
            water.GetComponent<SpriteRenderer>().color = Color.Lerp(baseWater, transparentWater, (elapsedTime / 2.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
