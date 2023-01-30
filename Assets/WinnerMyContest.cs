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
        //foreach (var item in GameController.Instance.matchpool)
        //{
        //   if( item.Value.MatchID == GameController.Instance.CurrentMatchID)
        //    {
        //        for
        //    }
        //}
        //if(GameController.Instance.CurrentPoolID == )

        //TO do 
        practiceName.text = "";
        spots.text = "";
        
    }


}
