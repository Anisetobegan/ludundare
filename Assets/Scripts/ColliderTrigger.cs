using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    List<GameObject> objectsCollided;
    [SerializeField] LayerMask layer;


    private void OnTriggerEnter(Collider other)
    {
        if (objectsCollided.Contains(other.gameObject) == false)
        {
            if (layer == (layer | (1 << other.gameObject.layer)) && other.gameObject.activeInHierarchy)
            {                
                objectsCollided.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsCollided.Remove(other.gameObject);
    }

    public void SetList(List<GameObject> listToGet)
    {
        objectsCollided = listToGet;
    }
}
