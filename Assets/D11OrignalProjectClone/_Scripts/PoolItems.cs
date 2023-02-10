using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PoolItems : MonoBehaviour
{

    public bool isEnabled;
    string poolId;
    public PoolManager poolmanager;
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
      //  if (poolId == null) Debug.LogWarning(gameObject.name);
        poolId = poolID;
        poolmanager = poolManager;
    }

    public void AddToPool()
    {
        if (poolmanager == null) Debug.Log("<color = green>"+gameObject.name+"</color>");

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
