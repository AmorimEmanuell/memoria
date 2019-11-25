using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour, IInteractable
{
    public void OnClick()
    {
        Debug.Log(name + " OnClick");
    }

    public void OnTouchDown()
    {
        Debug.Log(name + " OnTouchDown");
    }

    public void OnTouchUp()
    {
        Debug.Log(name + " OnTouchUp");
    }
}
