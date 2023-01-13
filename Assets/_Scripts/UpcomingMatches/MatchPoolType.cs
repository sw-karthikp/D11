using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class MatchPoolType : MonoBehaviour
{
    public TMP_Text entryFee;
    public string PoolId;
    public bool Gauranted;
    public TMP_Text prizePool;
    public TMP_Text totalSpots;
    public TMP_Text slotsFilled;
    public Slider silder;
    public Button click;
    public Button entryButtonClick;
    public Dictionary<string, Prizevalues> prizeList =new();
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

    public void SetValueToPoolObject(int _entryFee, int _poolId, Dictionary<string, Prizevalues> prize, int _prizePool, int _slotsFilled, int _totalSlots)
    {
        prizeList.Clear();
        int val1 = _totalSlots;
        int val2 = _slotsFilled;
        int val = val1 - val2;
        entryFee.text = _entryFee.ToString();
        prizePool.text = _prizePool.ToString();
        slotsFilled.text = val.ToString() + " spots left";
        totalSpots.text = _totalSlots.ToString();
        silder.minValue = 0;
        silder.maxValue = _totalSlots;
        silder.value = _slotsFilled;
        PoolId = _poolId.ToString();
        prizeList = prize;
    }

    public void PrizeListShow()
    {
        WinnerLeaderBoard.Instance.GetPrizeList(PoolId, prizeList, prizePool.text,entryFee.text,slotsFilled.text,totalSpots.text);
    }

    public void DisplayTeamMembers()
    {
        GameController.Instance.CurrentPoolID = PoolId;
        UIController.Instance.SelectMatchTeam.ShowMe();
    }
}
