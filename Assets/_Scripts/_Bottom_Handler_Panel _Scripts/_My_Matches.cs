using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class _My_Matches : UIHandler
{

    public static _My_Matches Instance;

    [Header("ToggleHolder")]
    public Toggle[] toggles;

    [Header("ObjectsToEnable")]
    public GameObject[] objects;
    
    [Header("Variables")]
    public TMP_Text liveStatus;
    public TMP_Text headerText;
    public TMP_Text teamFullNameA;
    public TMP_Text teamFullNameB;
    public TMP_Text teamCount;
    public TMP_Text contestCount;
    public Image liveStatusColor;

    [Header("Datas")]
    public string TeamA;
    public string TeamB;

    
    public string matchID;

    private void Awake()
    {
        Instance= this;
        toggles[0].onValueChanged.AddListener(delegate { OnValueChange(0); });
        toggles[1].onValueChanged.AddListener(delegate { OnValueChange(1); });
        toggles[2].onValueChanged.AddListener(delegate { OnValueChange(2); });
    }

    public override void ShowMe()
    {
        gameObject.SetActive(true);
        toggles[0].isOn= true;
    }
    public override void HideMe()
    {
        GameController.Instance.UnSubscribeLiveScoreDetails(matchID);
        gameObject.SetActive(false);
    }
    public override void OnBack()
    {
       
    }

    public void OnValueChange(int _index)
    {
        objects[_index].SetActive(toggles[_index].isOn ? true:false);       
    }

    public void SetDataToMyMatches(string _teamA,string _teamB,string _teamAFullName,string _teamBFullName ,string _id)
    {
        TeamA = _teamA;
        TeamB = _teamB;
        headerText.text = _teamA + " vs " + _teamB;
        matchID= _id;
        teamFullNameA.text = _teamAFullName;
        teamFullNameB.text = _teamBFullName;
        contestCount.text = GameController.Instance.selectedMatches.Count > 0 ? $"My Contests ({ReturnContestCount()})" : "My Contests";
       
    }

   
    public string ReturnContestCount()
    {
        int count = 0;
        foreach (var item in GameController.Instance.selectedMatches)
        {
            if (item.Key == GameController.Instance.CurrentMatchID.ToString())
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
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
        }
        return count.ToString();
    }

}
