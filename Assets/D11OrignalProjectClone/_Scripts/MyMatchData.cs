using D11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public TMP_Text time1;
    public Image[] Image;
    bool isCount = false;
    public string timeValSave;
    public string timeFormat;
    public int matchStatusID;
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
                                Time();
                                StopCoroutine(Timer(timeFormat));
                                StartCoroutine(Timer(timeFormat));
                                isCount = true;
                            }
                        }
                    }
                }
            }
        }
    }
    public void SetDetails(string teamAval, string teamBval, string id, string timeval, string _matchName, int _matchStatusID)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;
        matchStatusID = _matchStatusID;
        timeFormat = timeval;
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(SetFullCountryName());
        }

        if (gameObject.activeInHierarchy)
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
                                StopCoroutine(Timer(timeFormat));
                                StartCoroutine(Timer(timeFormat));
                                isCount = true;
                            }
                        }
                    }
                }
            }
        }

        foreach (var item in GameController.Instance.selectedMatches)
        {
            if(ID == item.Key)
            {
                Team.text = item.Value.SelectedTeam.Keys.Count.ToString();
                Contest.text = item.Value.SelectedPools.Keys.Count.ToString();
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
            string time1val = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Day.ToString();
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
            yield return new WaitForSeconds(0.1f);
            FireBaseManager.Instance.isFirstTime = false;
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

        foreach (var item1 in GameController.Instance.countryPic)
        {
            if (teamA.text == item1.Key)
            {
                Image[0].sprite = item1.pic;
            }
            else if (teamB.text == item1.Key)
            {
                Image[1].sprite = item1.pic;
            }
        }

        foreach (var item2 in GameController.Instance.color)
        {

            if (item2.Key == TeamA)
            {
                color1.color = item2.Value;
            }
            if (item2.Key == TeamB)
            {
                color2.color = item2.Value;
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
                        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(ID, TeamA, TeamB, valTimeSend));

                        //UIController.Instance.MainMenuScreen.HideMe();
                    }
                    if ((MatchTypeStatus)matchStatusID == MatchTypeStatus.Live)
                    {
                        UIController.Instance.mymatches.ShowMe();
                        GameController.Instance.SubscribeLiveScoreDetails(ID);
                        _My_Matches.Instance.SetDataToMyMatches(TeamA, TeamB, teamAFullName.text, teamBFullName.text, ID, valTimeSend);

                        // UIController.Instance.MainMenuScreen.HideMe();
                    }

                    if ((MatchTypeStatus)matchStatusID == MatchTypeStatus.Complete)
                    {
                        UIController.Instance.mymatches.ShowMe();
                        Debug.Log(ID + "#############################321");
                        GameController.Instance.SubscribeLiveScoreDetails(ID);
                        _My_Matches.Instance.SetDataToMyMatches(TeamA, TeamB, teamAFullName.text, teamBFullName.text, ID, valTimeSend);

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
        var matchduration = DateTime.ParseExact(timeFormat, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) - DateTime.Now;
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
            valTimeSend = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m" + " " +"Left";

        }
    

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer(timeString));

    }
}
