using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseAnimation : InteractiveResponse
{
    public string doAnimationName;
    public string undoAnimationName;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void DoResponseAction()
    {
        animator.Play(doAnimationName);
        Debug.Log("Played animation");
    }

    public override void UndoResponseAction()
    {
        animator.Play(undoAnimationName);
    }
}
