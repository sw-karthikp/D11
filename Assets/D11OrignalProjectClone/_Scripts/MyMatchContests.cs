using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MyMatchContests : MonoBehaviour
{
    public Transform parent;
    public string poolTypeName;
    public string spots;
    public string teamName;
    public string teamCount;
    public string rank;
    public static MyMatchContests Instance;
    public string totalSlots;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        FecthData();
    }

    private void OnDisable()
    {
        foreach (Transform item in parent)
        {
            item.gameObject.SetActive(false);
        }
    }


    public void FecthData()
    {

        foreach (Transform item in parent)
        {
            item.gameObject.SetActive(false);
        }
        //string poolsIndex ="";
        //try
        //{
        //    poolsIndex = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools.First().Value.PoolID;
        //}
        //catch(Exception e)
        //{
        //    return;
        //}
        Dictionary<string, Pools> pools = new();
        try
        {
            pools = GameController.Instance.matchpool.First(x => x.Value.MatchID == GameController.Instance.CurrentMatchID).Value.Pools;
        }
        catch(Exception e)
        {
            return;
        }
        
        foreach (var item in pools)
        {
            SelectdPoolID pool;
            try
            {
                pool = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools.Values.First(x => x.PoolID == item.Value.PoolID);
            }
            catch (Exception e)
            {
                continue;
            }

            poolTypeName = item.Value.Type;
            spots = item.Value.SlotsFilled + " spots";
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchContest");
            mprefabObj.transform.SetParent(parent);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.name = item.Value.PoolID.ToString();

            mprefabObj.GetComponent<MyContest>().SetDataToMyContest(poolTypeName, spots, item.Value.TotalSlots.ToString(), pool.TeamID, "1", "TEAM", item.Value.PoolID);
        }
    }


    //foreach (var item in GameController.Instance.selectedMatches)
    //{
    //    if(item.Key == GameController.Instance.CurrentMatchID)
    //    {
    //        foreach (var item1 in item.Value.SelectedPools.Values)
    //        {
    //            foreach (var item2 in GameController.Instance.matchpool.Values)
    //            {
    //                if (item.Key == item2.MatchID.ToString())
    //                {
    //                    foreach (var item3 in item2.Pools.Values)
    //                    {
    //                        if (item1.PoolID == item3.PoolID.ToString())
    //                        {

    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}

