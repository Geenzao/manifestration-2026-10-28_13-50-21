using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectifManager objectifManager;
    [SerializeField] private Chrono chrono;

    public UnityEvent onWin;
    public UnityEvent onLose;

    public bool IsRoundStarted { get; private set; }

    private void Awake()
    {
        chrono.onTermine.AddListener(Lose);
        objectifManager.onTousObjectifsTermines.AddListener(Win);
    }

    private void OnDestroy()
    {
        chrono.onTermine.RemoveListener(Lose);
        objectifManager.onTousObjectifsTermines.RemoveListener(Win);
    }

    public void StartRound(int objectiveAmount)
    {
        if (IsRoundStarted) return;

        ResetRound();
        objectifManager.AddObjectifs(objectiveAmount);
        chrono.Demarrer();
        IsRoundStarted = true;
    }

    private void Win() => EndRound(true);
    private void Lose() => EndRound(false);

    private void EndRound(bool won)
    {
        if (!IsRoundStarted) return; // évite Win puis Lose (ou l'inverse)

        IsRoundStarted = false;
        chrono.Pause();

        if (won) onWin.Invoke();
        else onLose.Invoke();

        ResetRound();
    }

    private void ResetRound()
    {
        objectifManager.ClearObjectives(); // détruit les copies et vide l'UI
        chrono.Reinitialiser();            // remet le temps et le texte à zéro
    }
}