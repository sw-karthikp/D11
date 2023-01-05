using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public string CurrentTeamA;
    public string CurrentTeamB;
    public int CurrentMatchID;

    public List<Team> team;
    public Match match;
    public List<MatchPoolType> matchpool;
    public List<Player> players;

    private void Awake()
    {
        Instance = this;
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
        string val = args.Snapshot.GetRawJsonValue();
        Debug.Log(val);

        match = JsonConvert.DeserializeObject<Match>(val);
       StartCoroutine(MainMenu_Handler.Instance.SetUpcomingMatchDetails());
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

    #region Match

    [Serializable]
    public class Match
    {
        public List<MatchStatus> Completed = new();
        public List<MatchStatus> Live = new();
        public List<MatchStatus> Upcoming = new();
    }

    [Serializable]
    public class MatchStatus
    {
        public bool HotGame;
        public int ID;
        public string TeamA;
        public string TeamB;

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

}

