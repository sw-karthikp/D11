using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyContestNEW : MonoBehaviour
{
    public Transform parent;
    public string poolTypeName;
    public string spots;
    public string teamName;
    public string teamCount;
    public static MyContestNEW Instance;
    public string totalSlots;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        FecthData();
    }

    public void FecthData()
    {
        foreach (var item in GameController.Instance.selectedMatches)
        {
            if (item.Key == GameController.Instance.CurrentMatchID)
            {
                foreach (var item1 in item.Value.SelectedPools.Values)
                {
                    teamName = item1.TeamID; teamCount = item1.TeamID;
                    foreach (var item2 in GameController.Instance.matchpool.Values)
                    {
                        if (item.Key == item2.MatchID.ToString())
                        {
                            foreach (var item3 in item2.Pools.Values)
                            {
                                if (item1.PoolID == item3.PoolID.ToString())
                                {
                                    poolTypeName = item3.Type;
                                    totalSlots = item3.TotalSlots.ToString();
                                    spots = item3.SlotsFilled.ToString();
                                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("ContestNEW");
                                    mprefabObj.transform.SetParent(parent);
                                    mprefabObj.gameObject.SetActive(true);
                                    mprefabObj.gameObject.name = item3.PoolID.ToString();
                                    mprefabObj.GetComponent<MyContest>().SetDataToMyContestNEW(poolTypeName, spots, totalSlots, teamName, teamCount , teamName, item3.PoolID);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
