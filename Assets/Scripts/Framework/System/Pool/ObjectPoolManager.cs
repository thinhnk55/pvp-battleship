using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;
/// <summary>
/// Spawn object from a pool
/// If root is specified, attach object to root else attach object to default pool-generated root
/// If first time spawn object, create new pool
/// </summary>
public class ObjectPoolManager : SingletonMono<ObjectPoolManager>
{
    static Dictionary<GameObject, BasePool> objectPoolDict = new Dictionary<GameObject, BasePool>();
    [SerializeField] List<GameObject> beforeLoadObject;
    [SerializeField] Canvas canvasRoot;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < beforeLoadObject.Count; i++)
        {
            SpawnObject<Component>(beforeLoadObject[i], Vector3.zero).gameObject.SetActive(false);
        }
    }
    /// <typeparam name="T"></typeparam>
    /// <param name="prefab">Object need to generate</param> 
    /// <param name="pos">Position</param>
    /// <param name="root">Parent of pool</param>
    public static T SpawnObject<T>(GameObject prefab, Vector3 pos,Transform root = null, bool isUI = false, int? quantity = null) where T : Component
    {
        if (isUI)
            SetCanvas();
        BasePool pool;
        if (!objectPoolDict.ContainsKey(prefab))
        {
            pool = RetrievePool<T>(prefab, root, isUI, quantity);
        }
        else
        {
            pool = objectPoolDict[prefab];
        }
        T obj = pool.GetItem<T>(root);
        obj.transform.position = pos;
        return obj;
    }

    static void SetCanvas()
    {
        if (!Instance.canvasRoot.worldCamera)
        {
            Instance.canvasRoot.planeDistance = 10;
            Instance.canvasRoot.worldCamera = Camera.main;
            //PCoroutine.PStartCoroutine(SetCanvas());
        }
    }

    static BasePool RetrievePool<T>(GameObject prefab ,Transform root, bool isUI = false, int? quatity = null)
    {
        BasePool pool;
        if (root == null)
        {
            GameObject @object = new GameObject(typeof(T).ToString() + " Pool");
            root = @object.transform;
            if (!isUI)
            {
                root.SetParent(Instance.transform);
            }
            else
            {
                root = root.gameObject.AddComponent<RectTransform>();
                root.SetParent(Instance.canvasRoot.transform);
                root.transform.localScale = Vector3.one;
                root.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            }

        }
        if (quatity != null)
        {
            pool = new BasePool(prefab, quatity.Value, root);
        }
        else if (PoolConfig.InitPool.ContainsKey(prefab))
        {
            pool = new BasePool(prefab, PoolConfig.InitPool[prefab], root);
        }
        else
        {
            pool = new BasePool(prefab, PoolConfig.DefaultInitPoolGO, root);
        }
        objectPoolDict.TryAdd(prefab, pool);
        return pool;
    }
}

