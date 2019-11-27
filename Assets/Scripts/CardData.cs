using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] private int _id = default;
    [SerializeField] private Texture _texture = default;

    public int Id => _id;
    public Texture Texture => _texture;
}
