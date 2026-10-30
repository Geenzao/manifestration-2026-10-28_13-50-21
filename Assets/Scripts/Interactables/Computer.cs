using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private Objectif objectifSO;
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject particle;
    [SerializeField] private TextMeshPro screenText;
    [SerializeField] private AudioClip clip;
    private int wishNmb;
    public InteractType Type => InteractType.Interact;

    private void Start()
    {
        wishNmb = Random.Range(1, 11);
        screenText.text = "PARCOURSUP\nVoeux envoyés : " + wishNmb;
    }

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
        SFXManager.instance.PlaySFX(clip);
        StartCoroutine(SabotageComputer());
    }

    public IEnumerator SabotageComputer()
    {
        // Sabotaged the text
        particle.SetActive(true);

        // Wishes being suppressed for 5s
        float elapsedTime = 0.0f;
        while (elapsedTime < 5.0f)
        {
            screenText.text = "PARCOURSUP\nVoeux envoyés : " + (int)Mathf.Max(0.0f, Mathf.Round(wishNmb - elapsedTime * 2.0f)) + "\n...suppression...";
            if ((wishNmb - elapsedTime * 2.0f) <= 0.5f)
            {
                screenText.color = Color.red;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Slowly turn transparent and then be destroyed
        elapsedTime = 0.0f;
        Color textBaseColor = screenText.color;
        Color textFinalColor = new Color(textBaseColor.r, textBaseColor.g, textBaseColor.b, 0.0f);
        while (elapsedTime < 2.0f)
        {
            normalModel.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", (2.0f - elapsedTime) / 2.0f);
            screenText.color = Color.Lerp(textBaseColor, textFinalColor, (elapsedTime / 2.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
