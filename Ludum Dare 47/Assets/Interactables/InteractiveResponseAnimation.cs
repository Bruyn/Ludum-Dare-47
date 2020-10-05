using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseAnimation : InteractiveResponse
{
    public string doAnimationName;
    public string undoAnimationName;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void DoResponseAction()
    {
        _animator.Play(doAnimationName);
        Debug.Log("Played animation");
    }

    public override void UndoResponseAction()
    {
        _animator.Play(undoAnimationName);
    }

    public override bool IsAvailable()
    {
        // var m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        // var m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
        // var m_ClipName = m_CurrentClipInfo[0].clip.name;

        var info = _animator.GetNextAnimatorStateInfo(0);
        var nameHash = info.fullPathHash;
        var value = info.IsName("IDLE");
        var tag = info.IsTag("IDLE");
        return true;
    }
}
