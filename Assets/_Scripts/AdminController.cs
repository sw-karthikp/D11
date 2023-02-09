using Firebase.Database;

using Newtonsoft.Json;

using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;
using System;


public class AdminController : MonoBehaviour
{

    public static AdminController Instance;
    public DatabaseReference database;

    [Header("----------------")]
    public List<TeamList> teamList = new List<TeamList>();
    public Dictionary<string, UpcomingMatchData> upcomingMatchData = new Dictionary<string, UpcomingMatchData>();
    public Dictionary<string, TeamPlayersListValue> newPlayerTeamListKeyobj = new();
    //public Dictionary<string, TeamList> newTeamList = new();
    public Dictionary<string,string> TeamFullName = new();



    public void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        SubscribePlayerDetails();
        TeamFullName = new Dictionary<string, string>() { { "AUS", "Australia" }, { "IND", "India" },
        { "PAK", "Pakistan" },{ "BAN", "Bangladesh" },{ "ENG", "England" },{ "NZ", "NewZealand" }
        ,{ "SL", "Sri Lanka" }};

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

        FirebaseDatabase.DefaultInstance.GetReference("Ram/Team").ValueChanged += TeamHandleValueChanged;
        FirebaseDatabase.DefaultInstance.GetReference("Ram/Upcoming").ValueChanged += UpcomingMatchValueChangeHandler;
    }

    public void UnSubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Ram/TeamPlayers")
      .ValueChanged -= HandleValueChanged;

        FirebaseDatabase.DefaultInstance.GetReference("Ram/Team").ValueChanged -= TeamHandleValueChanged;
        FirebaseDatabase.DefaultInstance.GetReference("Ram/Upcoming").ValueChanged -= UpcomingMatchValueChangeHandler;
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

        //foreach (var item in val.Children)
        //{
        //    foreach (var item1 in val.Children)
        //    {
        //        newPlayerTeamListKeyobj.Add(item1.Key, item1.Value);
        //    }
        //}

        foreach (var item in val.Children)
        {
            newPlayerTeamListKeyobj.Add(item.Key, JsonConvert.DeserializeObject<TeamPlayersListValue>(item.GetRawJsonValue()));
        }
    }

    private void TeamHandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("************ TeamDetailsListner");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DataSnapshot val = args.Snapshot;

        string json = val.GetRawJsonValue();

        teamList = (JsonConvert.DeserializeObject<List<TeamList>>(json));
    }

    /// <summary>
    /// -->> Upcoming Match Detail OnValueChange Handler... 
    /// </summary>
    
    private void UpcomingMatchValueChangeHandler(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("************ Upcoming Match OnValueHandler");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DataSnapshot val = args.Snapshot;

        string json = val.GetRawJsonValue();

        foreach(var item in val.Children)
        {
            upcomingMatchData.Add(item.Key, JsonConvert.DeserializeObject<UpcomingMatchData>(item.GetRawJsonValue()));
        }
    }
}


#region TeamPlayer
[Serializable]
    public class TeamPlayersListValue
    {
        public Dictionary<string,PlayersSubListValue> Players;
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
#endregion

#region TeamList
[Serializable]
public class TeamList 
{
    public string LogoIndex;
    public string LogoURL;
    public PlayerDetail PlayerDetails = new();
    public string TeamName;
}

[Serializable]
public class PlayerDetail
{
    public string T20;
    public string ODI;
    public string Test;
}

#endregion

#region Upcoming 

[Serializable]
public class UpcomingMatchData
{
    public string HotGame;
    public string ID;
    public string TeamA;    
    public string TeamB;
    public string Time;
    public string Type;
}
#endregion