using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer _frontFace;

    private const float RotateDuration = 0.5f;

    private bool _isRotating;
    private int _cardId;

    public void SetData(CardData data)
    {
        _frontFace.material.mainTexture = data.Texture;
        _cardId = data.Id;
    }

    public void OnClick()
    {
        if (_isRotating)
            return;

        _isRotating = true;

        transform.DOLocalRotate(new Vector3(0f, 180f, 0f), RotateDuration, RotateMode.LocalAxisAdd).OnComplete(OnRotationComplete);
    }

    private void OnRotationComplete()
    {
        _isRotating = false;
    }

    public void OnTouchDown() { }

    public void OnTouchUp() { }
}
