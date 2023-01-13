using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameController;

public class WinnerLeaderBoard : UIHandler
{

    public static WinnerLeaderBoard Instance;

    public TMP_Text prizePool;
    public TMP_Text entryAmount;
    public TMP_Text spotsLeft;
    public TMP_Text totalSpots;
    public GameObject childWinner;
    public Transform parent;
    private void Awake()
    {
        Instance =this;
    }
    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

        foreach (Transform Child in parent)
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


    public void GetPrizeList( string poolId, Dictionary<string, Prizevalues> prizeList, string _prizePool , string _entryAmount ,string _spotsLeft ,string _totalsports)
    {
        prizePool.text = _prizePool;
        entryAmount.text = _entryAmount;
        spotsLeft.text = _spotsLeft;
        totalSpots.text = _totalsports;

        foreach (var item in prizeList.Values)
        {
            GameObject mprefab = Instantiate(childWinner, parent);
            mprefab.GetComponent<WinnerContainer>().setRank(item.Rank, item.Value.ToString());
        }
    }

}
