using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour, IInteractable
{
    private const float RotateDuration = 0.5f;

    private bool _isRotating;

    public void OnClick()
    {
        if (_isRotating)
            return;

        _isRotating = true;

        transform.DOLocalRotate(new Vector3(0f, 180f, 0f), RotateDuration, RotateMode.LocalAxisAdd).OnComplete(OnRotationComplete);
    }

    public void OnTouchDown() { }

    public void OnTouchUp() { }

    private void OnRotationComplete()
    {
        _isRotating = false;
    }
}
