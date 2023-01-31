using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Firebase.Storage;
using UnityEngine.Networking;
using Firebase.Extensions;
using D11;
using Unity.VisualScripting;
using System.Net.Http.Headers;
using System.Globalization;

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
    public TMP_Text time1;
    public Image[] Image;
    bool isCount = false;
    public string timeValSave;
    public string timeFormat;
    public Image color1;
    public Image color2;
    string valTimeSend;
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
                    if (item1.Value.ID == ID)
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
                                Time();
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
    public void SetDetails(string teamAval, string teamBval, string id, string timeval, string _matchName)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;
        StartCoroutine(SetFullCountryName());
        timeFormat = timeval;
        foreach (var item in GameController.Instance.match)
        {

            foreach (var item1 in item.Value)
            {

                if (item1.Value.ID == ID)
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
                            Time();
                            StopCoroutine(Timer(timeval));
                            StartCoroutine(Timer(timeval));
                            isCount = true;
                        }
                 
                    }
                }
            }

        }
     
    }
    public void Time()
    {

        if (DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Day == DateTime.Now.Day)
        {
            string time = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay.ToString();
            string timeVal = DateTime.Parse(time).ToString("h:mm tt");
            time1.text = "Today," + " " + timeVal;
        }
        else if (DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Day == DateTime.Today.AddDays(1).Day)
        {
            string time = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay.ToString();
            string timeVal = DateTime.Parse(time).ToString("h:mm tt");
            time1.text = "Tomorrow," + " " + timeVal;
        }
        else
        {
            string time = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay.ToString();
            string timeVal = DateTime.Parse(time).ToString("h:mm tt");
            string time1val = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).DayOfYear.ToString();
            int month = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Month;
            string monthtext = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month);
            string timeVa1l = time1val + " " + monthtext;
            time1.text = timeVa1l + " " + timeVal;
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
            yield return new WaitForSeconds(3f);
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
        UIController.Instance.loading.SetActive(false);

        foreach (var item1 in GameController.Instance.color)
        {

            if (item1.Key == TeamA)
            {
                color1.color = item1.Value;
            }
            if (item1.Key == TeamB)
            {
                color2.color = item1.Value;
            }

        }
     
       
       
        
    }

        public void OnClickButton()
    {
        Debug.Log("called");
        foreach (var item in GameController.Instance.match)
        {
            foreach (var item1 in item.Value)
            {
                if (item1.Value.ID == ID)
                {
                    Debug.Log("callled" + ID);
                    if (item.Key == "Live")
                    {
                        Debug.Log("called * ");
                        GameController.Instance.SubscribeLiveScoreDetails(ID);
                        UIController.Instance.mymatches.ShowMe();
                        _My_Matches.Instance.SetDataToMyMatches(TeamA, TeamB, teamAFullName.text, teamBFullName.text, ID, valTimeSend);

                        return;
                    }
                     if (item.Key == "Upcoming")
                    {
                        Debug.Log("called **");
                        UIController.Instance.ContestPanel.ShowMe();

                        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(ID, TeamA, TeamB, valTimeSend));
                        return;
                    }

                     if (item.Key == "Complete")
                    {
                        Debug.Log("called ***");
                        UIController.Instance.mymatches.ShowMe();
                        GameController.Instance.SubscribeLiveScoreDetails(ID);
                        _My_Matches.Instance.SetDataToMyMatches(TeamA, TeamB, teamAFullName.text, teamBFullName.text, ID, valTimeSend);
                        return;

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
        DebugHelper.Log(teamA.text);

        if (gameObject.activeInHierarchy)
        {
            DebugHelper.Log(timeval + "%%%%%%%%%%%%%%%%%%%%%%%%%");
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
        var matchduration = DateTime.ParseExact(timeValSave, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) - DateTime.Now;
        var TimeDifference = matchduration;
        if (TimeDifference.Days * 24 + TimeDifference.Hours <= 0)
        {
            if (TimeDifference.Minutes <= 0 && TimeDifference.Seconds <= 0)
            {
                time.text = "Starting Soon";
                valTimeSend = "Starting Soon";
            }
            else
            {
                time.text = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
                valTimeSend = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s" + " " + "Left";
            }
        }
        else
        {
            time.text = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
            valTimeSend = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m" + " " + "Left";
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer(timeString));

    }

}
