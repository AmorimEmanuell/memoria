using System;
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
    private bool _overrideIsInteractable;

    private void Awake()
    {
        _originalScale = transform.localScale.x;
    }

    public void SetData(CardData data)
    {
        _frontFace.material.mainTexture = data.Texture;
        CardId = data.GetId();

        _overrideIsInteractable = true;
    }

    private void RevealFace()
    {
        IsInteractable = false && _overrideIsInteractable;

        transform.DOLocalRotate(new Vector3(0f, 180f, 0f), RotateDuration).OnComplete(() =>
        {
            OnFaceRevealed?.Invoke(this);
        });
    }

    public void HideFace(Action onComplete = null)
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, 0f), RotateDuration).OnComplete(() =>
        {
            IsInteractable = true && _overrideIsInteractable;
            onComplete?.Invoke();
        });
    }

    public void Shrink(Action<CardController> onComplete = null)
    {
        transform.DOScale(0f, RotateDuration).OnComplete(() =>
        {
            onComplete?.Invoke(this);
        });
    }

    public void ResetState()
    {
        transform.localScale = Vector3.one * _originalScale;
        transform.localRotation = Quaternion.identity;
    }

    public void ForceIsInteractable(bool IsInteractable)
    {
        _overrideIsInteractable = IsInteractable;
        this.IsInteractable = IsInteractable;
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
