using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour     //Object Pool class for pooling.
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _initialPoolSize = 5;
    [SerializeField] private int _maxPoolSize = 11;

    private readonly Queue<GameObject> _pool = new();

    private void Awake()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            AddObjectToPool();
        }
    }

    public GameObject GetObjectFromPool()   //Gets object from pool.
    {
        if (_pool.Count == 0)
        {
            AddObjectToPool();
        }

        GameObject obj = _pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)      //Returns object to pool.
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        if (_pool.Count < _maxPoolSize)
        {
            _pool.Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    private void AddObjectToPool()      //Add object to pool.
    {
        GameObject obj = Instantiate(_objectPrefab, transform);
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}