using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInteractor))]
public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Vector2 _moveDir = Vector2.zero;
    private PlayerInteractor interactor;

    private void Start()
    {
        interactor = GetComponent<PlayerInteractor>();
    }

    private void Update()
    {
        transform.position += new Vector3(_moveDir.x, 0.0f, _moveDir.y) * moveSpeed * Time.deltaTime;
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            _moveDir = ctx.ReadValue<Vector2>();
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            _moveDir = Vector2.zero;
        }
    }
}
