using System;
using System.Collections;
using System.Collections.Generic;
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
