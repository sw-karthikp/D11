using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using Firebase.Extensions;
using System.Threading.Tasks;
using Unity.VisualScripting;
using D11;

public class CompleteMatchHandler : MonoBehaviour
{
    public static CompleteMatchHandler Instance;

    public DatabaseReference database;

    [Header("More Livce Matches")]
    [SerializeField] private GameObject completeMatchCard;
    [SerializeField] private Transform parentCompleteMatch;

    [Header("More Complete Matches")]
    [SerializeField] private GameObject moreCompleteMatchCard;
    [SerializeField] private Transform parentMoreUpcoming;

    public List<MatchData> matches;
    //private string url;
    //private string teamBURL;

    private void Start()
    {
        Instance = this;

        database = FirebaseDatabase.DefaultInstance.RootReference;

        StartCoroutine(MatchDetails());

        //GetURL();
    }

    private IEnumerator MatchDetails()
    {
        //var task = FirebaseDatabase.DefaultInstance.GetReference("Matches").Child("Upcoming").GetValueAsync();
        var task = FirebaseDatabase.DefaultInstance.GetReference("Ram").Child("Complete").GetValueAsync();

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

        CreateCompleteMatchBoard();
    }

    private void CreateCompleteMatchBoard()
    {
        int count = (matches.Count > 3) ? 3 : matches.Count;

        if (matches.Count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject gameObject = Instantiate(completeMatchCard, parentCompleteMatch);
                gameObject.GetComponent<CompletMatchController>().SetCompleteMatchData(matches[i].HotGame, matches[i].MatchId, matches[i].TeamA, matches[i].TeamB, matches[i].Time, matches[i].MatchType, matches[i].TeamAURL, matches[i].TeamBURL);
                               
                //gameObject.GetComponent<Transform>().Find("DateTxt").GetComponent<TMP_Text>().text = matches[i].Time;
                ////gameObject.GetComponent<Transform>().Find("TimeTxt").GetComponent<TMP_Text>().text = matches[i].MatchDateTime;
                //gameObject.GetComponent<Transform>().Find("T20Txt").GetComponent<TMP_Text>().text = matches[i].MatchType;
                //gameObject.GetComponent<Transform>().Find("TeamImage1").GetComponentInChildren<TMP_Text>().text = matches[i].TeamA;
                ////gameObject.GetComponent<Transform>().Find("TeamImage1").GetComponent<Image>().sprite = 
                //gameObject.GetComponent<Transform>().Find("TeamImage2").GetComponentInChildren<TMP_Text>().text = matches[i].TeamB;
                ////gameObject.GetComponent<Transform>().Find("Live Toggle").GetComponent<Toggle>().isOn = matches[i].HotGame == "True" ? true : false;
            }
        }
    }

    public void MoreCompletMatchesDetails()
    {
        MorePageHandler.Instance.matchTitle.text = "Complete Matches";

        if (parentMoreUpcoming.childCount > 0)
        {
            foreach (Transform item in parentMoreUpcoming)
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < matches.Count; i++)
        {
            GameObject gameObject = Instantiate(moreCompleteMatchCard, parentMoreUpcoming);
            
            gameObject.GetComponent<Transform>().Find("SNo_TxtTmp").GetComponent<TMP_Text>().text = (i + 1).ToString();
            gameObject.GetComponent<Transform>().Find("date_Txt").GetComponent<TMP_Text>().text = matches[i].Time;
            gameObject.GetComponent<Transform>().Find("MatchID_Txt").GetComponent<TMP_Text>().text = matches[i].MatchId;
            gameObject.GetComponent<Transform>().Find("Team_Txt").GetComponent<TMP_Text>().text = matches[i].TeamA + " VS " + matches[i].TeamB;
            //gameObject.GetComponent<Transform>().Find("Formats_Txt ").GetComponent<TMP_Text>().text = int.Parse(matches[i].MatchType) == 0 ? "T20" : "ODI";

            //gameObject.GetComponent<Transform>().Find("Team_Txt").GetComponent<TMP_Text>().text = ;
        }
    }

    //private async Task<string> GetURL(string teamName)
    //{
    //    string url = null;

    //    await FirebaseDatabase.DefaultInstance.GetReference("Ram/Team").GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {

    //            DataSnapshot dataSnapshot = task.Result;

    //            foreach (var item in dataSnapshot.Children)
    //            {

    //                IDictionary data = (IDictionary)item.Value;

    //                if (data["TeamName"].ToString() == teamName)
    //                {
    //                    url = data["LogoURL"].ToString();
    //                    //Debug.Log($"123213123213213  {url}");
    //                }
    //            }
    //        }
    //    });

    //    return url;
    //}

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