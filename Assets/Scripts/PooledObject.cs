using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PooledObject<T> where T : MonoBehaviour
{
    public T prefab;
    public List<T> activeInstances;
    public List<T> instancesInPool;
    public T GetOrCreate()
    {
        T newObject = null;
        if (instancesInPool.Count == 0)
        {
            //Create object and put it in instancesInPool List
            newObject = GameObject.Instantiate(prefab);
            activeInstances.Add(newObject);
            return newObject;
        }
        else
        {
            activeInstances.Add(instancesInPool[0]);
            int index = activeInstances.IndexOf(instancesInPool[0]);            
            instancesInPool.RemoveAt(0);
            activeInstances[index].gameObject.SetActive(true);

            return activeInstances[index];
        }
    }
    public void AddToPool(T toAdd)
    {
        if (activeInstances.Contains(toAdd))
        {
            instancesInPool.Add(toAdd);
            activeInstances.Remove(toAdd);
            
        }
        else
            instancesInPool.Add(toAdd);
    }
}
