using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MyContest : MonoBehaviour
{
    public TMP_Text contestName;
    public TMP_Text spotsCount;
    public TMP_Text teamName;
    public TMP_Text teamCount;


    public void SetDataToMyContest(string _contestName ,string _spotsCount,string _teamName,string _teamCount)
    {
        contestName.text= _contestName;
        spotsCount.text= _spotsCount;
        teamName.text= _teamName;
        teamCount.text= _teamCount;
    }
}
