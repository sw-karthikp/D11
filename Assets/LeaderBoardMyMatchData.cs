using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class LeaderBoardMyMatchData : MonoBehaviour
{
    public TMP_Text playerCount;
    public Transform Parent;
    string _name;
    string _value;
    private void OnEnable()
    {
        foreach (var item in GameController.Instance.matchpool)
        {
            if(GameController.Instance.CurrentMatchID == item.Value.MatchID)
            {
                foreach (var item1 in item.Value.Pools.Values)
                {
                   if(GameController.Instance.CurrentPoolID == item1.PoolID.ToString())
                    {
                        playerCount.text = $"All Teams ({item1.LeaderBoard.Count})";
                        foreach (var item2 in item1.LeaderBoard)
                        {
                            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("LeaderBoard");
                            mprefabObj.transform.SetParent(Parent);
                            mprefabObj.gameObject.SetActive(true);

                            foreach (var item3 in item2.Value)
                            {
                                if (item3.Key == "Name")
                                {
                                    _name = item3.Value;
                                }
                                if (item3.Key == "Value")
                                {
                                    _value = item3.Value;
                                }
                                mprefabObj.gameObject.name = item3.Value;
                                mprefabObj.gameObject.GetComponent<LeaderBoardMyData>().SetData(_name, 1, _value, "",item2.Key);
                                Canvas.ForceUpdateCanvases();


                            }

                        }
                    }

                   
                }
            }
           
        }
    }
}
