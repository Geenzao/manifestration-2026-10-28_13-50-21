using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectifsUI : MonoBehaviour
{
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private GameObject objectivePrefab;

    private class Ligne
    {
        public TextMeshProUGUI texte;
        public GameObject check;
    }

    private Dictionary<Objectif, Ligne> lignes = new Dictionary<Objectif, Ligne>();

    public void AddObjective(Objectif objectif)
    {
        GameObject instance = Instantiate(objectivePrefab, objectivePanel.transform);

        Ligne ligne = new Ligne();
        ligne.texte = instance.GetComponentInChildren<TextMeshProUGUI>();
        ligne.check = instance.transform.Find("Check/Image").gameObject;
        ligne.check.SetActive(false);

        lignes.Add(objectif, ligne);
        Refresh(objectif);
    }

    public void Refresh(Objectif objectif)
    {
        if (!lignes.TryGetValue(objectif, out Ligne ligne)) return;

        ligne.texte.text = $"{objectif.objectifDescription} ({objectif.currentAmount}/{objectif.requiredAmount})";
        ligne.check.SetActive(objectif.IsCompleted);
    }

    public void ClearObjectives()
    {
        foreach (Transform child in objectivePanel.transform)
            Destroy(child.gameObject);

        lignes.Clear();
    }
}