using Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager>
{
    static Dictionary<GameObject, BasePool> objectPoolDict = new Dictionary<GameObject, BasePool>();
    [SerializeField] List<GameObject> beforeLoadObject;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < beforeLoadObject.Count; i++)
        {
            GenerateObject<Component>(beforeLoadObject[i]).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// (If pool not exist, create new pool)
    /// Get an object from pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefab"> Object need to generate</param>
    /// <param name="root"> Parent of pool </param>
    /// <param name="amount"> Number of object init </param>
    /// <returns></returns>
    public static T GenerateObject<T>(GameObject prefab, GameObject root = null, int? amount = null) where T : Component
    {
        BasePool pool;
        T obj;
        if (root == null)
        {
            root = new GameObject();
            root.transform.parent = Instance.transform;
            root.name = prefab.name.ToString() + " Pool";
        }
        if (!objectPoolDict.ContainsKey(prefab))
        {
            if (PoolConfig.InitPool.ContainsKey(prefab))
            {
                pool = new BasePool(prefab, PoolConfig.InitPool[prefab]);
            }
            else
            {
                if (amount == null)
                {
                    amount = PoolConfig.DefaultInitPoolGO;
                }
                pool = new BasePool(prefab, (int)amount);
            }
            objectPoolDict.TryAdd(prefab, pool);
        }
        else
        {
            pool = objectPoolDict[prefab];
        }
        obj = pool.GetItem<T>();
        obj.transform.parent = Instance.transform;
        return obj;
    }

    public static T GenerateObject<T>(GameObject prefab, Vector3 pos, GameObject root = null) where T : Component
    {
        BasePool pool;
        T obj;
        if (root == null)
        {
            root = new GameObject();
            root.transform.parent = Instance.transform;
            root.name = typeof(T).ToString() + " Pool";
        }
        if (!objectPoolDict.ContainsKey(prefab))
        { 

            if (PoolConfig.InitPool.ContainsKey(prefab))
            {
                pool = new BasePool(prefab, PoolConfig.InitPool[prefab]);
            }
            else
            {
                pool = new BasePool(prefab, PoolConfig.DefaultInitPoolGO);
            }
            objectPoolDict.TryAdd(prefab, pool);
        }
        else
        {
            pool = objectPoolDict[prefab];
        }
        obj = pool.GetItem<T>();
        obj.transform.parent = Instance.transform;
        obj.transform.position = pos;
        return obj;
    }
}
