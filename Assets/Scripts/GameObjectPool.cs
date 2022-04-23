using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class GameObjectPool : ScriptableObject
{
    public int InitSize { get; set; }
    public int MaxSize { get; set; }
    public GameObject Prefab { get; set; }
    private Stack<GameObject> _pool;

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
            _pool.Push(tmp);
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

        GameObject ret = _pool.Pop();
        ret.SetActive(true);
        return ret;
    }

    public void Release(GameObject go)
    {
        if (_pool.Count == MaxSize)
        {
            Destroy(go);
            return;
        }

        go.SetActive(false);
        _pool.Push(go);
    }
}