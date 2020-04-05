using System;
using UnityEngine;

public class PotionPickup : MonoBehaviour, IInteractable
{
    public Action<PotionPickup> OnPickup;

    public void OnClick()
    {
    }

    public void OnTouchDown()
    {
        OnPickup?.Invoke(this);
    }

    public void OnTouchUp()
    {
    }
}
