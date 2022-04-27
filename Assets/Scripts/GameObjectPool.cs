using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEditor.SceneTemplate;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameObjectPool : MonoBehaviour
{
    public int InitSize { get; set; }
    public int MaxSize { get; set; }
    public GameObject Prefab { get; set; }
    private Queue<GameObject> _pool;

    public GameObjectPool(int initSize, int maxSize, GameObject prefab)
    {
        InitSize = initSize;
        MaxSize = maxSize;
        Prefab = prefab;
        Warm();
    }

    public void Warm()
    {
        _pool.Clear();
        for (int i = 0; i < InitSize; i++)
        {
            GameObject tmp = Instantiate(Prefab);
            _pool.Enqueue(tmp);
            tmp.SetActive(false);
        }
    }

    public GameObject Get()
    {
        if (_pool.Count == 0)
        {
            GameObject tmp = Instantiate(Prefab);
            return tmp;
        }

        GameObject ret = _pool.Dequeue();
        ret.SetActive(true);
        // ret.GetComponent<Rigidbody>().WakeUp();
        // Debug.Log("Get() " + ret.GetInstanceID());
        return ret;
    }

    public void Release(GameObject go)
    {
        if (_pool.Count == MaxSize)
        {
            Destroy(go);
            return;
        }

        var rigidbody = go.GetComponent<Rigidbody>();
        rigidbody.Sleep();
        rigidbody.position = Vector3.zero;
        go.transform.position = Vector3.zero;
        go.SetActive(false);
        _pool.Enqueue(go);
    }

    private void Awake()
    {
        _pool = new Queue<GameObject>();
    }
}