using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    private RuntimeAnimatorController defaultAnimController;

    private void Start() {
        defaultAnimController = animator.runtimeAnimatorController;
    }

    public void SetAnimator(AnimatorOverrideController newAnimController){
        animator.runtimeAnimatorController = newAnimController;
    }

    public RuntimeAnimatorController GetCurrentAnimator(){
        return animator.runtimeAnimatorController;
    }

    public void ResetAnimator(){
        animator.runtimeAnimatorController = defaultAnimController;
    }
}
