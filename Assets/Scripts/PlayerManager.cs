using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // CENTRALIZED MANAGER FOR PLAYER
    public CharacterController characterController;

    PlayerLocomotionManager playerLocomotionManager;

    void Awake()
    {
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        characterController = GetComponent<CharacterController>();

        PlayerCamera.instance.player = this;
    }

    void Update()
    {
        playerLocomotionManager.HandleAllMovement();
    }

    void LateUpdate()
    {
        PlayerCamera.instance.HandleAllCameraActions();
    }
}
