
    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AddNewPlayerHandler;

public class LeaderBoardMyMatchDisplay : UIHandler
{

    public static LeaderBoardMyMatchDisplay Instance;

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
    public TMP_Text MainScoreA;
    public TMP_Text MainScoreB;
    [Header("Datas")]
    public string TeamA;
    public string TeamB;


    public string matchID;

    private void Awake()
    {
        Instance = this;
        toggles[0].onValueChanged.AddListener(delegate { OnValueChange(0); });
        toggles[1].onValueChanged.AddListener(delegate { OnValueChange(1); });
        toggles[2].onValueChanged.AddListener(delegate { OnValueChange(2); });
        toggles[3].onValueChanged.AddListener(delegate { OnValueChange(3); });
    }

    public override void ShowMe()
    {
        gameObject.SetActive(true);

        foreach (var item in toggles)
        {
            item.isOn = false;
        }

        toggles[1].isOn = true;
        StartCoroutine(delay());
       
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.2f);
        if (MyMatchContests.Instance.parent.childCount == 0)
        {
            MyMatchContests.Instance.FecthData();
        }

    }

    private void OnEnable()
    {
        Total1();
        Total2();
    }
    public override void HideMe()
    {
       // GameController.Instance.UnSubscribeLiveScoreDetails(matchID);
        gameObject.SetActive(false);
       // GameController.Instance.CurrentMatchID = "";
    }
    public override void OnBack()
    {

    }

    public void OnValueChange(int _index)
    {
        objects[_index].SetActive(toggles[_index].isOn ? true : false);
    }
    public void Total1()
    {

        foreach (var item in GameController.Instance.scoreCard.MatchDetails)
        {
            if (item.Key == "Innings1")
            {
                MainScoreA.text = item.Value.InningsRuns.ToString() + "/" + item.Value.InningsWickets.ToString() + " " + "(" + item.Value.InningsOvers.ToString() + ")";

            }

        }
    }

    public void Total2()
    {
        foreach (var item in GameController.Instance.scoreCard.MatchDetails)
        {
            if (item.Key == "Innings2")
            {
                MainScoreB.text = item.Value.InningsRuns.ToString() + "/" + item.Value.InningsWickets.ToString() + " " + "(" + item.Value.InningsOvers.ToString() + ")";

            }

        }

    }
    public void SetDataToMyMatches(string _teamA, string _teamB, string _teamAFullName, string _teamBFullName, string _id, string time)
    {
        GameController.Instance.CurrentTeamA = _teamA;
        GameController.Instance.CurrentTeamB = _teamB;
        GameController.Instance.CurrentMatchID = _id;
        GameController.Instance.CurrentMatchTimeDuration = time;
        TeamA = _teamA;
        TeamB = _teamB;
        headerText.text = _teamA + " vs " + _teamB;
        matchID = _id;
        foreach (var item in GameController.Instance.match)
        {
            foreach (var item1 in item.Value)
            {
                if (item1.Value.ID == matchID)
                {
                    if (item.Key == MatchTypeStatus.Live.ToString())
                    {
                        liveStatus.text = "Live";

                        liveStatusColor.color = new Color(1, 0, 0, 1);
                    }

                    if (item.Key == MatchTypeStatus.Complete.ToString())
                    {
                        liveStatus.text = "Completed";

                        liveStatusColor.color = new Color(0, 1, 0, 1);
                    }
                }
            }
        }

        foreach (var item in GameController.Instance.countryFullName)
        {
            if (item.Key == _teamA)
            {
                teamFullNameA.text = item.Value;
            }
            if (item.Key == _teamB)
            {
                teamFullNameB.text = item.Value;
            }
        }

        contestCount.text = GameController.Instance.selectedMatches.Count > 0 ? $"My Contests ({ReturnContestCount()})" : "My Contests";

    }


    public string ReturnContestCount()
    {
        int count = 0;

        //string key = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID.ToString()].SelectedPools.First(x =>x.Value.PoolID ==GameController.Instance.CurrentPoolID).Key
        //count = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID.ToString()].SelectedPools[k
        Debug.Log("Setting Date");
        foreach (var item in GameController.Instance.selectedMatches)
        {
            if (item.Key == GameController.Instance.CurrentMatchID.ToString())
            {
                foreach (var item1 in item.Value.SelectedPools.Values)
                {

                    count++;
                }
            }
        }
        return count.ToString();
    }


}
