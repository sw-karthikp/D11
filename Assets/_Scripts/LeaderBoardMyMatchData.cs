using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class LeaderBoardMyMatchData : MonoBehaviour
{
    public TMP_Text playerCount;
    public Transform Parent;
    string _name;
    string _value;
    // public Dictionary<string, string> value = new();


    void RefreshData()
    {
        Pools value;
        try
        {

            value = GameController.Instance.matchpool.First(X => X.Value.MatchID == GameController.Instance.CurrentMatchID).Value.Pools.Values.First(x => x.PoolID == GameController.Instance.CurrentPoolID);
        }
        catch(Exception e)
        {
            return;
        }

        foreach (Transform item in Parent)
        {
            item.gameObject.SetActive(false);
        }

        List<Dictionary<string, string>> leaderData = new();


        foreach (var item2 in value.LeaderBoard)
        {

            if (!string.IsNullOrWhiteSpace(item2.Value["Value"]))
            {
                Dictionary<string, string> val1 = new Dictionary<string, string>(){
                    { "Name" , item2.Value["Name"] },
                    { "Value" , item2.Value["Value"]}
                };


                leaderData.Add(val1);
                continue;
            }
            else
            {
                return;
            }
            
        }


        var list= leaderData.OrderByDescending(x => float.Parse(x["Value"]));
        Debug.Log(list.Count());
        Dictionary<int, Dictionary<string, string>> val = new();
        int ind = 0;
        foreach (var item in list)
        {
            ind += 1;
            val.Add(ind, new Dictionary<string, string>() { { "Name", item["Name"] },{ "Value", item["Value"] } });
        }
        foreach (var item in val)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("LeaderBoard");
            mprefabObj.transform.SetParent(Parent);
            mprefabObj.gameObject.SetActive(true);             
            _name = item.Value["Name"];
            _value = item.Value["Value"];
            mprefabObj.gameObject.GetComponent<LeaderBoardMyData>().SetData(_name, 1, _value, item.Key.ToString(), "");
            Canvas.ForceUpdateCanvases();
        }
    }

    private void OnDisable()
    {
        GameController.Instance.OnMatchPoolChanged -= RefreshData;
    }

    private void OnEnable()
    {
        GameController.Instance.OnMatchPoolChanged += RefreshData;
        RefreshData();
    }
}
