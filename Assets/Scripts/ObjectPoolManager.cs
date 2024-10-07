using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    Dictionary<string, PooledObject<MonoBehaviour>> pools = new Dictionary<string, PooledObject<MonoBehaviour>>();

    public static ObjectPoolManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreatePool<T>(T prefab) where T : MonoBehaviour
    {
        if (pools.ContainsKey(prefab.GetType().ToString()))
            return;
        
        PooledObject<MonoBehaviour> objToPool = new PooledObject<MonoBehaviour>();
        objToPool.prefab = prefab;
        objToPool.activeInstances = new List<MonoBehaviour>();
        objToPool.instancesInPool = new List<MonoBehaviour>();

        pools.Add(prefab.GetType().ToString(), objToPool);
    }

    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        string typeOf = prefab.GetType().ToString();

        CreatePool<T>(prefab);
        PooledObject<MonoBehaviour> objToPool = pools[typeOf];
        return (T)pools[typeOf].GetOrCreate();
    }

    public void AddToPool<T>(T toAdd) where T : MonoBehaviour
    {
        string typeOf = typeof(T).ToString();
        toAdd.gameObject.SetActive(false);
        pools[typeOf].AddToPool(toAdd);
    }
}
