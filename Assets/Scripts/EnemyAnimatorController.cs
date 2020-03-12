using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimatorController : MonoBehaviour
{
    public Action OnDamageAnimationFinished, OnAttackAnimationFinished;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetInteger(string key, int value)
    {
        _animator.SetInteger(key, value);
    }

    public void SetTrigger(string key)
    {
        _animator.SetTrigger(key);
    }

    public void DispatchDamageAnimationFinishedEvent()
    {
        OnDamageAnimationFinished?.Invoke();
    }

    public void DispatchAttackAnimationFinished()
    {
        OnAttackAnimationFinished?.Invoke();
    }
}
