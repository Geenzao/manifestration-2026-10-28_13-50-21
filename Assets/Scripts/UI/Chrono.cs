using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Chrono : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text texte;

    [Header("Réglages")]
    [SerializeField] private bool compteARebours = true;
    [SerializeField] private float dureeInitiale = 120f; // secondes (utilisé pour le compte à rebours)
    [SerializeField] private bool demarrerAuLancement = true;

    [Header("Événements")]
    public UnityEvent onTermine; // appelé quand le compte à rebours atteint 0

    private float temps;
    private bool enCours;
    private int derniereSecondeAffichee = -1;

    public float TempsActuel => temps;
    public bool EnCours => enCours;

    private void Start()
    {
        Reinitialiser();
        if (demarrerAuLancement) Demarrer();
    }

    private void Update()
    {
        if (!enCours) return;

        if (compteARebours)
        {
            temps -= Time.deltaTime;
            if (temps <= 0f)
            {
                temps = 0f;
                enCours = false;
                MettreAJourTexte();
                onTermine?.Invoke();
                return;
            }
        }
        else
        {
            temps += Time.deltaTime;
        }

        MettreAJourTexte();
    }

    public void Demarrer() => enCours = true;

    public void Pause() => enCours = false;

    public void Reinitialiser()
    {
        temps = compteARebours ? dureeInitiale : 0f;
        derniereSecondeAffichee = -1;
        MettreAJourTexte();
    }

    public void AjouterTemps(float secondes)
    {
        temps = Mathf.Max(0f, temps + secondes);
        MettreAJourTexte();
    }

    private void MettreAJourTexte()
    {
        if (texte == null) return;

        // Ceil pour le compte à rebours (affiche 0:01 jusqu'à la fin), Floor pour le chrono classique
        int secondes = compteARebours ? Mathf.CeilToInt(temps) : Mathf.FloorToInt(temps);

        // On ne réécrit le texte que quand la seconde change (évite des allocations inutiles)
        if (secondes == derniereSecondeAffichee) return;
        derniereSecondeAffichee = secondes;

        texte.text = $"{secondes / 60:00}:{secondes % 60:00}";
    }
}