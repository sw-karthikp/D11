using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class WinnerMyContest : MonoBehaviour
{
    public TMP_Text practiceName;
    public TMP_Text spots;
    public Dictionary<string, Prizevalues> prizeList = new Dictionary<string, Prizevalues>();
    public GameObject childWinner;
    public Transform parent;
    public GameObject Winning, objec;
    // Start is called before the first frame update
    private void OnEnable()
    {

        prizeList.Clear();
        foreach (var item in GameController.Instance.matchpool)
        {
            if (item.Value.MatchID == GameController.Instance.CurrentMatchID)
            {
                foreach (var item1 in item.Value.Pools.Values)
                {
                    if (GameController.Instance.CurrentPoolID == item1.PoolID)
                    {
                        Debug.Log(item1.Type + "###" + item1.TotalSlots.ToString());
                        practiceName.text =  item1.Type;
                        spots.text = item1.TotalSlots.ToString();
                      
                        if (item1.PoolID == "6")
                        {
                            Winning.SetActive(false);
                            objec.SetActive(true);
                
                        }
                        else
                        {
                            objec.SetActive(false);
                            Winning.SetActive(true);
                            prizeList = item1.PrizeList;
                            setData1();
                        }
                  
                    }

                }
              
             }
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


}
