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
    public TMP_Text prizepoolTxt;
    public TMP_Text entryAmount;
    public TMP_Text Rupee;
    public TMP_Text PracticeText;
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
    public bool  isclick1 =true;
   public   bool isclick2 = true;
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

       
        isclick1 = true;
        isclick2 = true;
        Invoke("setPractice", 0.1f);

    }

    public void setPractice()
    {
        if (GameController.Instance.CurrentPoolID == "6")
        {
            prizePool.gameObject.SetActive(false);
            Rupee.gameObject.SetActive(false);
            prizepoolTxt.gameObject.SetActive(false);
            PracticeText.gameObject.SetActive(true);
        }
        else
        {
            prizePool.gameObject.SetActive(true);
            Rupee.gameObject.SetActive(true);
            prizepoolTxt.gameObject.SetActive(true);
            PracticeText.gameObject.SetActive(false);
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
        if (swap[0].isOn && isclick1)
        {
            val1.text = "Rank";
            val2.text = "Entry";
            parent.gameObject.SetActive(true);
            rect.GetComponent<ScrollRect>().content = parent.GetComponent<RectTransform>();
            setData1();
            isclick1= false;
        }
        else
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);

            }
            parent.gameObject.SetActive(false);
            isclick1 = true;
        }
    }
    public void OnClickLeader()
    {
        if (swap[1].isOn && isclick2)
        {
            val1.text = "Name";
            val2.text = "";
            parentLeader.gameObject.SetActive(true);
            rect.GetComponent<ScrollRect>().content = parentLeader.GetComponent<RectTransform>();
            setData2();
            isclick2= false;
        }
        else
        {
            foreach (Transform child in parentLeader)
            {
                Destroy(child.gameObject);

            }
            parentLeader.gameObject.SetActive(false);
            isclick2 = true;
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
        if (_entryAmount == "JOIN")
        {

            entryAmount.text = "JOIN";
        }
        else if(_entryAmount == "CLOSED")
        {

            entryAmount.text = "CLOSED";
        }
        else
        {
            entryAmount.text = "JOIN" + " " + _entryAmount;
        }
       
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

    public void DisplayTeamMembers()
    {


        if (GameController.Instance.selectedMatches.Count >= 1 && GameController.Instance.selectedMatches.ContainsKey(GameController.Instance.CurrentMatchID))
        {

            foreach (var key in GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools.Keys)
            {

                if (GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedTeam.Keys.Count > 1)
                {
                    Debug.Log(" ######## JOIN GAME WITH MORE  TEAMS AVAILABLE ######");
                    ContestHandler.Instance.isCreateTeam = false;
                    ContestHandler.Instance.isContest = false;
                    ContestHandler.Instance.selectTeams.SetActive(true);
                    return;

                }
                else
                {
                    if (GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools[key].PoolID == GameController.Instance.CurrentPoolID)
                    {

                        UIController.Instance.SelectMatchTeam.ShowMe();
                        ContestHandler.Instance.isCreateTeam = false;
                        ContestHandler.Instance.isContest = false;
                        Debug.Log(" ###### CREATE NEW TEAM FOR SAME POOL IF  PLAYER JOINED AGAIN ######");
                        return;

                    }
                    else
                    {

                        Debug.Log(" ######## JOIN GAME WITH EXSISTING TEAM AVAILABLE ######");
                        ContestHandler.Instance.isCreateTeam = false;
                        ContestHandler.Instance.isContest = true;
                        ContestHandler.Instance.conformHandler.ShowMe();
                        return;

                    }

                }

            }
        }
        else
        {

            ContestHandler.Instance.isCreateTeam = false;
            ContestHandler.Instance.isContest = false;
            UIController.Instance.SelectMatchTeam.ShowMe();
            Debug.Log(" ###### CREATE NEW  ######");

            return;
        }


    }





}
