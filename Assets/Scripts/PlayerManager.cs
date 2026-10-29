using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // CENTRALIZED MANAGER FOR PLAYER
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerLocomotionManager playerLocomotionManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isJumping = false;
    public bool isGrounded = true;

    void Awake()
    {
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        characterController = GetComponent<CharacterController>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        animator = GetComponent<Animator>();

        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
        playerLocomotionManager.HandleAllMovement();
    }

    void LateUpdate()
    {
        PlayerCamera.instance.HandleAllCameraActions();
    }
}
