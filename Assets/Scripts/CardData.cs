using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Card Data")]
public class CardData : ScriptableObject, ICollectionItem
{
    [SerializeField] private int _id = default;
    [SerializeField] private Texture _texture = default;

    public Texture Texture => _texture;

    public int GetId()
    {
        return _id;
    }
}
