using System;
using UnityEngine;

public class InteractiveResponseBlendedAnimation : InteractiveResponse
{
    public float animationSpeed = 1f;
    
    private Animator _animator;
    private float blendParam = .0f;

    private int currIncrement = -1;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        blendParam = Mathf.Clamp(blendParam + animationSpeed * Time.deltaTime * currIncrement, 0, 1) ;
        _animator.SetFloat("blendParam", blendParam);
    }

    public override void DoResponseAction()
    {
        currIncrement = 1;
    }

    public override void UndoResponseAction()
    {
        currIncrement = -1;
    }

    public override bool IsAvailable()
    {
        return true;
    }
}