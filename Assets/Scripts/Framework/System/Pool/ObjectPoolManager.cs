using Framework;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spawn object from a pool
/// If root is specified, attach object to root else attach object to default pool-generated root
/// If first time spawn object, create new pool
/// </summary>
public class ObjectPoolManager : SingletonMono<ObjectPoolManager>
{
    static Dictionary<GameObject, BasePool> objectPoolDict = new Dictionary<GameObject, BasePool>();
    [SerializeField] List<GameObject> beforeLoadObject;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < beforeLoadObject.Count; i++)
        {
            SpawnObject<Component>(beforeLoadObject[i]).gameObject.SetActive(false);
        }
    }
    /// <typeparam name="T"></typeparam>
    /// <param name="prefab"> Object need to generate</param>
    /// <param name="root"> Parent of pool </param>
    /// <param name="amount"> Number of object init </param>
    public static T SpawnObject<T>(GameObject prefab, Transform root = null, int? amount = null) where T : Component
    {
        BasePool pool;
        T obj;

        //if pool not exist, create pool
        if (!objectPoolDict.ContainsKey(prefab))
        {
            if (root == null)
            {
                GameObject @object = new GameObject();
                root = @object.transform;
                root.transform.parent = Instance.transform;
                root.name = typeof(T).ToString() + " Pool " + prefab.name;
            }
            if (PoolConfig.InitPool.ContainsKey(prefab))
            {
                pool = new BasePool(prefab, PoolConfig.InitPool[prefab], root);
            }
            else
            {
                if (amount == null)
                {
                    amount = PoolConfig.DefaultInitPoolGO;
                }
                pool = new BasePool(prefab, (int)amount, root);
            }
            objectPoolDict.TryAdd(prefab, pool);
        }
        else
        {
            pool = objectPoolDict[prefab];
        }
        obj = pool.GetItem<T>(root);
        return obj;
    }
    /// <typeparam name="T"></typeparam>
    /// <param name="prefab">Object need to generate</param> 
    /// <param name="pos">Position</param>
    /// <param name="root">Parent of pool</param>
    public static T SpawnObject<T>(GameObject prefab, Vector3 pos, Transform root = null) where T : Component
    {
        BasePool pool;
        T obj;
        //if pool not exist, create pool
        if (!objectPoolDict.ContainsKey(prefab))
        {
            if (root == null)
            {
                GameObject @object = new GameObject();
                root = @object.transform;
                root.transform.parent = Instance.transform;
                root.name = typeof(T).ToString() + " Pool";
            }
            if (PoolConfig.InitPool.ContainsKey(prefab))
            {
                pool = new BasePool(prefab, PoolConfig.InitPool[prefab], root);
            }
            else
            {
                pool = new BasePool(prefab, PoolConfig.DefaultInitPoolGO, root);
            }
            objectPoolDict.TryAdd(prefab, pool);
        }
        else
        {
            pool = objectPoolDict[prefab];
        }
        obj = pool.GetItem<T>(root);
        obj.transform.position = pos;
        return obj;
    }
}
