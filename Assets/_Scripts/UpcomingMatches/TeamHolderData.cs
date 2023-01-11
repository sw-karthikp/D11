using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Firebase.Storage;
using UnityEditor.SceneManagement;
using UnityEngine.Networking;
using Firebase.Extensions;
using D11;

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
        if (isCount)
        {
            StopCoroutine(Timer(timeValSave));
            StartCoroutine(Timer(timeValSave));

        }

    }
    public void SetDetails(string teamAval, string teamBval, string id, string timeval, string _matchName)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;

        if (gameObject.activeInHierarchy)
        {
            Debug.Log(timeval + "%%%%%%%%%%%%%%%%%%%%%%%%%");
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }

        isCount = true;

       StartCoroutine(SetFullCountryName());
    }


    public IEnumerator SetFullCountryName()
    {
        GameController.Instance.countryFullName.Clear();
        GameController.Instance.countryFullName = new Dictionary<string, string>() { { "AUS", "Australia" }, { "IND", "India" },
        { "PAK", "Pakistan" },{ "ENG", "England" }};
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

        if(FireBaseManager.Instance.isFirstTime)
        {
            yield return new WaitForSeconds(1.2f);
            FireBaseManager.Instance.isFirstTime = false;
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

        foreach (var item1 in GameController.Instance.countrySpriteImage)
        {
            if (teamA.text == item1.Key)
            {
                Image[0].sprite = item1.Value;
            }
            else if (teamB.text == item1.Key)
            {
                Image[1].sprite = item1.Value;
            }
        }
    }

    public void OnClickButton()
    {

        UIController.Instance.ContestPanel.ShowMe();
        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(int.Parse(ID), TeamA, TeamB, timeFormat));
        UIController.Instance.MainMenuScreen.HideMe();
    }




    public void SetDetailsMymatches(string teamAval, string teamBval, string id, string timeval, string _matchName)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        TeamA = teamAval;
        TeamB = teamBval;
        ID = id;
        Debug.Log(teamA);

        if (gameObject.activeInHierarchy)
        {
            Debug.Log(timeval + "%%%%%%%%%%%%%%%%%%%%%%%%%");
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }
        isCount = true;

        StartCoroutine(SetFullCountryName());

    }

    public IEnumerator Timer(string timeString)
    {
        timeValSave = timeString;
        if (string.IsNullOrWhiteSpace(timeValSave)) yield break;
        string[] formats = { "dd/MM/yyyy HH:mm:ss" };
        var matchduration = DateTime.Parse(CommonFunctions.Instance.ChangeDateFormat(timeValSave, formats)) - DateTime.Now;

        var TimeDifference = matchduration;
        if (TimeDifference.Days * 24 + TimeDifference.Hours <= 0)
        {
            if (TimeDifference.Minutes <= 0 && TimeDifference.Seconds <= 0)
                time.text = "Live";
            else
                time.text = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
        }
        else
        {
            time.text = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer(timeString));

    }

}
