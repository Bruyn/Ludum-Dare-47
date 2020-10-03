using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseAnimation : InteractiveResponse
{
    public string animationName;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void DoResponseAction()
    {
        animator.Play(animationName);
    }
}
