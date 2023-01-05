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
    public ScrollRect rect;
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
        rect.content = viewTypePoolMatches.GetComponent<RectTransform>();
        viewAllPoolMatches.SetActive(false);
        viewTypePoolMatches.SetActive(true);

    }

    public void OnClickClose()
    {
        UIController.Instance.MainMenuScreen.ShowMe();
        if(UIController.Instance.WinnerLeaderBoard.gameObject.activeSelf)
        {
            UIController.Instance.WinnerLeaderBoard.HideMe();
            return;
        }
        UIController.Instance.ContestPanel.HideMe();
    }


    public IEnumerator SetUpcomingMatchPoolDetails(int MatchId ,string teamA ,string teamB)
    {
        MatchIDVal = MatchId.ToString();
        TeamA = teamA;
        TeamB = teamB;
        GameController.Instance.CurrentTeamA = teamA;
        GameController.Instance.CurrentTeamB = teamB;
        GameController.Instance.CurrentMatchID = MatchId;
        val.Clear();
        Debug.Log("******************");

        for (int i = 0; i < GameController.Instance.matchpool.Count; i++)
        {


            for (int j = 0; j < GameController.Instance.matchpool[i].Pools.Count; j++)
            {


                if (GameController.Instance.matchpool[i].MatchID == MatchId)
                {
                    bool canSkip = false;
                    foreach (Transform child in parent)
                    {
                        if (child.name.Contains(GameController.Instance.matchpool[i].Pools[j].PoolID.ToString()))
                        {
                            canSkip = true;
                            break;
                        }
                    }
                    if (canSkip) continue;
                    GameObject mPoolPrefab = Instantiate(ViewallChild, parent);
                    mPoolPrefab.name = GameController.Instance.matchpool[i].Pools[j].PoolID.ToString();
                    mPoolPrefab.GetComponent<MatchPoolType>().SetValueToPoolObject(GameController.Instance.matchpool[i].Pools[j].Entry, GameController.Instance.matchpool[i].Pools[j].PoolID, GameController.Instance.matchpool[i].Pools[j].PrizeList,
                    GameController.Instance.matchpool[i].Pools[j].PrizePool, GameController.Instance.matchpool[i].Pools[j].SlotsFilled, GameController.Instance.matchpool[i].Pools[j].TotalSlots);
                    yield return new WaitForSeconds(0f);
                }


            }
        }
    }
}
