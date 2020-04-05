using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimatorController : MonoBehaviour
{
    public Action OnDmgAnimationComplete, OnAtkAnimationComplete;

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

    public void DispatchDmgAnimationCompleteEvent()
    {
        OnDmgAnimationComplete?.Invoke();
    }

    public void DispatchAttackAnimationFinished()
    {
        OnAtkAnimationComplete?.Invoke();
    }
}
