using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using D11;
using System.Globalization;

public class TeamHolderData : MonoBehaviour
{

    public TMP_Text teamA;
    public TMP_Text teamB;
    public string TeamA;
    public string TeamB;
    public int ID;
    public bool isPrimeGame;
    public Button Click;
    public TMP_Text time;
    bool isCount = false;
    public string timeValSave;
    private void Awake()
    {
        Click.onClick.AddListener(() => { OnClickButton(); });
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        Debug.Log("*************************** JJJJJJJJ");
        if (isCount)
        {
            StopCoroutine(Timer(timeValSave));
            StartCoroutine(Timer(timeValSave));
    
        }
  
    }
    public void SetDetails(string teamAval, string teamBval, int id, string timeval)
    {
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;
        
        if(gameObject.activeInHierarchy)
        {
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }
 
        isCount = true;
    }

    public void OnClickButton()
    {

        UIController.Instance.ContestPanel.ShowMe();
        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(ID, TeamA, TeamB ,timeValSave));
        UIController.Instance.MainMenuScreen.HideMe();
    }

    public IEnumerator Timer(string timeString)
    {
        timeValSave = timeString;
        if (string.IsNullOrWhiteSpace(timeValSave)) yield break;
        string[] formats = { "dd/MM/yyyy HH:mm:ss"};
        var matchduration = DateTime.Parse(CommonFunctions.Instance.ChangeDateFormat(timeValSave,formats)) - DateTime.Now;
       // Debug.Log(DateTime.Parse(CommonFunctions.Instance.ChangeDateFormat(timeValSave, formats)));
        Debug.Log(CommonFunctions.Instance.ChangeDateFormat(timeValSave, formats) + "$$$$$$$$$$$" + DateTime.Now);
        var TimeDifference = matchduration;
        if (TimeDifference.Days * 24 + TimeDifference.Hours <= 0 )
        {
            if(TimeDifference.Minutes<=0 && TimeDifference.Seconds<=0)
                time.text =  "Live";
            else
            time.text =  TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
        }
        else
        {
            time.text = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
        }
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer(timeString));

    }

}
