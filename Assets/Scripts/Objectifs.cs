using System;
using Unity.VisualScripting;
using UnityEngine;

public class Objectifs : MonoBehaviour
{
    [SerializeField] private GameObject objectif;
    [SerializeField] private GameObject objectifPrefab;


    public void AddObjective()
    {
        // Instantiate(préfet, position, rotation, parent)
        GameObject instance = Instantiate(objectifPrefab, transform.position, transform.rotation, objectif.transform);
    }
}
