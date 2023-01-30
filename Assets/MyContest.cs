using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MyContest : MonoBehaviour
{
    public TMP_Text contestName;
    public TMP_Text spotsCount;
    public TMP_Text totalSpots;
    public TMP_Text teamName;
    public TMP_Text teamCount;
    public TMP_Text joinedTeam;
    public  Slider slider;
    int poolID;
    int val2;
    int totalslots;
    public void SetDataToMyContest(string _contestName ,string _spotsCount, string _totalspots , string _teamName,string _teamCount ,string _joinedTeam)
    {
        contestName.text= _contestName;
        spotsCount.text= _spotsCount;
        totalSpots.text= _totalspots;
        teamName.text= _teamName;
        joinedTeam.text= _joinedTeam;
        teamCount.text= _teamCount;
        slider.value = int.Parse(spotsCount.text);
    }

    public void SetDataToMyContestNEW(string _contestName, string _spotsCount, string _totalspots, string _teamName, string _teamCount, string _joinedTeam, int _poolID)
    {
        contestName.text = _contestName;
        totalSpots.text = _totalspots + " spots";
        teamName.text = _teamName;
        joinedTeam.text = _joinedTeam;
        teamCount.text = _teamCount;
        poolID = _poolID;
        totalslots = int.Parse(_totalspots);
        if (GameController.Instance._joinedPlayers.Count >= 1)
        {
            foreach (var item in GameController.Instance._joinedPlayers)
            {
                if (item.Key == GameController.Instance.CurrentMatchID)
                {
                  
                    val2 = item.Value.Values.Count;
                    slider.value = val2;
                    spotsCount.text = (totalslots - item.Value.Values.Count) + "spots left";
                    val2 = item.Value.Values.Count;
                    slider.value = val2;
           
                }


            }
        }
    }
}
