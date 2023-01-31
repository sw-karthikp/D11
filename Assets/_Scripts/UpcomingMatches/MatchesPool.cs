using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static GameController;
using System.Linq;
using System;

public class MatchesPool : MonoBehaviour
{

    public GameObject prefab;
    public Transform parent;
    public TMP_Text pooltype;

    public void SetValueToObject(Dictionary<string,Pools> pools,string _PoolType)
    {
        pooltype.text = _PoolType;

        foreach (var item in pools.Values)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PoolType");
            mprefabObj.transform.SetParent(parent);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.name = item.Type;
            bool check = false;
            try
            {
                SelectdPoolID intractable = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools.Values.First(x => x.PoolID == item.PoolID.ToString());
            }
            catch(Exception e)
            {
                check = true;
            }
            
            mprefabObj.GetComponent<MatchPoolType>().SetValueToPoolObject(item.Entry, item.PoolID, item.PrizeList, item.LeaderBoard, item.PrizePool, item.SlotsFilled, item.TotalSlots, item.Type, item, check);
        }
    }
}
