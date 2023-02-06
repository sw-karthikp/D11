using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using static AddNewPlayerHandler;
using static GameController;


public class MatchPoolType : MonoBehaviour
{
    Pools pool;
    public GameObject[] obj;
    public TMP_Text entryFee;
    public string PoolId;
    public string PoolTypeName;
    public bool Gauranted;
    public TMP_Text prizePool;
    public TMP_Text totalSpots;
    public TMP_Text slotsFilled;
    public Slider silder;
    public Button click;
    public Button entryButtonClick;
    public Dictionary<string, Prizevalues> prizeList = new();
    public Dictionary<string, Dictionary<string, string>> leader = new();
    int val1;
    int val2;
    public GameObject prizePoolText;
    public TMP_Text practice;
    public GameObject rupee;
    public GameObject amount;
    public GameObject Trophy;
    public TMP_Text Firstst;

    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnDestroy()
    {
        GameController.Instance.UnSubscribeMatchPools();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        click.onClick.AddListener(() =>
        {
            GameController.Instance.currentPools = pool;
            UIController.Instance.WinnerLeaderBoard.ShowMe();
            PrizeListShow();

        });
        entryButtonClick.onClick.AddListener(() => { DisplayTeamMembers(); });
    }



    public void SetValueToPoolObject(int _entryFee, string _poolId, Dictionary<string, Prizevalues> prize, Dictionary<string, Dictionary<string, string>> _leader, int _prizePool, int _slotsFilled, int _totalSlots, string _poolTypeName, Pools Pool, bool intractable)
    {
        pool = Pool;
        val1 = _totalSlots;
        val2 = _slotsFilled;
        prizeList = prize;
        PoolId = _poolId.ToString();
        if (_entryFee != 0)
        {
            rupee.SetActive(true);
            amount.SetActive(true);
            Trophy.SetActive(true);
            prizePoolText.SetActive(true);
            practice.gameObject.SetActive(false);
            Firstst.text = "<sprite index=0>" + " " + SetPrizevalue();

            prizePool.text = _prizePool.ToString();
            entryFee.text = "<sprite index=2>" + " " + _entryFee.ToString();
            obj[0].SetActive(true);
            obj[1].SetActive(false);
        }
        else
        {
            practice.gameObject.SetActive(true);
            prizePoolText.SetActive(false);
            practice.text = "Practice Contest";
            entryFee.text = "JOIN";
            rupee.SetActive(false);
            Trophy.SetActive(false);
            amount.SetActive(false);
            obj[0].SetActive(false);
            obj[1].SetActive(true);
        }

        totalSpots.text = _totalSlots.ToString() + " spots"; ;


        leader = _leader;
        PoolTypeName = _poolTypeName;
        slotsFilled.text = (_totalSlots - _slotsFilled) + " spots left";
        Debug.Log(_slotsFilled + "####" + _totalSlots);
        Debug.Log(_slotsFilled / _totalSlots + "$$$$$$$");
        float val = ((float)_slotsFilled / (float)_totalSlots);
        silder.value = val;
        entryButtonClick.interactable = intractable;
        if (val2 == _totalSlots)
        {
            entryButtonClick.interactable = false;

            entryFee.text = "CLOSED";
            slotsFilled.text = "Contest Full";
        }


    }

    public void PrizeListShow()
    {

        WinnerLeaderBoard.Instance.GetPrizeList(PoolId, prizeList, leader, prizePool.text, entryFee.text, val2, val1, entryButtonClick.interactable);
        GameController.Instance.CurrentPoolID = PoolId;
    }


    public void DisplayTeamMembers()
    {


        if (GameController.Instance.selectedMatches.Count >= 1)
        {

            if (GameController.Instance.selectedMatches.ContainsKey(GameController.Instance.CurrentMatchID))
            {
                GameController.Instance.CurrentPoolTypeName = PoolTypeName;
                GameController.Instance.CurrentPoolID = PoolId;
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
                GameController.Instance.CurrentPoolTypeName = PoolTypeName;
                GameController.Instance.CurrentPoolID = PoolId;
                ContestHandler.Instance.isCreateTeam = false;
                ContestHandler.Instance.isContest = false;
                UIController.Instance.SelectMatchTeam.ShowMe();
                Debug.Log(" ###### CREATE NEW  ######");
                
                return;
            }
        }


    }
    string rank;
    public string SetPrizevalue()
    {

        foreach (var item in prizeList)
        {
            Debug.Log(item.Key + "@@@@@@@@@");
            var val = item.Key.Split("p");
            var valid = val.Last();
            Debug.Log(valid + "^^^^^^^^^" + PoolId);

            {
                if (item.Value.Rank == "1")
                {
                    rank = item.Value.Value.ToString();
                    Debug.Log("$$$$$" + rank);
                }



            }
        }

        return rank;
    }
}
