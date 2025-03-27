using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rikayon : MonoBehaviour {

    public Animator animator;

    void Start()
    {
        animator.SetTrigger("Walk_Cycle_2");
    }
}
