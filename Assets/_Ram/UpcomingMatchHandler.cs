using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

using Newtonsoft.Json.Serialization;
using Unity.VisualScripting;
using TMPro;
using System;
using static UpcomingMatchHandler;
using UnityEngine.UI;
using D11;

public class UpcomingMatchHandler : MonoBehaviour
{
    public static UpcomingMatchHandler Instance;

    public DatabaseReference database;

    private int dataCount;

    [Header("Main Menu Card")]
    [SerializeField] private GameObject createUpcomingToggle;
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
                
                foreach (var item3 in AdminController.Instance.teamList)
                {
                    if (item3.TeamName == data1["TeamA"].ToString())
                    {
                        matchData.TeamAURL = item3.LogoURL;
                    }
                    else if (item3.TeamName == data1["TeamB"].ToString())
                    {
                        matchData.TeamBURL = item3.LogoURL;
                    }
                }
                matchData.Time = data1["Time"].ToString();
                matchData.MatchId = data1["ID"].ToString();
                matchData.MatchType = data1["Type"].ToString();
                matchData.HotGame = data1["HotGame"].ToString();

                matches.Add(matchData);
            }
        }

        string[] format = { "dd/MM/yyyy hh:mm:ss", "dd-MM-yyyy hh:mm:ss" };
        matches.Sort((p1, p2) => (DateTime.Parse(CommonFunctions.Instance.ChangeDateFormat(p1.Time, format)).CompareTo(DateTime.Parse(CommonFunctions.Instance.ChangeDateFormat(p2.Time, format)))));

        CreateUpcomingMatchBoard();
    }

    private void CreateUpcomingMatchBoard()
    {
        if (tranParent.childCount > 3) return;

        int count = (matches.Count > 3) ? 3 : matches.Count;

        if (matches.Count > 0)
        {
            if(matches.Count >= 3)
            {
                GameObject createToggle = Instantiate(createUpcomingToggle, tranParent);
                createToggle.GetComponent<Button>().onClick.AddListener( delegate {  CreateMatchHandler.instance.ShowMe();  });

                for (int i = 0; i < 2; i++)
                {
                    GameObject gameObject = Instantiate(upcomingToggle, tranParent);
                    gameObject.GetComponent<UpcomingMatchController>().SetUpcomingMatchData(matches[i].HotGame, matches[i].MatchId, matches[i].TeamA, matches[i].TeamB, matches[i].Time, matches[i].MatchType, matches[i].TeamAURL, matches[i].TeamBURL);
                }
            }
            else
            {   
                for (int i = 0; i < (3 - matches.Count); i++)
                {
                    GameObject gameObject = Instantiate(createUpcomingToggle, tranParent);
                }

                int j = 0;
                for (int i = tranParent.childCount; i < 3; i++)
                {
                    GameObject gameObject = Instantiate(upcomingToggle, tranParent);
                    gameObject.GetComponent<UpcomingMatchController>().SetUpcomingMatchData(matches[j].HotGame, matches[j].MatchId, matches[j].TeamA, matches[j].TeamB, matches[j].Time, matches[j].MatchType, matches[j].TeamAURL, matches[j].TeamBURL);
                    j++;
                }
            }
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                GameObject gameObject = Instantiate(createUpcomingToggle, tranParent);
            }
        }
    }

    public void MoreUpcomingMatchesDetails()
    {
        MorePageHandler.Instance.matchTitle.text = "Upcoming Matches";

        if (parentMoreUpcoming.childCount > 0)
        {
            foreach (Transform item in parentMoreUpcoming)
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < matches.Count; i++)
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
        public string TeamAURL;
        public string TeamBURL;
    }
}
