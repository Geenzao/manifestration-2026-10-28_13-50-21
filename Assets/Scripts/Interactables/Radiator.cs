using System.Collections;
using UnityEngine;

public class Radiator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject switchModel;
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

    public void Throw(Vector2 dir, float throwPower)
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
        switchModel.GetComponent<MeshRenderer>().material.color = Color.red;

        // 5 second of being cut
        yield return new WaitForSeconds(5.0f);

        // Slowly turn transparent and then be destroyed
        float elapsedTime = 0.0f;
        Color baseColor = normalModel.GetComponent<MeshRenderer>().material.color;
        Color switchBaseColor = normalModel.GetComponent<MeshRenderer>().material.color;
        Color finalColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);
        Color switchFinalColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);
        while (elapsedTime < 2.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, finalColor, (elapsedTime / 2.0f));
            switchModel.GetComponent<MeshRenderer>().material.color = Color.Lerp(switchBaseColor, switchFinalColor, (elapsedTime / 2.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
