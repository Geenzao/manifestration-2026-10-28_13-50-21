using System.Collections.Generic;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    public List<Objectif> objectifs;
    public ObjectifsUI objectifsUI;
    private List<Objectif> objectifsCopies = new List<Objectif>();

    public void Start()
    {
        AddObjectifs(5);
    }

    public void AddObjectifs(int amount)
    {
        List<Objectif> disponibles = new List<Objectif>(objectifs);
        disponibles.RemoveAll(m => objectifsCopies.Exists(o => o.objectifID == m.objectifID));

        amount = Mathf.Min(amount, disponibles.Count);

        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, disponibles.Count);
            Objectif modele = disponibles[index];
            disponibles.RemoveAt(index);

            Objectif obj = Instantiate(modele);
            obj.requiredAmount = Random.Range(modele.minAmount, modele.maxAmount + 1);
            obj.currentAmount = 0;

            objectifsCopies.Add(obj);
            objectifsUI.AddObjective(obj);
        }
    }

    public void Progresser(string id, int quantite)
    {
        foreach (Objectif obj in objectifsCopies)
        {
            if (obj.objectifID != id || obj.IsCompleted) continue;

            obj.currentAmount = Mathf.Min(obj.currentAmount + quantite, obj.requiredAmount);
            objectifsUI.Refresh(obj);
        }
    }

    public void ClearObjectives()
    {
        foreach (Objectif obj in objectifsCopies)
            Destroy(obj);

        objectifsCopies.Clear();
        objectifsUI.ClearObjectives();
    }
}
