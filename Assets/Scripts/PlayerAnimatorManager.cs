using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    PlayerManager player;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void UpdateMovementValue(float newValue)
    {
        player.animator.SetFloat("Movement", newValue);
    }

    public void PlayTargetAnimation(string targetAnim, bool isPerformingAction, bool applyRootMotion = true)
    {
        player.animator.applyRootMotion = applyRootMotion;
        player.animator.CrossFade(targetAnim, 0.2f);
        player.isPerformingAction = isPerformingAction;
    }
}
