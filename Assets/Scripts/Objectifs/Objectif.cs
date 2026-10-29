using UnityEngine;

[CreateAssetMenu(fileName = "Objectif", menuName = "Objectifs/Objectif")]
public class Objectif : ScriptableObject
{
    public string objectifID;
    public string objectifDescription;
    [HideInInspector] public int requiredAmount;
    [HideInInspector] public int currentAmount;
    public int minAmount;
    public int maxAmount;

    public bool IsCompleted => currentAmount >= requiredAmount;
}
