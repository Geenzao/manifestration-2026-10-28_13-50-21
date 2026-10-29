using UnityEngine;
using System.Collections.Generic;

public class StudentManager : MonoBehaviour
{
    [SerializeField] string state;
    Animator animator;

    List<string> standingStates = new List<string> { "Idle_01", "Idle_02"};
    List<string> sittingStates = new List<string> { "Sitting_Idle_01", "Sitting_Idle_02" };


    void Start()
    {
        animator = GetComponent<Animator>();

        // Set initial state
        PlayRandomAnimationBasedOnState();
    }

    void PlayRandomAnimationBasedOnState()
    {
        List<string> currentStateList = state == "Standing" ? standingStates : sittingStates;
        string randomAnimation = currentStateList[Random.Range(0, currentStateList.Count)];
        animator.Play(randomAnimation);
    }

}
