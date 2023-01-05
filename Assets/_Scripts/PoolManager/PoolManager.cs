using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Transform poolParent;

    public List<PoolObjects> poolObjects;

    Dictionary<string,Queue<PoolItems>> pooledObjects = new Dictionary<string,Queue<PoolItems>>();

    Dictionary<string, PoolItems> objectsToCreate= new Dictionary<string, PoolItems>();


    void Start()
    {
        foreach (var item in poolObjects)
        {
            objectsToCreate.Add(item.poolName,item.pool);
            for (int i = 0; i < item.poolCount; i++)
            {
                CreatePoolObject(item.poolName);
            }
        }
    }

    public PoolItems GetPoolObject(string poolID)
    {
        if(pooledObjects[poolID].Count > 0)
        {
            PoolItems obj = pooledObjects[poolID].Dequeue();
            return obj;
        }
        else
        {
            CreatePoolObject(poolID);
            return GetPoolObject(poolID);
        }
    }
    
    public void SetPoolObject(string poolID, PoolItems objToAdd)
    {
        pooledObjects[poolID].Enqueue(objToAdd);
    }


    void CreatePoolObject(string poolId)
    {
        GameObject obj = Instantiate(objectsToCreate[poolId].gameObject,poolParent);
        obj.gameObject.SetActive(false);
        obj.GetComponent<PoolItems>().SetPool(this, poolId);
        if (pooledObjects.ContainsKey(poolId))
            pooledObjects[poolId].Enqueue(obj.GetComponent<PoolItems>());
        else
        {
            Queue<PoolItems> queue = new Queue<PoolItems>();
            queue.Enqueue(obj.GetComponent<PoolItems>());
            pooledObjects.Add(poolId, queue);
        }

    }
}

[System.Serializable]
public class PoolObjects
{
    public int poolCount;
    public PoolItems pool;
    public string poolName;
}