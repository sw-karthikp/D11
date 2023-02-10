using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using System.Linq;
using Unity.VisualScripting;
using System;
using Firebase.Firestore;

public class ScoreCardPanel : MonoBehaviour
{
    public static ScoreCardPanel Instance;
    [Header("Variables")]
    public TMP_Text TeamAText;
    public TMP_Text OverAText;
    public TMP_Text ScoreATeam;
    public TMP_Text TeamBText;
    public TMP_Text OverBText;
    public TMP_Text ScoreBTeam;

    public Button onClickA;
    public Button onClickB;
    [Header("Transform")]
    public Transform parentABatter;
    public Transform parentBBatter;
    public Transform parentABowler;
    public Transform parentBBowler;
    public Transform content;
    

    [Header("objectToExpand")]
    public GameObject teamAExpandBatters;
    public GameObject teamBExpandBatters;
    public GameObject teamAExpandBowlers;
    public GameObject teamBExpandBowlers;

    [Header("ImageToRotate")]
    public Image arrowA;
    public Image arrowB;

    bool isOnA = false;
    bool isOnB = false;


    private void Awake()
    {
        Instance = this;
        onClickA.onClick.AddListener(() => { OnClickExpandA(); });
        onClickB.onClick.AddListener(() => { OnClickExpandB();  });
    }
    private void OnEnable()
    {
        GetData();
        teamAExpandBatters.SetActive(false);
        teamAExpandBowlers.SetActive(false);
        arrowA.transform.DORotate(new Vector3(0, 0, -90), 0.1f);
        teamBExpandBatters.SetActive(false);
        teamBExpandBowlers.SetActive(false);
        arrowB.transform.DORotate(new Vector3(0, 0, -90), 0.1f);
        OverAText.text = "(overs)";
        ScoreATeam.text = "-";
        OverBText.text = "(overs)";
        ScoreBTeam.text = "-";
        InstantDataInnings1();
        InstantDataInnings2();
    }

    private void OnDisable()
    {
   
    }


    public void GetData()
    {
        TeamAText.text = GameController.Instance.scoreCard.TeamA;
        TeamBText.text = GameController.Instance.scoreCard.TeamB;
    }

    public void OnClickExpandA()
    {
        if (!isOnA)
        {
            teamAExpandBatters.SetActive(true);
            teamAExpandBowlers.SetActive(true);
            arrowA.transform.DORotate(new Vector3(0, 0, 90), 0.1f);
            isOnA = true;
            InstantDataInnings1();
        }
        else
        {
            teamAExpandBatters.SetActive(false);
            teamAExpandBowlers.SetActive(false);
            arrowA.transform.DORotate(new Vector3(0, 0, -90), 0.1f);
            isOnA = false;
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentABatter.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.transform as RectTransform);
    }
    public void OnClickExpandB()
    {
        if (!isOnB)
        {
            teamBExpandBatters.SetActive(true);
            teamBExpandBowlers.SetActive(true);
            arrowB.transform.DORotate(new Vector3(0, 0, 90), 0.1f);
            isOnB = true;
            InstantDataInnings2();
        }
        else
        {
            teamBExpandBatters.SetActive(false);
            teamBExpandBowlers.SetActive(false);
            arrowB.transform.DORotate(new Vector3(0, 0, -90), 0.1f);
            isOnB = false;
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentBBatter.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.transform as RectTransform);
    }



    public void InstantDataInnings1()
    {
        if (GameController.Instance.scoreCard != null)
        {
            foreach (Transform child in parentABatter)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in parentABowler)
            {
                child.gameObject.SetActive(false);
            }
            if (GameController.Instance.scoreCard.MatchDetails.ContainsKey("Innings1"))
            {
                foreach (var item1 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings1").Value.Batting.Score)
                {
                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBatter");
                    mprefabObj.transform.SetParent(parentABatter);
                    mprefabObj.gameObject.SetActive(true);

                    try
                    {
                        var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamA).Players.Values.First(x => x.ID == item1.Key);
                        mprefabObj.GetComponent<BatterContainer>().SetData(detail.Name, item1.Value.Status, item1.Value.Score, item1.Value.Balls, item1.Value.Four, item1.Value.Six, "");
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                  
                    

                }
                foreach (var item2 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings1").Value.Bowling)
                {
                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBowler");
                    mprefabObj.transform.SetParent(parentABowler);
                    mprefabObj.gameObject.SetActive(true);

                    try
                    {
                        var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamB).Players.Values.First(x => x.ID == item2.Key);
                        mprefabObj.GetComponent<BowlerContainer>().SetData(detail.Name, item2.Value.Over, item2.Value.Mainden, item2.Value.Runs, item2.Value.Wicket, item2.Value.Extra);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }

                }

                foreach (var item in GameController.Instance.scoreCard.MatchDetails)
                {
                    if (item.Key == "Innings1")
                    {
                        OverAText.text = $"({item.Value.InningsOvers} Overs)";
                        ScoreATeam.text = item.Value.InningsRuns.ToString();

                    }

                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(parentABatter.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content.transform as RectTransform);
        }


    }

    public void InstantDataInnings2()
    {

        foreach (Transform child in parentBBatter)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in parentBBowler)
        {
            child.gameObject.SetActive(false);
        }

        if(GameController.Instance.scoreCard.MatchDetails.ContainsKey("Innings2"))
        {
            foreach (var item1 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings2").Value.Batting.Score)
            {
                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBatter");
                mprefabObj.transform.SetParent(parentBBatter);
                mprefabObj.gameObject.SetActive(true);
                try
                {
                    var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamB).Players.Values.First(x => x.ID == item1.Key);
                    mprefabObj.GetComponent<BatterContainer>().SetData(detail.Name, item1.Value.Status, item1.Value.Score, item1.Value.Balls, item1.Value.Four, item1.Value.Six, "");
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            
            }

            foreach (var item2 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings2").Value.Bowling)
            {
                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBowler");
                mprefabObj.transform.SetParent(parentBBowler);
                mprefabObj.gameObject.SetActive(true);
                try
                {
                    var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamA).Players.Values.First(x => x.ID == item2.Key);
                    mprefabObj.GetComponent<BowlerContainer>().SetData(detail.Name, item2.Value.Over, item2.Value.Mainden, item2.Value.Runs, item2.Value.Wicket, item2.Value.Extra);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

            }
        }

        foreach (var item in GameController.Instance.scoreCard.MatchDetails)
        {
            if (item.Key == "Innings2")
            {
                OverBText.text = $"({item.Value.InningsOvers} Overs)";
                ScoreBTeam.text = item.Value.InningsRuns.ToString();
         
            }

        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentABatter.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.transform as RectTransform);
    }
}
