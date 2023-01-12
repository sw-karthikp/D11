using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameController;

public class ContestHandler : UIHandler
{
    public static ContestHandler Instance;
    public GameObject PrizePoolChild;
    public GameObject ViewallChild;
    public Transform parent;
    public Dictionary<string, List<string>> val = new Dictionary<string, List<string>>();
    public GameObject viewAllPoolMatches;
    public GameObject viewTypePoolMatches;
    public VerticalLayoutGroup rect;
    public string TeamA;
    public string TeamB;
    public string MatchIDVal;

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

        foreach (Transform Child in parent)
        {
            Destroy(Child.gameObject);
        }
        foreach (Transform Child in viewAllPoolMatches.transform)
        {
            Destroy(Child.gameObject);
        }
    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {

        viewAllPoolMatches.SetActive(false);
        viewTypePoolMatches.SetActive(true);
        rect.reverseArrangement = true;
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


    public IEnumerator SetUpcomingMatchPoolDetails(int MatchId, string teamA, string teamB, string _timeduration)
    {

        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }

        MatchIDVal = MatchId.ToString();
        TeamA = teamA;
        TeamB = teamB;
        GameController.Instance.CurrentTeamA = teamA;
        GameController.Instance.CurrentTeamB = teamB;
        GameController.Instance.CurrentMatchID = MatchId;
        GameController.Instance.CurrentMatchTimeDuration = _timeduration;
        val.Clear();
        Debug.Log("******************" + MatchId);


        foreach (var item in GameController.Instance.matchpool.Values)
        {
            foreach (var item1 in item.Pools.Values)
            {

                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MatchPools", false);
                mprefabObj.transform.SetParent(parent);
                mprefabObj.gameObject.SetActive(true);
                mprefabObj.name = item1.PoolID.ToString();
                mprefabObj.GetComponent<MatchesPool>().SetValueToObject(item1.Entry, item1.PoolID, item1.PrizeList, item1.PrizePool, item1.SlotsFilled, item1.TotalSlots,item1.Type);

            }

            yield return new WaitForSeconds(0f);
            rect.reverseArrangement = false;

        }
       

    }
}