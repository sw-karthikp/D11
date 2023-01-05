using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItems : MonoBehaviour
{

    public bool isEnabled;
    string poolId;
    PoolManager poolmanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPool(PoolManager poolManager,string poolID)
    {
        poolId = poolID;
        poolmanager = poolManager;
    }

    public void AddToPool()
    {
        poolmanager.SetPoolObject(poolId, this);
    }


    private void OnEnable()
    {
        isEnabled = true;
    }

    private void OnDisable()
    {
        AddToPool();
        isEnabled = false;
    }
}
