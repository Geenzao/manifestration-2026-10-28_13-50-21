using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectifManager : MonoBehaviour
{
    public static ObjectifManager Instance { get; private set; }

    [SerializeField] private List<Objectif> objectifs;
    [SerializeField] private ObjectifsUI objectifsUI;
    [SerializeField] private ObjectifSpawner spawner;
    public UnityEvent onTousObjectifsTermines;
    private List<Objectif> objectifsCopies = new List<Objectif>();
    private bool objectifsTerminesNotifie;

    // Nombre d'objectifs différents disponibles = nombre de rounds max
    public int MaxObjectifs => objectifs.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // Tire "amount" objectifs différents au hasard et fait apparaître leurs objets
    public void AddObjectifs(int amount)
    {
        objectifsTerminesNotifie = false;

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

            if (spawner != null)
                spawner.Spawn(obj); // après requiredAmount, pour spawn le bon nombre
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

        if (!objectifsTerminesNotifie &&
            objectifsCopies.Count > 0 &&
            objectifsCopies.TrueForAll(obj => obj.IsCompleted))
        {
            objectifsTerminesNotifie = true;
            onTousObjectifsTermines?.Invoke();
        }
    }

    public void ClearObjectives()
    {
        foreach (Objectif obj in objectifsCopies)
            Destroy(obj);

        objectifsCopies.Clear();
        objectifsTerminesNotifie = false;
        objectifsUI.ClearObjectives();

        if (spawner != null)
            spawner.Clear();
    }
}