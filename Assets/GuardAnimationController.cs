using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAnimationController : MonoBehaviour
{

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        if(!animator || animator == null){
            Debug.Log("No animator found for GuardAnimationController on " + gameObject.name);
        }
    }

    public void SetIsWalking(bool t){
        animator.SetBool("isWalking", t);
    }
    
    
}
