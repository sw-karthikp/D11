using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
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


    private void OnEnable()
    {

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
        MatchIDVal = MatchId.ToString();
        TeamA = teamA;
        TeamB = teamB;
        GameController.Instance.CurrentTeamA = teamA;
        GameController.Instance.CurrentTeamB = teamB;
        GameController.Instance.CurrentMatchID = MatchId;
        GameController.Instance.CurrentMatchTimeDuration = _timeduration;
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

        foreach (var item in GameController.Instance.matchpool.Values)
        {
            Debug.Log("called *****************1");
            if (item.MatchID == GameController.Instance.CurrentMatchID)
            {
                Debug.Log("called *****************2");
                foreach (var item1 in item.Pools.Values)
                {
                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MatchPools");
                    mprefabObj.transform.SetParent(parent);
                    mprefabObj.gameObject.SetActive(true);
                    mprefabObj.name = item1.PoolID.ToString();
                    mprefabObj.GetComponent<MatchesPool>().SetValueToObject(item1.Entry, item1.PoolID, item1.PrizeList,item1.LeaderBoard, item1.PrizePool, item1.SlotsFilled, item1.TotalSlots, item1.Type);
                    Canvas.ForceUpdateCanvases();
                }
               
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent.transform as RectTransform);
        }
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