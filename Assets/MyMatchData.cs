using D11;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class MyMatchData : MonoBehaviour
{
    public TMP_Text teamA;
    public TMP_Text teamB;
    public TMP_Text Match;
    public TMP_Text teamAFullName;
    public TMP_Text teamBFullName;
    public TMP_Text Team;
    public TMP_Text Contest;
    public string TeamA;
    public string TeamB;
    public string ID;
    public bool isPrimeGame;
    public Button Click;
    public TMP_Text time;
    public Image[] Image;
    bool isCount = false;
    public string timeValSave;
    public string timeFormat;
    public int matchStatusID;
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
        Invoke("setTimeAfterDelay", 0.2f);

    }

    public void setTimeAfterDelay()
    {
        if (isCount)
        {
            foreach (var item in GameController.Instance.match)
            {

                foreach (var item1 in item.Value)
                {
                    if (this.gameObject.name == item1.Value.ID)
                    {
                        if (item.Key == "Live")
                        {
                            time.text = "LIVE";

                        }
                        else if (item.Key == "Complete")
                        {
                            time.text = "0s";

                        }
                        else
                        {
                            if (gameObject.activeInHierarchy)
                            {
                                StopCoroutine(Timer(timeValSave));
                                StartCoroutine(Timer(timeValSave));
                                isCount = true;
                            }
                        }
                    }
                }

            }

        }
    }
    public void SetDetails(string teamAval, string teamBval, string id, string timeval, string _matchName,int _matchStatusID)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;
        matchStatusID = _matchStatusID;
        Debug.Log(_matchStatusID + "%%%%%%%%%%");

        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(SetFullCountryName());
        }

        if (gameObject.activeInHierarchy)
        {
            Debug.Log(timeval + "%%%%%%%%%%%%%%%%%%%%%%%%%");
            foreach (var item in GameController.Instance.match)
            {

                foreach (var item1 in item.Value)
                {
                    if (this.gameObject.name == item1.Value.ID) 
                    {
                        if (item.Key == "Live")
                        {
                            time.text = "LIVE";
                            isCount = true;
                        }
                        else if (item.Key == "Complete")
                        {
                            time.text = "0s";
                            isCount = true;
                        }
                        else
                        {
                            if (gameObject.activeInHierarchy)
                            {
                                StopCoroutine(Timer(timeValSave));
                                StartCoroutine(Timer(timeValSave));
                                isCount = true;
                            }
                        }
                    }
                }

            }



        }
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

        if (FireBaseManager.Instance.isFirstTime)
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
        foreach (var item in GameController.Instance.match)
        {

            foreach (var item1 in item.Value)
            {
                if (item1.Value.ID == ID)


                {


                    if ((MatchTypeStatus)matchStatusID == MatchTypeStatus.Upcoming)
                    {
                        UIController.Instance.ContestPanel.ShowMe();
                        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(ID, TeamA, TeamB, timeFormat));

                        //UIController.Instance.MainMenuScreen.HideMe();
                    }
                    if ((MatchTypeStatus)matchStatusID == MatchTypeStatus.Live)
                    {
                        UIController.Instance.mymatches.ShowMe();
                        GameController.Instance.SubscribeLiveScoreDetails(ID);
                        _My_Matches.Instance.SetDataToMyMatches(TeamA, TeamB, teamAFullName.text, teamBFullName.text, ID, timeFormat);

                        // UIController.Instance.MainMenuScreen.HideMe();
                    }

                    if ((MatchTypeStatus)matchStatusID == MatchTypeStatus.Complete)
                    {
                        UIController.Instance.mymatches.ShowMe();
                        Debug.Log(ID + "#############################321");
                        GameController.Instance.SubscribeLiveScoreDetails(ID);
                        _My_Matches.Instance.SetDataToMyMatches(TeamA, TeamB, teamAFullName.text, teamBFullName.text, ID, timeFormat);

                        // UIController.Instance.MainMenuScreen.HideMe();
                    }
                }
            }
        }
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
        var matchduration = DateTime.Parse(timeValSave) - DateTime.Now;

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
