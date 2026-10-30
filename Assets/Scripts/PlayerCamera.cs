using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerManager player;
    public static PlayerCamera instance;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform; // This is the transform that we will be rotating up and down to look up and down.


    // Tweak camera performance
    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1f; // The bigger this value is, the slower the camera will reach the target position.
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -10; // Lowest point player can look down
    [SerializeField] float maximumPivot = 15; // Highest point player can look up
    [SerializeField] float cameraCollisionRadius = 0.2f; // The radius of the sphere that will be used to check for collisions with the environment.
    [SerializeField] LayerMask collisionLayers; // The layers that the camera will collide with. This should be set to everything except the player layer.
    // Tweak values
    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; // Used for cam collisions
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition; // The default position of the camera pivot transform, which is the position we will return to when there are no collisions with the environment.
    private float targetCameraZPosition; // The target position of the camera pivot transform, which is the position we will move to when there are collisions with the environment.


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
        cameraZPosition = cameraObject.transform.localPosition.z; // Store the default position of the camera pivot transform.
    }

    public void HandleAllCameraActions()
    {   
        if(player == null)
            return;

        // FOLLOW
        HandleFollowTarget();
        // ROTATE
        HandleRotations();
        // COLLIDE
        HandleCollisions();

    }

    void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    void HandleRotations()
    {   
        // If we want, we could add an input to control the camera with the mouse, but for now, we will just use the player's movement input to control the camera's rotation.
        // It would work the same way as the player's movement input, but we would need to add a new input action for the camera's rotation. For now, we will just use the player's movement input to control the camera's rotation.
        leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime; // Rotate left and right

        // If we switch to mouse reading inputs, we would uncomment the following code:
        upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime; // Rotate up and down
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot); // Clamp the up and down rotation so the player can't look too far up or down

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // Rotate this gameobject (left and right)
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // Rotate the camera pivot (up and down) // Only if we switch to mouse reading inputs
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position; // This is the direction from the camera pivot transform to the camera object. We will use this direction to check for collisions with the environment.
        direction.Normalize();

        // If the sphere cast hits something, we will move the camera pivot transform closer to the player so that the camera doesn't clip through the environment.
        if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collisionLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        // If the target camera position is too close to the player, we will set it to the minimum distance so that the camera doesn't clip through the player.
        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        // Smoothly move the camera pivot transform to the target position
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
