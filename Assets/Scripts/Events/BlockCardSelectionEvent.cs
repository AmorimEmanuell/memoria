using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCardSelectionEvent : GameEvent
{
    public bool Block { get; private set; }

    public BlockCardSelectionEvent(bool block)
    {
        Block = block;
    }
}
