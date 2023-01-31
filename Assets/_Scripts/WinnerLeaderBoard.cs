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
using System.Linq;

public class WinnerLeaderBoard : UIHandler
{

    public static WinnerLeaderBoard Instance;

    public TMP_Text prizePool;
    public TMP_Text entryAmount;
    public TMP_Text spotsLeft;
    public TMP_Text totalSpots;
    public Slider val;
    public GameObject childWinner;
    public Transform parent;
    public GameObject childLeaderBoard;
    public Transform parentLeader;
    public Toggle[] swap;
    public RectTransform rect;
    int val1slider;
    int val2slider;
    public TMP_Text val1;
    public TMP_Text val2;
    FirebaseFirestore db;
    public Dictionary<string, Prizevalues> prizeList = new Dictionary<string, Prizevalues>();
    public Dictionary<string, Dictionary<string, string>> leader = new();
    string name;
    string value;
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
        if (parent.childCount == 0)
        {
            Invoke("setData1", 0.2f);
        }


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
            val2.text = "";
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


                    foreach (var item3 in item.Value)
                    {
                        var JoinedPooID = item2.Key;
                        var poolID = JoinedPooID.Split("P");
                        string poolIDVal = poolID.Last();
                        Debug.Log(poolIDVal + "%%%%%%%%%%%%%%%%%%%%" + JoinedPooID);
                        if (GameController.Instance.CurrentPoolID == poolIDVal)
                        {

                            if (item3.Key == "Name")
                            {
                                name = item3.Value;
                            }
                            if (item3.Key == "Value")
                            {
                                value = item3.Value;
                            }



                            mprefab.GetComponent<LeaderBoardContainer>().SetLeaderBoard(name, "");


                        }
                    }

                }

            }

        }
    }

    public void GetPrizeList(string poolId, Dictionary<string, Prizevalues> _prizeList, Dictionary<string, Dictionary<string, string>> _leader, string _prizePool, string _entryAmount, int _spotsLeft, int _totalsports, bool intractable)
    {
        prizePool.text = _prizePool;
        if (_entryAmount != "0")
            entryAmount.text = "JOIN" + " " + _entryAmount;
        else
            entryAmount.text = "Free";
        entryAmount.transform.parent.gameObject.GetComponent<Button>().interactable = intractable;
        val1slider = _spotsLeft;
        val2slider = _totalsports;
        prizeList = _prizeList;
        Debug.Log(val1slider + " ####" + val2slider);
        float valtest = ((float)val1slider / (float)val2slider);
        Debug.Log(valtest);
        val.value = valtest;
        spotsLeft.text = (_totalsports - _spotsLeft).ToString() + "spots left";
        totalSpots.text = val2slider.ToString() + " spots";
        leader = _leader;
        if (_totalsports == _spotsLeft)
        {
            spotsLeft.text = "Contest Full";
        }
    }






}
