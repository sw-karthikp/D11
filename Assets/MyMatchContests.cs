using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class MyMatchContests : MonoBehaviour
{
    public Transform parent;
    public string poolTypeName;
    public string spots;
    public string teamName;
    public string teamCount;
    private void OnEnable()
    {
        FecthData();
    }

    public void FecthData()
    {
        foreach (var item in GameController.Instance.selectedMatches.Values)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchContest");
            mprefabObj.transform.SetParent(parent);
            mprefabObj.gameObject.SetActive(true);

            foreach (var item1 in item.SelectedPools.Values)
            {
                teamName = item1.TeamID;
                foreach (var item2 in GameController.Instance.matchpool.Values)
                {
                    foreach (var item3 in item2.Pools.Values)
                    {
                        if (item1.PoolID == item3.PoolID.ToString())
                        {
                            poolTypeName = item3.Type;
                            spots = item3.SlotsFilled.ToString();
                            
                        }
                    }
                }
                mprefabObj.GetComponent<MyContest>().SetDataToMyContest(poolTypeName, spots,teamName,teamCount);
            }
        }
    }
}
