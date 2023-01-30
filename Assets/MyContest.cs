using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    int spotsFilled;
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
        spotsFilled = int.Parse(_spotsCount);
        slider.value = val2;
        spotsCount.text = (totalslots - spotsFilled) + "spots left";
        val2 = spotsFilled;
        slider.value = val2;
      
    }
}
