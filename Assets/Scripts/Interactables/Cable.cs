using System.Collections;
using UnityEngine;

public class Cable : MonoBehaviour, IInteractable
{
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
        Color baseColor = cutModel.GetComponent<MeshRenderer>().material.color;
        Color finalColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);
        while (elapsedTime < 2.0f)
        {
            cutModel.GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, finalColor, (elapsedTime / 2.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
