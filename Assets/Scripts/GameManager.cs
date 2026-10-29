using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Chrono chrono;

    public UnityEvent onWin;
    public UnityEvent onLose;

    public bool IsRoundStarted { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        chrono.onTermine.AddListener(Lose);
        ObjectifManager.Instance.onTousObjectifsTermines.AddListener(Win);
        StartRound(10);
    }

    private void OnDestroy()
    {
        if (chrono != null)
            chrono.onTermine.RemoveListener(Lose);

        if (ObjectifManager.Instance != null)
            ObjectifManager.Instance.onTousObjectifsTermines.RemoveListener(Win);

        if (Instance == this)
            Instance = null;
    }

    public void StartRound(int objectiveAmount)
    {
        if (IsRoundStarted) return;

        ResetRound();
        ObjectifManager.Instance.AddObjectifs(objectiveAmount);
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
        ObjectifManager.Instance.ClearObjectives(); // détruit les copies et vide l’UI
        chrono.Reinitialiser();            // remet le temps et le texte à zéro
    }
}