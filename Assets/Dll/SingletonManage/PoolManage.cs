using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static Attribute;

public class PoolManage
{
    private static PoolManage __instance;
    private Dictionary<string,Stack<GameObject>> poollist = new Dictionary<string, Stack<GameObject>>();
    public static PoolManage Instance
    { 
        get
        {
            if (__instance == null)
            {
                __instance = new PoolManage();
                __instance.poollist = new Dictionary<string, Stack<GameObject>>();
            }
            return __instance; 
        }
    }

    public void ClearPool()
    {
        foreach (var pool in poollist.Values)
        {
            pool.Clear();
        }
    }

    public GameObject GetPoolGameObject(string type, string poolName,Transform parent = null)
    {
        GameObject pool;
    Start:
        if (poollist.ContainsKey(poolName) && poollist[poolName].Count > 0)
        {
            pool = poollist[poolName].Pop();
            if (pool == null)
            {
                goto Start;
            }
            pool.transform.SetParent(parent);
        }
        else
        {
            pool = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + type + "/" + poolName),parent);
        }
        pool.SetActive(true);
        pool.name = poolName;
        return pool;
    }

    public GameObject GetPoolGameObject(string type, string poolName,Vector3 position, Transform parent = null)
    {
        GameObject pool;
    Start:
        if (poollist.ContainsKey(poolName) && poollist[poolName].Count > 0)
        {
            pool = poollist[poolName].Pop();
            if (pool == null)
            {
                goto Start;
            }
            pool.transform.SetParent(parent);
            pool.transform.position = position;
        }
        else
        {
            pool = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + type + "/" + poolName), position, Quaternion.identity, parent);
        }
        pool.SetActive(true);
        pool.name = poolName;
        return pool;
    }

    public void PushGameObject(string poolName, GameObject obj,bool returnnullparent = false)
    {
        if (returnnullparent) { obj.transform.SetParent(null); }
        obj.SetActive(false);
        if (!poollist.ContainsKey(poolName))
        {
            poollist.Add(poolName, new Stack<GameObject>());
        }
        if (!poollist[poolName].Contains(obj))
        {
            poollist[poolName].Push(obj);
        }
    }
}
