using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePool : MonoBehaviour
{
    private const int MaxCount = 1000;

    [SerializeField] private GameObject objectPrefab;
    [Range(0, MaxCount)]
    [SerializeField] private int objectsCount = MaxCount;

    private Stack<GameObject> _inactiveObjects = new Stack<GameObject>();

    private void Start()
    {
        for(var i = 0; i < objectsCount; i++)
        {
            InitObject();
        }
    }

    public T PopObject<T>() where T : MonoBehaviour
    {
        if (_inactiveObjects.Count == 0)
        {
            InitObject();
        }

        _inactiveObjects.Pop().TryGetComponent(out T res);
        return res;
    }

    public void PushObject(GameObject obj)
    {
        _inactiveObjects.Push(obj);
    }

    public bool IsAllObjectsInactive()
    {
        return _inactiveObjects.Count == transform.childCount;
    }

    private void InitObject()
    {
        var obj = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.gameObject.SetActive(false);
        _inactiveObjects.Push(obj);
    }
}
