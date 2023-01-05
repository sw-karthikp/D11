using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LiveMatchController : MonoBehaviour
{
    public Button StartScore;
    [Header("Variables")]
    public TMP_Text date;
    public TMP_Text teamA;
    public TMP_Text teamB;
    public TMP_Text matchType;
  //  public TMP_Text PrizeValue;

    private void Awake()
    {
        StartScore.onClick.AddListener(() => { OnclickScore();  });
    }


    public void OnclickScore()
    {
        AdminUIController.Instance.ScoreSettingScreen.ShowMe();
        ScoreSettingPanel.Instance.SetCurrentMacthValue(teamA.text,teamB.text);
    }

    public void SetValueToLiveMatch(string _date, string _matchType ,string  _teamA, string _teamB)
    {
        date.text = _date;
        teamA.text = _teamA;
        teamB.text = _teamB;
        matchType.text = _matchType;
    }
}
