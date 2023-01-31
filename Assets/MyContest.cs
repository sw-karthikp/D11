using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public TMP_Text rank;
    public TMP_Text joinedTeam;
    public  Slider slider;
    public string poolID;
    int val2;
    int totalslots;
    int spotsFilled;
    public Button onclick;
    public Button onclick1;

    private void Awake()
    {
        onclick.onClick.AddListener(()=> { UIController.Instance.myLeadeBoardVal.ShowMe(); SetPoolID(); });
    }

    public void SetDataToMyContest(string _contestName ,string _spotsCount, string _totalspots , string _teamName,string _teamCount ,string _joinedTeam, string _poolID)
    {
        contestName.text= _contestName;
        spotsCount.text= _spotsCount;
        totalSpots.text= _totalspots;
        teamName.text= _teamName;
        joinedTeam.text= _joinedTeam;
        teamName.text= $"T{_teamCount}";
        float val = ((float)spotsFilled / (float)totalslots);
        slider.value = val;
        poolID = _poolID;
       
    }
    public void SetPoolID()
    {
        GameController.Instance.CurrentPoolID = poolID;
    }

    public void SetDataToMyContestNEW(string _contestName, string _spotsCount, string _totalspots, string _teamName, string _teamCount, string _joinedTeam, string _poolID)
    {
        var teamCountval = _teamCount.Split("Team");
         string Count = teamCountval.Last();
        contestName.text = _contestName;
        totalSpots.text = _totalspots + " spots";
        teamName.text = _teamName;
        joinedTeam.text = $"Joined with {1} team";
        teamCount.text = $"T{Count}";
        poolID = _poolID;
        totalslots = int.Parse(_totalspots);
        spotsFilled = int.Parse(_spotsCount);
        slider.value = val2;
        spotsCount.text = (totalslots - spotsFilled) + "spots left";
        float val = ((float)spotsFilled / (float)totalslots);
        slider.value = val;


    }
}
