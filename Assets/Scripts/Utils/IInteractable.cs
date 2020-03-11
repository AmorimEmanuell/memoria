using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnTouchDown();
    void OnClick();
    void OnTouchUp();
}
