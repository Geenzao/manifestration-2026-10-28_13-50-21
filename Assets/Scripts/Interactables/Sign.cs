using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject normalModel;
    [SerializeField] private TextMeshPro signText;
    [SerializeField] private List<string> normalMessages;
    [SerializeField] private List<string> sabotagedMessages;
    public InteractType Type => InteractType.Interact;

    private void Start()
    {
        signText.text = normalMessages[Random.Range(0, normalMessages.Count)];
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteractor interactor)
    {
        // If have a pencil object, sabotage the sign with
        if (interactor.GetPickedItem().ItemType == ItemType.Pencil)
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
        StartCoroutine(SabotageSign());
    }

    public IEnumerator SabotageSign()
    {
        // Sabotaged the text
        signText.text = sabotagedMessages[Random.Range(0, sabotagedMessages.Count)];

        // 5 second pause
        yield return new WaitForSeconds(5.0f);

        // Slowly turn transparent and then be destroyed
        float elapsedTime = 0.0f;
        Color baseColor = normalModel.GetComponent<MeshRenderer>().material.color;
        Color textBaseColor = signText.color;
        Color finalColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);
        Color textFinalColor = new Color(textBaseColor.r, textBaseColor.g, textBaseColor.b, 0.0f);
        while (elapsedTime < 2.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, finalColor, (elapsedTime / 2.0f));
            signText.color = Color.Lerp(textBaseColor, textFinalColor, (elapsedTime / 2.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
