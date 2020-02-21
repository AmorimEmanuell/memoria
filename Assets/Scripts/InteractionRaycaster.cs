﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    [SerializeField] private LayerMask _rayMask = default;

    private const float ClickThreshold = 0.3f;

    private Dictionary<Collider, IInteractable> _savedInteractables = new Dictionary<Collider, IInteractable>();
    private IInteractable _currentInteractable;
    private float _touchDownTime;
    private RaycastHit[] _hit = new RaycastHit[1];

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleTouchDown();
        else if (Input.GetMouseButtonUp(0))
            HandleTouchUp();
    }

    private void HandleTouchDown()
    {
        _touchDownTime = Time.unscaledTime;

        var interactable = GetInteractable();
        if (interactable != null)
        {
            if (_currentInteractable != interactable)
                _currentInteractable = interactable;

            _currentInteractable.OnTouchDown();
        }
    }

    private void HandleTouchUp()
    {
        var interactable = GetInteractable();
        if (interactable != null)
        {
            if (_currentInteractable == interactable)
            {
                _currentInteractable.OnTouchUp();

                if (Time.unscaledTime - _touchDownTime <= ClickThreshold)
                    _currentInteractable.OnClick();
            }
        }

        _currentInteractable = null;
    }

    private IInteractable GetInteractable()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.RaycastNonAlloc(ray, _hit, Mathf.Infinity, _rayMask) > 0)
        {
            var collider = _hit[0].collider;

            if (_savedInteractables.ContainsKey(collider))
                return _savedInteractables[collider];

            var interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _savedInteractables.Add(collider, interactable);
                return interactable;
            }
        }

        return null;
    }
}
