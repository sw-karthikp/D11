using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerMyContest : MonoBehaviour
{
    public TMP_Text practiceName;
    public TMP_Text spots;
    // Start is called before the first frame update
    private void OnEnable()
    {
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
                    }

                }
              
             }
        }
    }


}
