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
    public List<MatchPoolType> matchpool;
    public List<Player> players;

    private void OnApplicationQuit()
    {
        FirebaseDatabase.DefaultInstance.App.Dispose();
    }

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
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

        Debug.Log("************ MatchDetailsListner");
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

        Debug.Log("************ MatchDetailsListner");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }
        string val = args.Snapshot.GetRawJsonValue();
        Debug.Log(val);

        matchpool = JsonConvert.DeserializeObject<List<MatchPoolType>>(val);

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
        Debug.Log(val);

        players = JsonConvert.DeserializeObject<List<Player>>(val);

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

    #region MatchPools



    [Serializable]
    public class MatchPoolType
    {

        public int MatchID;
        public List<Pools> Pools = new List<Pools>();

    }

    [Serializable]
    public class Pools
    {
        public int Entry;
        public int PoolID;
        public List<Prizevalues> PrizeList = new List<Prizevalues>();
        public int PrizePool;
        public int SlotsFilled;
        public int TotalSlots;
        public string Type;
    }
 
    [Serializable]
    public class Prizevalues
    {
        public string Rank;
        public int Value;
    }

    #endregion

    #region Players

    [Serializable]
    public class Player
    {
        public List<PlayerDetails> Players;
        public string TeamName;
    }
    [Serializable]
    public class PlayerDetails
    {
        public int FPoint;
        public string ID;
        public string Name;
        public int Type;
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

