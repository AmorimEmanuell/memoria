using UnityEngine;

public class WindowPanel : MonoBehaviour
{
    [SerializeField] private GameObject _myWindowObj;

    public virtual void Open() 
    {
        _myWindowObj.SetActive(true);
    }

    public virtual void Close() 
    {
        _myWindowObj.SetActive(false);
    }
}
