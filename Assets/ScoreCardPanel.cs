using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEditor.Progress;
using System.Linq;

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


    [Header("objectToExpand")]
    public GameObject teamAExpandBatters;
    public GameObject teamBExpandBatters;
    public GameObject teamAExpandBowlers;
    public GameObject teamBExpandBowlers;

    [Header("ImageToRotate")]
    public Image arrowA;
    public Image arrowB;

    bool isOnA = true;
    bool isOnB = true;


    private void Awake()
    {
        Instance = this;
        onClickA.onClick.AddListener(() => { OnClickExpandA(); Canvas.ForceUpdateCanvases(); });
        onClickB.onClick.AddListener(() => { OnClickExpandB(); Canvas.ForceUpdateCanvases(); });
    }
    private void OnEnable()
    {
        GetData();
    }

    public void GetData()
    {
        TeamAText.text = GameController.Instance.scoreCard.TeamA;
        TeamBText.text = GameController.Instance.scoreCard.TeamB;
    }

    public void OnClickExpandA()
    {
        if (isOnA)
        {
            teamAExpandBatters.SetActive(true);
            teamAExpandBowlers.SetActive(true);
            arrowA.transform.DORotate(new Vector3(0, 0, 90), 0.1f);
            isOnA = false;
            InstantDataInnings1();
        }
        else
        {
            teamAExpandBatters.SetActive(false);
            teamAExpandBowlers.SetActive(false);
            arrowA.transform.DORotate(new Vector3(0, 0, -90), 0.1f);
            isOnA = true;
        }
    }
    public void OnClickExpandB()
    {
        if (isOnB)
        {
            teamBExpandBatters.SetActive(true);
            teamBExpandBowlers.SetActive(true);
            arrowB.transform.DORotate(new Vector3(0, 0, 90), 0.1f);
            isOnB = false;
            InstantDataInnings2();
        }
        else
        {
            teamBExpandBatters.SetActive(false);
            teamBExpandBowlers.SetActive(false);
            arrowB.transform.DORotate(new Vector3(0, 0, -90), 0.1f);
            isOnB = true;
        }
    }



    public void InstantDataInnings1()
    {

        foreach (Transform child in parentABatter)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in parentABowler)
        {
            child.gameObject.SetActive(false);
        }
        foreach (var item1 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings1").Value.Batting.Score)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBatter");
            mprefabObj.transform.SetParent(parentABatter);
            mprefabObj.gameObject.SetActive(true);
            var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamA).Players.Values.First(x => x.ID == item1.Key);
            Debug.Log(detail.Name + "BBBBBBBBB");
            mprefabObj.GetComponent<BatterContainer>().SetData(detail.Name, item1.Value.Status, item1.Value.Score, item1.Value.Balls, item1.Value.Four, item1.Value.Six, "");
        }

        foreach (var item2 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings1").Value.Bowling)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBowler");
            mprefabObj.transform.SetParent(parentABowler);
            mprefabObj.gameObject.SetActive(true);
            var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamB).Players.Values.First(x => x.ID == item2.Key);
            Debug.Log(detail.Name + "CCCCCCCCCC");
            mprefabObj.GetComponent<BowlerContainer>().SetData(detail.Name, item2.Value.Over, item2.Value.Mainden, item2.Value.Runs, item2.Value.Wicket, item2.Value.Extra);
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
        foreach (var item1 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings2").Value.Batting.Score)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBatter");
            mprefabObj.transform.SetParent(parentBBatter);
            mprefabObj.gameObject.SetActive(true);
            var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamB).Players.Values.First(x => x.ID == item1.Key);
            Debug.Log(detail.Name + "AAAAA");
            mprefabObj.GetComponent<BatterContainer>().SetData(detail.Name, item1.Value.Status, item1.Value.Score, item1.Value.Balls, item1.Value.Four, item1.Value.Six, "");
        }

        foreach (var item2 in GameController.Instance.scoreCard.MatchDetails.First(x => x.Key == "Innings2").Value.Bowling)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerScoreBowler");
            mprefabObj.transform.SetParent(parentBBowler);
            mprefabObj.gameObject.SetActive(true);
            var detail = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.scoreCard.TeamA).Players.Values.First(x => x.ID == item2.Key);
            Debug.Log(detail.Name + "DDDDDDDDDD");
            mprefabObj.GetComponent<BowlerContainer>().SetData(detail.Name, item2.Value.Over, item2.Value.Mainden, item2.Value.Runs, item2.Value.Wicket, item2.Value.Extra);
        }
    }
}
