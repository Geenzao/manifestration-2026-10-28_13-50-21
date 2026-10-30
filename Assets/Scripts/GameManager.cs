using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Hub, RoundEnCours }

    [Header("Références")]
    [SerializeField] private Chrono chrono;
    [SerializeField] private GameObject hubSpawn;
    [SerializeField] private GameObject questSpawn;

    private Transform player;

    [Header("Événements")]
    public UnityEvent<int> onRoundStart;    // numéro du round qui démarre
    public UnityEvent onWin;                // round réussi
    public UnityEvent onLose;               // round raté
    public UnityEvent<int> onReturnToHub;   // numéro du prochain round disponible
    public UnityEvent onGameWon;            // tous les rounds sont terminés

    public GameState State { get; private set; }
    public int CurrentRound { get; private set; } = 1;
    public int TotalRounds => ObjectifManager.Instance.MaxObjectifs;
    public bool IsRoundStarted => State == GameState.RoundEnCours;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");

        if (go != null)
            player = go.transform;
        else
            Debug.LogWarning("GameManager : aucun objet avec le tag Player trouvé.");
    }

    private void Start()
    {
        chrono.onTermine.AddListener(Lose);
        ObjectifManager.Instance.onTousObjectifsTermines.AddListener(Win);

        CurrentRound = 1;
        GoToHub();
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

    // Appelé par l'objet interactif du hub
    public void StartRound()
    {
        if (State != GameState.Hub) return;

        ResetRound();
        ObjectifManager.Instance.AddObjectifs(CurrentRound); // round N => N objectifs
        Teleport(questSpawn);
        chrono.Demarrer();

        State = GameState.RoundEnCours;
        onRoundStart.Invoke(CurrentRound);
    }

    private void Win() => EndRound(true);
    private void Lose() => EndRound(false);

    private void EndRound(bool won)
    {
        if (State != GameState.RoundEnCours) return; // évite Win puis Lose (ou l'inverse)

        chrono.Pause();
        ResetRound();

        if (won)
        {
            onWin.Invoke();

            if (CurrentRound >= TotalRounds)
            {
                onGameWon.Invoke();
                CurrentRound = 1;   // on peut rejouer depuis le round 1
            }
            else
            {
                CurrentRound++;
            }
        }
        else
        {
            onLose.Invoke();
            CurrentRound = 1;       // défaite : retour au round 1
        }

        GoToHub();
    }

    private void GoToHub()
    {
        State = GameState.Hub;
        Teleport(hubSpawn);
        onReturnToHub.Invoke(CurrentRound);
    }

    private void ResetRound()
    {
        ObjectifManager.Instance.ClearObjectives(); // détruit les copies et vide l'UI
        chrono.Reinitialiser();                     // remet le temps et le texte à zéro
    }

    private void Teleport(GameObject cible)
    {
        if (player == null) FindPlayer();
        if (player == null || cible == null) return;

        Transform t = cible.transform;
        CharacterController cc = player.GetComponent<CharacterController>();
        Rigidbody rb = player.GetComponent<Rigidbody>();

        // Un CharacterController écrase la position si on ne le désactive pas
        if (cc != null) cc.enabled = false;

        player.SetPositionAndRotation(t.position, t.rotation);

        if (rb != null)
        {
            rb.position = t.position;
            rb.rotation = t.rotation;
            rb.linearVelocity = Vector3.zero;   // "velocity" avant Unity 6
            rb.angularVelocity = Vector3.zero;
        }

        if (cc != null) cc.enabled = true;

        Physics.SyncTransforms();
    }
}