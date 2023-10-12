using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ObjectPool : MonoBehaviour
    {
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;

        void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, transform);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            return null;
        }
    }
}