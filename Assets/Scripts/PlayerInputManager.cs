using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;
    PlayerControls playerControls;

    [Header("Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount; // This is the total amount of movement we are going to be doing in any direction. It is a combination of both vertical and horizontal input.

    [Header("Action Input")]
    [SerializeField] bool jumpInput = false;

    [Header("UI Input")]
    [SerializeField] bool settingsInput = false;


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

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnSceneChanged(Scene previousScene, Scene currentScene)
    {   
        if (currentScene.name == "MainMenu")
        {
            instance.enabled = false;
        }
        else
        {   
            instance.enabled = true;
        }
    }

    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // Takes values when input is performed and give it to our movementInput variable
            playerControls.PlayerMovement.Movement.performed += input => movementInput = input.ReadValue<Vector2>();
            playerControls.PlayerActions.Jump.performed += input => jumpInput = true;
            playerControls.PlayerUI.OpenSettings.performed += input => settingsInput = true;
        }

        playerControls.Enable();
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }
    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleSettingsInput();
    }
    void HandleMovementInput()
    {  
        if (PlayerUIManager.instance.isUIOpen)
            return;

        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // This is the total amount of movement we are going to be doing in any direction. It is a combination of both vertical and horizontal input.
        // Mathf.Abs is used to get the absolute value of the input, so that we can get the total amount of movement regardless of the direction.
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // Manual clamping to make movement feels smoother (optional)
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        player.playerAnimatorManager.UpdateMovementValue(moveAmount);
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {   
            jumpInput = false;

            if (PlayerUIManager.instance.isUIOpen)
                return;

            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    private void HandleSettingsInput()
    {
        if (settingsInput)
        {
            settingsInput = false;
            PlayerUIManager.instance.HandleSettings();
        }
    }

}
