using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using static UnityEditor.Progress;
using Newtonsoft.Json.Serialization;
using Unity.VisualScripting;
using TMPro;
using System;
using static UpcomingMatchHandler;
using UnityEngine.UI;

public class UpcomingMatchHandler : MonoBehaviour
{
    public static UpcomingMatchHandler Instance;

    public DatabaseReference database;

    private int dataCount;

    [Header("Main Menu Card")]
    [SerializeField] private GameObject upcomingToggle;
    [SerializeField] private Transform tranParent;

    [Header("More Upcoming Matches")]
    [SerializeField] private GameObject moreUpcomingMatchCard;
    [SerializeField] private Transform parentMoreUpcoming;

    public List<MatchData> matches;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        Instance = this;

        database = FirebaseDatabase.DefaultInstance.RootReference;

        //StartCoroutine(MatchDetails());
        SubscribePlayerDetails();
    }

    private IEnumerator MatchDetails()
    {
        //var task = FirebaseDatabase.DefaultInstance.GetReference("Matches").Child("Upcoming").GetValueAsync();
        var task = FirebaseDatabase.DefaultInstance.GetReference("Ram").Child("Upcoming").GetValueAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception == null)
        {
            DataSnapshot dataSnapshot = task.Result;
            
            foreach(var item1 in dataSnapshot.Children)
            {
                IDictionary data1 = (IDictionary)item1.Value;
                MatchData matchData = new MatchData();

                matchData.TeamA = data1["TeamA"].ToString();
                matchData.TeamB = data1["TeamB"].ToString();
                matchData.Time = data1["Time"].ToString();
                matchData.MatchId = data1["ID"].ToString();
                matchData.MatchType = data1["Type"].ToString();
                matchData.HotGame = data1["HotGame"].ToString();

                matches.Add(matchData);
            }
        }

        //matches.Sort((p1, p2) => (DateTime.Parse(p1.Time)).CompareTo(DateTime.Parse(p2.Time)));

        CreateUpcomingMatchBoard();
    }

    private void CreateUpcomingMatchBoard()
    {
        int count = (matches.Count > 3) ? 3 : matches.Count;

        if (matches.Count > 0)
        {
            for (int i = 0; i < count ; i++)
            {
                GameObject gameObject = Instantiate(upcomingToggle, tranParent);

                gameObject.GetComponent<Transform>().Find("DateTxt").GetComponent<TMP_Text>().text = matches[i].Time;
                //gameObject.GetComponent<Transform>().Find("TimeTxt").GetComponent<TMP_Text>().text = matches[i].MatchDateTime;
                gameObject.GetComponent<Transform>().Find("T20Txt").GetComponent<TMP_Text>().text = matches[i].MatchType;
                gameObject.GetComponent<Transform>().Find("TeamImage1").GetComponentInChildren<TMP_Text>().text = matches[i].TeamA;
                gameObject.GetComponent<Transform>().Find("TeamImage2").GetComponentInChildren<TMP_Text>().text = matches[i].TeamB;
                gameObject.GetComponent<Transform>().Find("Live Toggle").GetComponent<Toggle>().isOn = matches[i].HotGame == "True" ? true : false;
            }
        }
    }

    public void MoreUpcomingMatchesDetails()
    {
        for(int i = 0; i < matches.Count; i++)
        {
            GameObject gameObject = Instantiate(moreUpcomingMatchCard, parentMoreUpcoming);

            gameObject.GetComponent<Transform>().Find("SNo_TxtTmp").GetComponent<TMP_Text>().text = (i + 1).ToString();
            gameObject.GetComponent<Transform>().Find("date_Txt").GetComponent<TMP_Text>().text = matches[i].Time;
            gameObject.GetComponent<Transform>().Find("MatchID_Txt").GetComponent<TMP_Text>().text = matches[i].MatchId;
            gameObject.GetComponent<Transform>().Find("Team_Txt").GetComponent<TMP_Text>().text = matches[i].TeamA + " VS " + matches[i].TeamB;
            gameObject.GetComponent<Transform>().Find("Formats_Txt ").GetComponent<TMP_Text>().text = int.Parse(matches[i].MatchType) == 0 ? "T20" : "ODI";

            //gameObject.GetComponent<Transform>().Find("Team_Txt").GetComponent<TMP_Text>().text = ;
        }
    }

    public void SubscribePlayerDetails()
    {

        FirebaseDatabase.DefaultInstance
      .GetReference("Ram/Upcoming")
      .ValueChanged += HandleValueChanged;
    }

    public void UnSubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Ram/Upcoming")
      .ValueChanged -= HandleValueChanged;
    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        StartCoroutine(MatchDetails());
    }

    [Serializable]
    public class MatchData
    {
        public string TeamA;
        public string TeamB;
        public string MatchId;
        public string HotGame;
        public string Time;
        public string MatchType;
    }
}
