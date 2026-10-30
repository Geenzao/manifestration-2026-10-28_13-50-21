using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scribble : MonoBehaviour, IInteractable
{
    [SerializeField] private Objectif objectifSO;
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject scribbledModel;
    [SerializeField] private AudioClip clip;
    public InteractType Type => InteractType.Interact;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // If have a pencil object, sabotage the sign with
        if ((interactor.GetPickedItem() != null) && (interactor.GetPickedItem().ItemType == ItemType.Pencil))
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
        StartCoroutine(SabotageSign());
    }

    public IEnumerator SabotageSign()
    {
        // Sabotaged the table
        normalModel.SetActive(false);
        scribbledModel.SetActive(true);

        // 5 second pause
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
