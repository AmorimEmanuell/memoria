using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardController : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer _frontFace = default;

    public Action<CardController> OnRevealFace;

    public int CardId { get; private set; }

    private const float RotateDuration = 0.5f;

    private bool _isRotating, _isRevealed;

    public void SetData(CardData data)
    {
        _frontFace.material.mainTexture = data.Texture;
        CardId = data.Id;
    }

    private void RevealFace()
    {
        _isRotating = true;
        transform.DOLocalRotate(new Vector3(0f, 180f, 0f), RotateDuration).OnComplete(OnRevealFaceComplete);
    }

    private void OnRevealFaceComplete()
    {
        _isRotating = false;
        _isRevealed = true;

        OnRevealFace?.Invoke(this);
    }

    public void HideFace(Action onComplete = null)
    {
        _isRotating = true;
        transform.DOLocalRotate(new Vector3(0f, 0f, 0f), RotateDuration).OnComplete(() =>
        {
            OnHideFaceComplete();
            onComplete?.Invoke();
        });
    }

    private void OnHideFaceComplete()
    {
        _isRotating = false;
        _isRevealed = false;
    }

    public void Shrink(Action onComplete = null)
    {
        transform.DOScale(0f, RotateDuration).OnComplete(() =>
        {
            OnRotateAndShrinkComplete();
            onComplete?.Invoke();
        });
    }

    private void OnRotateAndShrinkComplete()
    {
        _isRevealed = false;
        _isRotating = false;

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        ObjectPooler.Instance.Return(gameObject);
    }

    public void OnClick()
    {
        if (_isRotating || _isRevealed)
            return;

        RevealFace();
    }

    public void OnTouchDown() { }

    public void OnTouchUp() { }
}
