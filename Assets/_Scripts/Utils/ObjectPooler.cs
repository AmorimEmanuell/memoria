using System.Collections.Generic;
using UnityEngine;

public enum PoolItemType
{
    Card,
    Cell
}

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] private List<ObjectPoolItem> _objectsToPool;

    private readonly Dictionary<PoolItemType, List<GameObject>> _pools = new Dictionary<PoolItemType, List<GameObject>>();

    private void Awake()
    {
        for (var i = 0; i < _objectsToPool.Count; i++)
        {
            _pools.Add(_objectsToPool[i].ItemType, new List<GameObject>());
        }
    }

    public GameObject Retrieve(PoolItemType itemType)
    {
        var pooledObjects = _pools[itemType];

        for (var i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return CreateNewObject(itemType);
    }

    private GameObject CreateNewObject(PoolItemType itemType)
    {
        GameObject objToCreate = null;

        for (var i = 0; i < _objectsToPool.Count; i++)
        {
            if (_objectsToPool[i].ItemType == itemType)
            {
                objToCreate = _objectsToPool[i].Prefab;
                break;
            }
        }

        var newObj = Instantiate(objToCreate, transform);
        newObj.SetActive(false);
        _pools[itemType].Add(newObj);
        return newObj;
    }

    public void Return(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(transform);
    }
}

[System.Serializable]
public class ObjectPoolItem
{
    [SerializeField] private PoolItemType _itemType = default;
    [SerializeField] private GameObject _prefab = default;

    public PoolItemType ItemType
    {
        get { return _itemType; }
    }

    public GameObject Prefab
    {
        get { return _prefab; }
    }
}
