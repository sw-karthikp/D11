using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.Tilemaps;
using System;
using static UnityEditor.Progress;

public class AdminController : MonoBehaviour
{

    public static AdminController Instance;
    public DatabaseReference database;

    [Header("----------------")]

    public Dictionary<string, object> newPlayerTeamListKeyobj = new();




    public void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        SubscribePlayerDetails();

        
        //FirebaseDatabase.DefaultInstance.GetReference("Ram/TeamPlayers").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    DataSnapshot snapshot = task.Result;

        //    if (snapshot.Exists)
        //    {


        //    }

        //    else
        //    {
        //        Debug.Log("Team Players Needed to  Added By Admin User **************************");
        //    }
        //});
    }

    public void SubscribePlayerDetails()
    {

        FirebaseDatabase.DefaultInstance
      .GetReference("Ram/TeamPlayers")
      .ValueChanged += HandleValueChanged;
    }

    public void UnSubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Ram/TeamPlayers")
      .ValueChanged -= HandleValueChanged;
    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        newPlayerTeamListKeyobj.Clear();
        Debug.Log("************ MatchDetailsListner");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DataSnapshot val = args.Snapshot;

        foreach (var item in val.Children)
        {
            foreach (var item1 in val.Children)
            {
                newPlayerTeamListKeyobj.Add(item1.Key, item1.Value);
            }
        }

        



    }
}



    [Serializable]
    public class TeamPlayersListValue
    {
        public PlayersSubListValue Players;
        public string TeamName;

    }




    [Serializable]
    public class PlayersSubListValue
    {
        public string FPoint;
        public string ID;
        public string Name;
        public string Type;
        public string URL;
    }


