using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;

public class LiveMatchHandler : MonoBehaviour
{
    public static LiveMatchHandler Instance;

    public DatabaseReference database;

    [Header("More Livce Matches")]
    [SerializeField] private GameObject liveMatchCard;
    [SerializeField] private Transform parentLiveMatch;

    [Header("More Live Matches")]
    [SerializeField] private GameObject moreLiveMatchCard;
    [SerializeField] private Transform parentMoreLiveMatches;

    public List<MatchData> matches;

    private void Start()
    {
        Instance = this;

        database = FirebaseDatabase.DefaultInstance.RootReference;

        StartCoroutine(MatchDetails());
    }

    private IEnumerator MatchDetails()
    {
        //var task = FirebaseDatabase.DefaultInstance.GetReference("Matches").Child("Upcoming").GetValueAsync();
        var task = FirebaseDatabase.DefaultInstance.GetReference("Ram").Child("Live").GetValueAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception == null)
        {
            DataSnapshot dataSnapshot = task.Result;

            foreach (var item1 in dataSnapshot.Children)
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

       // matches.Sort((p1, p2) => (DateTime.Parse(p1.Time)).CompareTo(DateTime.Parse(p2.Time)));

        CreateLiveMatchBoard();
    }

    private void CreateLiveMatchBoard()
    {
        int count = (matches.Count > 3) ? 3 : matches.Count;

        if (matches.Count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject gameObject = Instantiate(liveMatchCard, parentLiveMatch);
                gameObject.GetComponent<LiveMatchController>().SetValueToLiveMatch(matches[i].Time, matches[i].MatchType, matches[i].TeamA, matches[i].TeamB);
                //gameObject.GetComponent<Transform>().Find("DateTxt").GetComponent<TMP_Text>().text = matches[i].Time;
                ////gameObject.GetComponent<Transform>().Find("TimeTxt").GetComponent<TMP_Text>().text = matches[i].MatchDateTime;
                //gameObject.GetComponent<Transform>().Find("T20Txt").GetComponent<TMP_Text>().text = matches[i].MatchType;
                //gameObject.GetComponent<Transform>().Find("TeamImage1").GetComponentInChildren<TMP_Text>().text = matches[i].TeamA;
                //gameObject.GetComponent<Transform>().Find("TeamImage2").GetComponentInChildren<TMP_Text>().text = matches[i].TeamB;
                gameObject.GetComponent<Transform>().Find("Live Toggle").GetComponent<Toggle>().isOn = matches[i].HotGame == "True" ? true : false;
            }
        }
    }

    public void MoreLiveMatchesDetails()
    {
        for (int i = 0; i < matches.Count; i++)
        {
            GameObject gameObject = Instantiate(moreLiveMatchCard, parentMoreLiveMatches);

            gameObject.GetComponent<Transform>().Find("SNo_TxtTmp").GetComponent<TMP_Text>().text = (i + 1).ToString();
            gameObject.GetComponent<Transform>().Find("date_Txt").GetComponent<TMP_Text>().text = matches[i].Time;
            gameObject.GetComponent<Transform>().Find("MatchID_Txt").GetComponent<TMP_Text>().text = matches[i].MatchId;
            gameObject.GetComponent<Transform>().Find("Team_Txt").GetComponent<TMP_Text>().text = matches[i].TeamA + " VS " + matches[i].TeamB;
            gameObject.GetComponent<Transform>().Find("Formats_Txt ").GetComponent<TMP_Text>().text = int.Parse(matches[i].MatchType) == 0 ? "T20" : "ODI";

            //gameObject.GetComponent<Transform>().Find("Team_Txt").GetComponent<TMP_Text>().text = ;
        }
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
