using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MyMatches : UIHandler
{

    public Toggle[] toggles;
    public GameObject[] contentsToDisplay;
    public ScrollRect rect;
    public GameObject prefab;
    public Dictionary<string, Dictionary<string, MatchStatus>> mymatches = new Dictionary<string, Dictionary<string, MatchStatus>>();
    public GameObject[] empty;

    public void Awake()
    {
        toggles[0].onValueChanged.AddListener(delegate { OnvalueChnaged(0); });
        toggles[1].onValueChanged.AddListener(delegate { OnvalueChnaged(1); });
        toggles[2].onValueChanged.AddListener(delegate { OnvalueChnaged(2); });
    }

    public override void ShowMe()
    {
        gameObject.SetActive(true);
        toggles[0].isOn = true;
    }
    public override void HideMe()
    {
        gameObject.SetActive(false);
    }
    public override void OnBack()
    {

    }

    public void OnvalueChnaged(int _index)
    {

        if (toggles[_index].isOn)
        {
            rect.content = contentsToDisplay[_index].GetComponent<RectTransform>();
            contentsToDisplay[_index].SetActive(true);
            ShowDataOnValueChange();
        }
        else
        {
            contentsToDisplay[_index].SetActive(false);
            
        }
    }


    public void UpdateData()
    {
        mymatches.Clear();

        foreach (var item in GameController.Instance.selectedMatches)
        {

            foreach (var item2 in GameController.Instance.match)
            {
                foreach (var item3 in item2.Value)
                {
                    if (item3.Value.ID.ToString() == item.Key)
                    {
                        if (mymatches.ContainsKey(item2.Key))
                        {
                            mymatches[item2.Key].Add(item3.Key, item3.Value);
                        }
                        else
                        {
                            mymatches.Add(item2.Key, new Dictionary<string, MatchStatus>() { { item3.Key, item3.Value } });
                        }

                    }
                }
            }
            //GameController.Instance.SubscribeLiveScoreDetails(item.Key);
        }
    }

    public void ShowDataOnValueChange()
    {


        if (!mymatches.ContainsKey("Complete"))
        {
            empty[2].SetActive(true);
        }
        else
        {
            SetDetailsToPrefabInstans(2, MatchTypeStatus.Complete.ToString());
        }

        if (!mymatches.ContainsKey("Live"))
        {
            empty[1].SetActive(true);
        }
        else
        {
            SetDetailsToPrefabInstans(1, MatchTypeStatus.Live.ToString());
        }

        if (!mymatches.ContainsKey("Upcoming"))
        {
            empty[0].SetActive(true);
        }
        else
        {
            SetDetailsToPrefabInstans(0, MatchTypeStatus.Upcoming.ToString());
        }


    }


    void SetDetailsToPrefabInstans(int _index, string keyval)
    {
        empty[_index].SetActive(false);
        foreach (Transform child in contentsToDisplay[_index].transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (var item in mymatches[keyval])
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchData");
            mprefabObj.transform.SetParent(contentsToDisplay[_index].transform);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.name = "";
            mprefabObj.GetComponent<MyMatchData>().SetDetails(item.Value.TeamA, item.Value.TeamB, item.Value.ID.ToString(), item.Value.Time, "ICC MENS CRICKET", _index);
        }

    }
}
