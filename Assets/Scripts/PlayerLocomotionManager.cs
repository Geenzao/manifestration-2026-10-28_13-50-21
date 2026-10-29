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

    [Header("Ground Check & Jump")]
    [SerializeField] float gravityForce = -9.81f; // Force at which player is pulled down.
    [SerializeField] LayerMask groundLayer; // Transform at the player's feet to check for ground.
    [SerializeField] float groundCheckDistance = 0.4f; // Distance from the player's feet to check for ground.
    [SerializeField] Vector3 yVelocity; // Force at which player is pulled up or down.
    [SerializeField] float groundedYVelocity = -20; // Force at which player is sticking to the ground while grounded.
    [SerializeField] float jumpHeight = 1f; // Height of the jump.


    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    void Update()
    {
        HandleGroundCheck();

        if(player.isGrounded)
        {
            if(yVelocity.y < 0)
            {
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
 
            yVelocity.y += gravityForce * Time.deltaTime;
        }

        // Apply the vertical velocity to the character controller
        player.characterController.Move(yVelocity * Time.deltaTime);
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
    
    void HandleGroundCheck()
    {
        player.isGrounded = Physics.CheckSphere(player.transform.position, groundCheckDistance, groundLayer);
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

    public void AttemptToPerformJump()
    {
        if (player.isJumping)
            return;

        if (!player.isGrounded)
            return;

        player.playerAnimatorManager.PlayTargetAnimation("Rat_Jump_01", true);

        player.isJumping = true;
    }

    public void ApplyJumpingVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce); // 1.5f is the jump height, you can adjust this value to change the jump height.
    }

}
