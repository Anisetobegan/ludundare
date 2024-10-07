using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //Dictionary of different pools of objects to instantiate
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    [System.Serializable]
    public class Pool //Class to set different type of pools to instantiate (set in inspector)
    {
        public string tag; //Tag to identify the specific type of pool we want
        public List<GameObject> prefabs; //List to store the Prefabs we want to instantiate
        public int size; //Size of the objects to spawn for each pool
    }

    public List<Pool> pools; //List for different types of pools set

    public static ObjectPool Instance
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

    void Start()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                int rng = Random.Range(0, pool.prefabs.Count);

                GameObject obj = Instantiate(pool.prefabs[rng]);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " +  tag + " doesnLt exist");
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}
