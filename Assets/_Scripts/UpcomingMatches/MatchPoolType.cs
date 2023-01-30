using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using static GameController;

public class MatchPoolType : MonoBehaviour
{
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
    public Dictionary<string, Prizevalues> prizeList =new();
    public Dictionary<string, Dictionary<string, string>> leader = new();
    int val1;
    int val2;
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
        click.onClick.AddListener(() => { UIController.Instance.WinnerLeaderBoard.ShowMe(); PrizeListShow(); });
        entryButtonClick.onClick.AddListener(() => { DisplayTeamMembers(); });
    }

    

    public void SetValueToPoolObject(int _entryFee, int _poolId, Dictionary<string, Prizevalues> prize, Dictionary<string, Dictionary<string, string>> _leader, int _prizePool, int _slotsFilled, int _totalSlots,string _poolTypeName)
    {
       
        val1 = _totalSlots;
        entryFee.text = "<sprite index=2>" +" " +_entryFee.ToString();
      
        prizePool.text = _prizePool.ToString();
        totalSpots.text = _totalSlots.ToString();
        silder.minValue = 0;
        silder.maxValue = _totalSlots;
      
        PoolId = _poolId.ToString();
        prizeList = prize;
        leader = _leader;
        PoolTypeName = _poolTypeName;

        if(GameController.Instance._joinedPlayers.Count >= 1)
        {
            foreach (var item in GameController.Instance._joinedPlayers)
            {
                if (item.Key == GameController.Instance.CurrentMatchID)
                {
                    foreach (var poolID in item.Value)
                    {
                 
                        
                            slotsFilled.text = (_totalSlots - item.Value.Values.Count) + "spots left";
                            val2 = poolID.Value.Count;
                            silder.value = val2;
                            if (val2 == _totalSlots)
                            {
                                entryButtonClick.interactable = false;
                                click.interactable = false;
                                entryFee.text = "Closed";
                            }
                        
                  
                    }

                }

               
            }
        }
 
    }

    public void PrizeListShow()
    {

        WinnerLeaderBoard.Instance.GetPrizeList(PoolId, prizeList,leader, prizePool.text,entryFee.text, val2, val1);
        GameController.Instance.CurrentPoolID = PoolId;
    }
    public void DisplayTeamMembers()
    {
       
        GameController.Instance.CurrentPoolTypeName = PoolTypeName;
        GameController.Instance.CurrentPoolID = PoolId;
        UIController.Instance.SelectMatchTeam.ShowMe();
    }
}
