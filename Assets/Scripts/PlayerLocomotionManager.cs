using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{   
    PlayerManager player;
    // Values taken from PlayerInputManager
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount; // This is the total amount of movement we are going to be doing in any direction. It is a combination of both vertical and horizontal input.
    
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        // GROUNDED
        HandleGroundedMovement();
        HandleRotation();
        // JUMPING
    }

    void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();

        // Move Direction is based on the camera's forward and right direction, multiplied by the vertical and horizontal movement input values respectively. This allows the player to move in the direction they are facing relative to the camera.
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(PlayerInputManager.instance.moveAmount > 0.5f)
        {
            // Running speed
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if(PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            // Walking speed
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    void HandleRotation()
    {
        // This is the direction we want to rotate towards, based on the camera's forward and right direction, multiplied by the vertical and horizontal movement input values respectively. This allows the player to rotate in the direction they are moving relative to the camera.
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        // This is the new rotation we want to apply to the player, based on the target rotation direction. We use Quaternion.LookRotation to create a rotation that looks in the direction of the target rotation direction.
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
