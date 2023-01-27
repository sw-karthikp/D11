using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameController;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using Unity.VisualScripting;

public class WinnerLeaderBoard : UIHandler
{

    public static WinnerLeaderBoard Instance;

    public TMP_Text prizePool;
    public TMP_Text entryAmount;
    public TMP_Text spotsLeft;
    public TMP_Text totalSpots;
    public GameObject childWinner;
    public Transform parent;
    public GameObject childLeaderBoard;
    public Transform parentLeader;
    public Toggle[] swap;
    public RectTransform rect;
    public TMP_Text val1;
    public TMP_Text val2;
    FirebaseFirestore db;
    public Dictionary<string, Prizevalues> prizeList = new Dictionary<string, Prizevalues>();
    public Dictionary<string, string> leader = new Dictionary<string, string>();
    private void Awake()
    {
        Instance = this;
        swap[0].onValueChanged.AddListener(delegate { OnClickWinner(); });
        swap[1].onValueChanged.AddListener(delegate { OnClickLeader(); });
        db = FirebaseFirestore.DefaultInstance;
    }
    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

        foreach (Transform Child in parent)
        {
            Destroy(Child.gameObject);
        }
        foreach (Transform child in parentLeader)
        {
            Destroy(child.gameObject);

        }

    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        swap[0].isOn = true;
        Invoke("setData1", 0.5f);
    }

      

    public override void OnBack()
    {

    }
    private void OnEnable()
    {
   
    }
    public void OnClickWinner()
    {
        if (swap[0].isOn)
        {
            val1.text = "Rank";
            val2.text = "Entry";
            parent.gameObject.SetActive(true);
            rect.GetComponent<ScrollRect>().content = parent.GetComponent<RectTransform>();
            setData1();
        }
        else
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);

            }
            parent.gameObject.SetActive(false);
        }
    }
    public void OnClickLeader()
    {
        if (swap[1].isOn)
        {
            val1.text = "Name";
            val2.text = "Points";
            parentLeader.gameObject.SetActive(true);
            rect.GetComponent<ScrollRect>().content = parentLeader.GetComponent<RectTransform>();
            setData2();
        }
        else
        {
            foreach (Transform child in parentLeader)
            {
                Destroy(child.gameObject);

            }
            parentLeader.gameObject.SetActive(false);
        }
    }

    public void setData1()
    {
        foreach (var item in prizeList.Values)
        {
       
            GameObject mprefab = Instantiate(childWinner, parent);

            mprefab.GetComponent<WinnerContainer>().setRank(item.Rank, item.Value.ToString());
        }
    }
    public void setData2()
    {
        foreach (var item in leader)
        {
       
            GameObject mprefab = Instantiate(childLeaderBoard, parentLeader);
            foreach (var item1 in GameController.Instance._joinedPlayers)
            {
                foreach (var item2 in item1.Value)
                {
                    if(item.Key == item2.Key)
                    {
                        mprefab.GetComponent<LeaderBoardContainer>().SetLeaderBoard(item2.Value, item.Value);
                    }
                }
              
            }
           
        }
    }

    public void GetPrizeList(string poolId, Dictionary<string, Prizevalues> _prizeList, string _prizePool, string _entryAmount, string _spotsLeft, string _totalsports)
    {
        prizePool.text = _prizePool;
        entryAmount.text = _entryAmount;
        spotsLeft.text = _spotsLeft;
        totalSpots.text = _totalsports;
        prizeList = _prizeList;

    }


    public void GetLeaderBoardList(string poolId, Dictionary<string, string> _leader, string _prizePool, string _entryAmount, string _spotsLeft, string _totalsports)
    {
        prizePool.text = _prizePool;
        entryAmount.text = _entryAmount;
        spotsLeft.text = _spotsLeft;
        totalSpots.text = _totalsports;
        leader = _leader;

    }




}
