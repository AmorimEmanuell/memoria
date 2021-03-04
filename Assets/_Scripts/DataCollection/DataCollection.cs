using System.Collections.Generic;
using UnityEngine;

public class DataCollection<T> : MonoBehaviour where T : ICollectionItem
{
    [SerializeField] private T[] _data = default;

    public T[] GetCollection()
    {
        return _data;
    }

    public List<int> GetIds()
    {
        var ids = new List<int>();

        for (var i = 0; i < _data.Length; i++)
            ids.Add(_data[i].GetId());

        return ids;
    }

    public T GetItem(int id)
    {
        for (var i = 0; i < _data.Length; i++)
        {
            if (_data[i].GetId() == id)
                return _data[i];
        }

        Debug.LogError($"CardDataCollection:GetData - CardId {id} not found!");
        return default;
    }
}

public interface ICollectionItem
{
    int GetId();
}
