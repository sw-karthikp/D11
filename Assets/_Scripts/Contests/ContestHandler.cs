using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ContestHandler : UIHandler
{
    public static ContestHandler Instance;

    [Header("Variables")]
    public GameObject PrizePoolChild;
    public GameObject ViewallChild;
    public Transform parent;
    public Dictionary<string, List<string>> val = new Dictionary<string, List<string>>();
    public GameObject viewAllPoolMatches;
    public GameObject viewTypePoolMatches;
    public string TeamA;
    public string TeamB;
    public string MatchIDVal;
    public TMP_Text contestCount;
    public TMP_Text teamCount;
    public TMP_Text contestText;


    [Header("ToggleHolder")]
    public Toggle[] toggles;

    [Header("ObjectsToEnable")]
    public GameObject[] objects;


    private void Awake()
    {
        Instance = this;
        toggles[0].onValueChanged.AddListener(delegate { OnValueChange(0); OnenableToggleForContest(); });
        toggles[1].onValueChanged.AddListener(delegate { OnValueChange(1); });
        toggles[2].onValueChanged.AddListener(delegate { OnValueChange(2); });
    }

    public void OnValueChange(int _index)
    {
        objects[_index].SetActive(toggles[_index].isOn ? true : false);
    }


    

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
        GameController.Instance.CurrentMatchID = "";
    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        toggles[0].isOn = true;
    }

    public override void OnBack()
    {

    }

    private void OnDisable()
    {
        GameController.Instance.OnMatchPoolChanged -= OnenableToggleForContest;
    }

    private void OnEnable()
    {
        GameController.Instance.OnMatchPoolChanged += OnenableToggleForContest;
        viewAllPoolMatches.SetActive(false);
        viewTypePoolMatches.SetActive(true);
    }

    public void OnClickClose()
    {
        UIController.Instance.MainMenuScreen.ShowMe();
        if (UIController.Instance.WinnerLeaderBoard.gameObject.activeSelf)
        {
            UIController.Instance.WinnerLeaderBoard.HideMe();
            return;
        }
        UIController.Instance.ContestPanel.HideMe();
    }

 


    public IEnumerator SetUpcomingMatchPoolDetails(string MatchId, string teamA, string teamB, string _timeduration)
    {
        //yield return new WaitForSeconds(Time.deltaTime);
        MatchIDVal = MatchId.ToString();
        TeamA = teamA;
        TeamB = teamB;
        GameController.Instance.CurrentTeamA = teamA;
        GameController.Instance.CurrentTeamB = teamB;
        GameController.Instance.CurrentMatchID = MatchId;
        GameController.Instance.CurrentMatchTimeDuration = _timeduration;
        contestText.text = $"{TeamA} vs {TeamB}\n{_timeduration}";
        val.Clear();
        OnenableToggleForContest();
        DebugHelper.Log("******************" + MatchId);
        yield return null;
         
    }
   

    public void OnenableToggleForContest()
    {
        
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }

        Dictionary < string,Dictionary<string, Pools>> currentPools = new();

        Debug.Log("CurrentpooolCount");

        foreach (var item in GameController.Instance.matchpool.Values)
        {
            Debug.Log("called *****************1" + item.MatchID);
            if (item.MatchID == GameController.Instance.CurrentMatchID)
            {
                Debug.Log("called *****************2");
                foreach (var item1 in item.Pools)
                {
                    if(currentPools.ContainsKey(item1.Value.Type))
                    {
                        currentPools[item1.Value.Type].Add(item1.Key, item1.Value) ;
                    }
                    else
                    {
                        currentPools.Add(item1.Value.Type, new Dictionary<string, Pools>() { { item1.Key, item1.Value } });
                    }
                }               
            }            
        }

        Debug.Log("CurrentpooolCount" + currentPools.Count);


        foreach (var item in currentPools)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MatchPools");
            mprefabObj.transform.SetParent(parent);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.name = item.Key;
            //Debug.Log(item1.Value.SlotsFilled + "^^^^^" + item1.Value.TotalSlots);
            //mprefabObj.GetComponent<MatchesPool>().SetValueToObject(item1.Value.Entry, item1.Value.PoolID, item1.Value.PrizeList,item1.Value.LeaderBoard, item1.Value.PrizePool, item1.Value.SlotsFilled, item1.Value.TotalSlots, item1.Value.Type,item1.Value);
            mprefabObj.GetComponent<MatchesPool>().SetValueToObject(item.Value,item.Key);
            Canvas.ForceUpdateCanvases();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.transform as RectTransform);
        contestCount.text = GameController.Instance.selectedMatches.Count > 0 ? $"My Contests ({ReturnContestCount()})" : "My Contests";
        teamCount.text = GameController.Instance.selectedMatches.Count > 0 ? $"My Teams ({ReturnTeamCount()})" : "My Teams";


    }

    public string  ReturnTeamCount()
    {
        int count = 0;
        count = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedTeam.Count;
        return count.ToString();
    }
    public string ReturnContestCount()
    {
        int count = 0;

        try
        {
            count = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools.Count;
            
        }
        catch (Exception e)
        {

        }


        //foreach (var item in GameController.Instance.selectedMatches)
        //{
        //    if (item.Key == GameController.Instance.CurrentMatchID.ToString())
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
        //                            count++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        return count.ToString();
    }

}