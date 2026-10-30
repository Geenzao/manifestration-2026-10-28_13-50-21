using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjetObjectif
{
    public GameObject prefab;
    public List<Transform> positions;   // positions possibles dans la scène
}

[System.Serializable]
public class ObjectifSpawnEntree
{
    public Objectif objectif;           // le modèle (ScriptableObject)
    public List<ObjetObjectif> objets;  // objets associés à cet objectif
}

public class ObjectifSpawner : MonoBehaviour
{
    [SerializeField] private List<ObjectifSpawnEntree> entrees;
    [SerializeField] private Transform conteneur; // optionnel : parent des objets spawnés

    private readonly List<GameObject> instances = new List<GameObject>();
    private readonly HashSet<Transform> occupees = new HashSet<Transform>();

    // Pour chaque objet associé, fait apparaître requiredAmount exemplaires
    public void Spawn(Objectif objectif)
    {
        ObjectifSpawnEntree entree = entrees.Find(e =>
            e.objectif != null && e.objectif.objectifID == objectif.objectifID);

        if (entree == null)
        {
            Debug.LogWarning($"ObjectifSpawner : aucune entrée pour l'objectif '{objectif.objectifID}'.");
            return;
        }

        foreach (ObjetObjectif objet in entree.objets)
        {
            if (objet.prefab == null) continue;

            foreach (Transform pos in ChoisirPositions(objet.positions, objectif.requiredAmount))
            {
                GameObject go = Instantiate(objet.prefab, pos.position, pos.rotation, conteneur);
                instances.Add(go);
                occupees.Add(pos);
            }
        }
    }

    // Détruit tous les objets spawnés (fin de round)
    public void Clear()
    {
        foreach (GameObject go in instances)
        {
            if (go != null) Destroy(go);
        }

        instances.Clear();
        occupees.Clear();
    }

    // Tire "amount" positions au hasard, en évitant celles déjà occupées ce round
    private List<Transform> ChoisirPositions(List<Transform> positions, int amount)
    {
        List<Transform> resultat = new List<Transform>();
        if (positions == null) return resultat;

        List<Transform> libres = positions.FindAll(p => p != null && !occupees.Contains(p));
        List<Transform> prises = positions.FindAll(p => p != null && occupees.Contains(p));

        Melanger(libres);
        Melanger(prises);
        libres.AddRange(prises); // les positions déjà prises ne servent qu'en dernier recours

        if (libres.Count == 0) return resultat;

        if (libres.Count < amount)
            Debug.LogWarning($"ObjectifSpawner : {libres.Count} position(s) pour {amount} objet(s), certaines seront réutilisées.");

        for (int i = 0; i < amount; i++)
            resultat.Add(libres[i % libres.Count]);

        return resultat;
    }

    private static void Melanger<T>(List<T> liste) // Fisher-Yates
    {
        for (int i = liste.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (liste[i], liste[j]) = (liste[j], liste[i]);
        }
    }
}