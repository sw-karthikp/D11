using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using static UnityEditor.Progress;

public class GameController : SerializedMonoBehaviour
{

    public static GameController Instance;
    public string CurrentTeamA;
    public string CurrentTeamB;
    public int CurrentMatchID;
    public string CurrentMatchTimeDuration;
    public string CurrentPoolID;
    public List<Team> team;
    public Dictionary<string,Dictionary<string,MatchStatus>> match = new ();
    public Dictionary<string,MatchPools> matchpool = new ();
    public Dictionary<string,Player> players = new();
    public Dictionary<string, Sprite> flags = new Dictionary<string, Sprite>();
    public List<Sprite> flagsval= new List<Sprite>();
    public Dictionary<string, string> countryFullName = new Dictionary<string, string>();
    private void OnApplicationQuit()
    {
        FirebaseDatabase.DefaultInstance.App.Dispose();
    }

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        countryFullName = new Dictionary<string, string>() { { "AUS", "Australia" }, { "IND", "India" },
        { "PAK", "Pakistan" },{ "BAN", "Bangladesh" },{ "ENG", "England" },{ "NZ", "NewZealand" }
        ,{ "SL", "Sri Lanka" }};

    }
    public void SubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Teams")
      .ValueChanged += HandleValueChanged;
    }

    public void UnSubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Teams")
      .ValueChanged -= HandleValueChanged;
    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {

        Debug.Log("************ MatchDetailsListner");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        string val = args.Snapshot.GetRawJsonValue();


        team = JsonConvert.DeserializeObject<List<Team>>(val);
    }


    public void SubscribeMatchDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Matches")
      .ValueChanged += HandleValueMatch;
    }

    public void UnSubscribeMatchDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Matches")
      .ValueChanged -= HandleValueMatch;
    }


    void HandleValueMatch(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }
        DataSnapshot val = args.Snapshot;
        Debug.Log(val.GetRawJsonValue());
        foreach (var item in val.Children)
        {
            Dictionary<string, MatchStatus> matchnew = new();
            foreach (var item1 in item.Children)
            {
                matchnew.Add(item1.Key, JsonConvert.DeserializeObject<MatchStatus>(item1.GetRawJsonValue()));
            }
            match.Add(item.Key, matchnew);
        }
        MainMenu_Handler.Instance.OnvalueChangeT20();
        MainMenu_Handler.Instance.OnvalueChangeODI();
        MainMenu_Handler.Instance.OnvalueChangeTEST();
        MainMenu_Handler.Instance.OnvalueChangeT10();
    }

    public void SubscribeMatchPools()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("MatchPools")
      .ValueChanged += HandleValueMatchPools;
    }

    public void UnSubscribeMatchPools()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("MatchPools")
      .ValueChanged -= HandleValueMatchPools;
    }


    void HandleValueMatchPools(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }
        DataSnapshot val = args.Snapshot;
        foreach (var item in val.Children)
        {
            matchpool.Add(item.Key,JsonConvert.DeserializeObject<MatchPools>(item.GetRawJsonValue()));
        }
    }


    public void SubscribePlayers()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("TeamPlayers")
      .ValueChanged += HandleValuePlayers;
    }

    public void UnSubscribePlayers()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("TeamPlayers")
      .ValueChanged -= HandleValuePlayers;
    }


    void HandleValuePlayers(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("************ MatchDetailsListner");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

       string val = args.Snapshot.GetRawJsonValue();
      // players = JsonConvert.DeserializeObject<List<Player>>(val);

        foreach (var item in args.Snapshot.Children)
        {
            players.Add(item.Key, JsonConvert.DeserializeObject<Player>(item.GetRawJsonValue()));
        }

    }

    #region Team

    [Serializable]
    public class Team
    {
        public string ID;
        public string Name;
        public TeamType TEAMS = new();
    }

    [Serializable]
    public class TeamType
    {
        public string ODI;
        public string T20;
        public string TEST;
    }

    #endregion

    #region SetPlayerDetailsRealDb
    public class MatchDeatils
    {
        public List<string> Matches = new();
        public PoolDetails PoolDetail;

    }

    public class PoolDetails
    {
        public List<MatchDeatilValues> PoolDetailsval = new();

    }


    public class MatchDeatilValues 
    {
        public int MatchDetail;
        public int TeamDetails;
    }

    public class TeamDetailsValues 
    {
        public string TeamId;
        public List<Playerscls> Players = new();
    }


    public class Playerscls 
    {
        public string Captain;
        public string VCaptain;
        public List<Teams> TeamA;
        public List<Teams> TeamB;
    }

    public class Teams
    {
        public string TeamName;
        public List<Players> Players = new();

    }

    public class Players
    {
        public string playerID;
    }

    #endregion

}

