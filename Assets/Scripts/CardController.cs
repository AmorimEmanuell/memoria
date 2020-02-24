using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer _frontFace = default;

    public Action<CardController> OnFaceRevealed;

    public int CardId { get; private set; }
    public bool IsInteractable { get; set; }

    private const float RotateDuration = 0.5f;

    private float _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale.x;
    }

    public void SetData(CardData data)
    {
        _frontFace.material.mainTexture = data.Texture;
        CardId = data.GetId();
    }

    private void RevealFace()
    {
        IsInteractable = false;
        transform.DOLocalRotate(new Vector3(0f, 180f, 0f), RotateDuration).OnComplete(() =>
        {
            OnFaceRevealed?.Invoke(this);
        });
    }

    public void HideFace(Action onComplete = null)
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, 0f), RotateDuration).OnComplete(() =>
        {
            IsInteractable = true;
            onComplete?.Invoke();
        });
    }

    public void Shrink(Action onComplete = null)
    {
        transform.DOScale(0f, RotateDuration).OnComplete(() =>
        {
            ReturnToPool();
            onComplete?.Invoke();
        });
    }

    public void ReturnToPool()
    {
        transform.localScale = Vector3.one * _originalScale;
        transform.localRotation = Quaternion.identity;

        ObjectPooler.Instance.Return(gameObject);
    }

    public void OnClick()
    {
        if (!IsInteractable)
        {
            return;
        }

        RevealFace();
    }

    public void OnTouchDown() { }

    public void OnTouchUp() { }
}
