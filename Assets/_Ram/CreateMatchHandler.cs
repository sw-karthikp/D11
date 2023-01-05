using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using System;

public class CreateMatchHandler : UIHandler
{
    //[Header("Team A")]
    //[SerializeField] private TMP_InputField teamAId;
    //[SerializeField] private TMP_InputField teamAName;

    //[Header("Team B")]
    //[SerializeField] private TMP_InputField teamBId;
    //[SerializeField] private TMP_InputField teamBName;

    //[Header("Other")]
    //[SerializeField] private TMP_InputField pricePool;
    //[SerializeField] private Toggle hotGame;
    //[SerializeField] private Button submit;

    [Header("Match Details")]
    [SerializeField] private TMP_InputField teamAName;
    [SerializeField] private TMP_InputField teamBName;
    [SerializeField] private Toggle hotGame;
    [SerializeField] private TMP_InputField matchType;
    [SerializeField] private TMP_InputField matchDate;
    [SerializeField] private TMP_InputField matchTime;

    // Firebase 
    public DatabaseReference database;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Getted in Last Match No 
    public void CreateNewMatch()
    {
        string matchId = "";

        FirebaseDatabase.DefaultInstance.GetReference("CommonValues").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception);
                return;
            }

            DataSnapshot dataSnapshot = task.Result;

            IDictionary data1 = (IDictionary)dataSnapshot.Value;

            if (string.IsNullOrEmpty(data1["LastMatchNo"].ToString()))
            {
                int inc = 0;
                matchId = inc.ToString();
            }
            else
            {
                int inc = int.Parse(data1["LastMatchNo"].ToString());
                inc++;
                matchId = inc.ToString();
            }

            Debug.Log("---------- Match Id " + matchId);

            StoreDataInFirebase(matchId);
          
        });
    }


    //Store data in Database
    private void StoreDataInFirebase(string id)
    {
        
        Debug.Log(id);
        MatchDetail matchDetail = new MatchDetail();
        //matchDetail.TEAMIDA = teamAId.text;
        matchDetail.TeamA = teamAName.text;
        //matchDetail.TEAMIDB = teamBId.text;
        matchDetail.TeamB = teamBName.text;
        //matchDetail.TopPricePool = pricePool.text;
        matchDetail.ID = id;
        matchDetail.HotGame = (hotGame.isOn ? true : false).ToString();
        matchDetail.Type = Convert.ToInt32(matchType.text);
        matchDetail.Time = matchDate.text + " " + matchTime.text;
        //matchDetail.MatchTime = matchTime.text;

        string json = JsonUtility.ToJson(matchDetail);
        string key = database.Child("Ram").Child("Upcoming").Push().Key;
        database.Child("Ram").Child("Upcoming").Child(key).SetRawJsonValueAsync(json);

        database.Child("CommonValues").Child("LastMatchNo").SetRawJsonValueAsync(id);
       
    }

    public class MatchDetail
    {
        //public string TEAMIDA;
        //public string TEAMIDB;
        public string TeamA;
        public string TeamB;
        //public string TopPricePool;
        public string ID;
        public string HotGame;
        public int Type;
        public string Time;
        //public string MatchTime;
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }
}
