using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataCollection : MonoBehaviour
{
    [SerializeField] private CardData[] _data = default;

    public CardData[] GetData()
    {
        return _data;
    }

    public List<int> GetIds()
    {
        var ids = new List<int>();

        for (var i = 0; i < _data.Length; i++)
            ids.Add(_data[i].Id);

        return ids;
    }

    public CardData GetData(int cardId)
    {
        for (var i = 0; i < _data.Length; i++)
        {
            if (_data[i].Id == cardId)
                return _data[i];
        }

        Debug.LogError($"CardDataCollection:GetData - CardId {cardId} not found!");
        return null;
    }
}
