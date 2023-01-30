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
    public static MyMatchContests Instance;
    public string totalSlots;
    private void Awake()
    {
        Instance= this;
    }

    private void OnEnable()
    {
        FecthData();
    }

    public void FecthData()
    {
        foreach (var item in GameController.Instance.selectedMatches)
        {
            if(item.Key == GameController.Instance.CurrentMatchID)
            {
                foreach (var item1 in item.Value.SelectedPools.Values)
                {
                    foreach (var item2 in GameController.Instance.matchpool.Values)
                    {
                        if (item.Key == item2.MatchID.ToString())
                        {
                            foreach (var item3 in item2.Pools.Values)
                            {
                                if (item1.PoolID == item3.PoolID.ToString())
                                {
                                    poolTypeName = item3.Type;
                                    spots = item3.SlotsFilled.ToString();
                                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchContest");
                                    mprefabObj.transform.SetParent(parent);
                                    mprefabObj.gameObject.SetActive(true);
                                    mprefabObj.name = item3.PoolID.ToString();
                                    mprefabObj.GetComponent<MyContest>().SetDataToMyContest(poolTypeName, spots, totalSlots, teamName, teamCount , teamName ,item3.PoolID);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
