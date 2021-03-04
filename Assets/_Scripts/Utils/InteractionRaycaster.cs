using System.Collections.Generic;
using UnityEngine;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private Camera raycastCamera = default;
    [SerializeField] private LayerMask rayMask = default;

    private const float ClickThreshold = 0.3f;

    private readonly Dictionary<Collider, IInteractable> savedInteractables = new Dictionary<Collider, IInteractable>();
    private IInteractable currentInteractable;
    private float touchDownTime;
    private readonly RaycastHit[] hit = new RaycastHit[1];

    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        var touches = Input.touches;
        if (touches.Length > 0)
        {
            var touch = touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchDown(touch.position);
                    break;

                case TouchPhase.Ended:
                    HandleTouchUp(touch.position);
                    break;

                default:
                    break;
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouchDown(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleTouchUp(Input.mousePosition);
        }
#endif
    }

    private void HandleTouchDown(Vector3 inputPosition)
    {
        touchDownTime = Time.unscaledTime;

        var interactable = GetInteractable(inputPosition);
        if (interactable != null)
        {
            if (currentInteractable != interactable)
            {
                currentInteractable = interactable;
            }

            currentInteractable.OnTouchDown();
        }
    }

    private void HandleTouchUp(Vector3 inputPosition)
    {
        var interactable = GetInteractable(inputPosition);
        if (interactable != null)
        {
            if (currentInteractable == interactable)
            {
                currentInteractable.OnTouchUp();

                if (Time.unscaledTime - touchDownTime <= ClickThreshold)
                {
                    currentInteractable.OnClick();
                }
            }
        }

        currentInteractable = null;
    }

    private IInteractable GetInteractable(Vector3 inputPosition)
    {
        var ray = raycastCamera.ScreenPointToRay(inputPosition);
        if (Physics.RaycastNonAlloc(ray, hit, Mathf.Infinity, rayMask) > 0)
        {
            var collider = hit[0].collider;

            if (savedInteractables.ContainsKey(collider))
            {
                return savedInteractables[collider];
            }

            var interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                savedInteractables.Add(collider, interactable);
                return interactable;
            }
        }

        return null;
    }
}
