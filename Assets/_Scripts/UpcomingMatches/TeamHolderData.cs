using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TeamHolderData : MonoBehaviour
{

    public TMP_Text teamA;
    public TMP_Text teamB;
    public TMP_Text Match;
    public TMP_Text teamAFullName;
    public TMP_Text teamBFullName;
    public string TeamA;
    public string TeamB;
    public string ID;
    public bool isPrimeGame;
    public Button Click;
    public Button MyMatchDetails;
    public TMP_Text time;
    public Image[] Image;
    bool isCount = false;
    public string timeValSave;
    public string timeFormat;
    private void Awake()
    {
        Click.onClick.AddListener(() => { OnClickButton(); });
        //  MyMatchDetails.onClick.AddListener(() => { OnClickButton();  })
        ;
    }

    private void OnDestroy()
    {
        GameController.Instance.UnSubscribeMatchDetails();
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
    public void SetDetails(string teamAval, string teamBval, string id, string timeval, string _matchName)
    {
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;
        Match.text = "INTERNATIONAL";
        //foreach (var item in GameController.Instance.flags)
        //{

        //    if (item.Key == TeamA)
        //    {
        //        Image[0].sprite = GameController.Instance.flags[item.Key];
        //    }
        //    else if (item.Key == TeamB)
        //    {
        //        Image[1].sprite = GameController.Instance.flags[item.Key];
        //    }



        //}

        foreach (var item in GameController.Instance.countryFullName)
        {
            if (item.Key == TeamA)
            {
                teamAFullName.text = item.Value;
            }
            else if (item.Key == TeamB)
            {
                teamBFullName.text = item.Value;
            }
        }

        Match.text = _matchName;
        //TeamName[0].sprite = GameController.Instance.flags[GameController.Instance.flags.Keys.First(x => x == "AUS")];

        Debug.Log(ID + "&&&&&&&&&&&&&&&&&&&&&&&");
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }

        isCount = true;
    }

    public void OnClickButton()
    {

        UIController.Instance.ContestPanel.ShowMe();
        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(int.Parse(ID), TeamA, TeamB, timeFormat));
        UIController.Instance.MainMenuScreen.HideMe();
    }


    public void SetDetailsMymatches(string teamAval, string teamBval, string id, string timeval, String detail)
    {
        teamA.text = teamAval;
        teamB.text = teamBval;
        TeamA = teamAval;
        TeamB = teamBval;
        ID = id;
        Debug.Log(teamA);


        foreach (var item in GameController.Instance.flags)
        {

            if (item.Key == TeamA)
            {
                Image[0].sprite = GameController.Instance.flags[item.Key];
            }
            else if (item.Key == TeamB)
            {
                Image[1].sprite = GameController.Instance.flags[item.Key];
            }



        }
        Match.text = detail;


        foreach (var item in GameController.Instance.countryFullName)
        {

            if (item.Key == TeamA)
            {
                teamAFullName.text = GameController.Instance.countryFullName[item.Value];
            }
            else if (item.Key == TeamB)
            {
                teamBFullName.text = GameController.Instance.countryFullName[item.Value];
            }



        }
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }

        isCount = true;
    }
    public IEnumerator Timer(string timeString)
    {

        timeValSave = timeString;
        if (string.IsNullOrWhiteSpace(timeValSave)) yield break;



        string timeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        Debug.Log(DateTime.Now.ToString() + "$$$$$$$$$$$$$$");

        timeValSave = timeValSave.Replace('/', '-');
        timeNow = timeNow.Replace('/', '-');

        Debug.Log(timeValSave + "&&&               %%%%   " + timeNow);
        var matchduration = DateTime.Parse(timeValSave) - DateTime.Parse(timeNow);

        var TimeDifference = matchduration;

        if (TimeDifference.Days * 24 + TimeDifference.Hours <= 0)
        {
            if (TimeDifference.Minutes <= 0 && TimeDifference.Seconds <= 0)
            {
                time.text = "Live";
                timeFormat = "Live";
            }

            else
            {
                time.text = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
                timeFormat = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
            }

        }
        else
        {
            time.text = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
            timeFormat = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer(timeString));

    }

}
