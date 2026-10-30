using System.Collections;
using UnityEngine;

public class Cable : MonoBehaviour, IInteractable
{
    [SerializeField] private Objectif objectifSO;
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject cutModel;
    public InteractType Type => InteractType.Interact;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // If interacted with normally
        RemoveInteractable();
    }

    public void Throw(Vector3 dir, float throwPower)
    {
        return;
    }

    public void RemoveInteractable()
    {
        ObjectifManager.Instance.Progresser(objectifSO.objectifID, 1);
        StartCoroutine(CutCable());
    }

    public IEnumerator CutCable()
    {
        // Switch model
        normalModel.SetActive(false);
        cutModel.SetActive(true);

        // 5 second of being cut
        yield return new WaitForSeconds(5.0f);

        // Slowly turn transparent and then be destroyed
        float elapsedTime = 0.0f;
        while (elapsedTime < 2.0f)
        {
            cutModel.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", (2.0f - elapsedTime) / 2.0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
