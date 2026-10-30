using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    public bool isUIOpen = false;
    [SerializeField] GameObject settingsUI; // Reference to your settings UI GameObject
    [SerializeField] GameObject creditsUI; // Reference to your pause menu UI GameObject

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void HandleSettings()
    {
        isUIOpen = !isUIOpen; // Toggle the UI state
        settingsUI.SetActive(isUIOpen); // Show or hide the settings UI based on the state

        Cursor.visible = isUIOpen;
    }

    public void HandleCredits()
    {
        isUIOpen = !isUIOpen; // Toggle the UI state
        creditsUI.SetActive(isUIOpen); // Show or hide the credits UI based on the state

        Cursor.visible = isUIOpen;
    }
    
}
