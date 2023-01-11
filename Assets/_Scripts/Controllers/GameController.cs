using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

public class GameController : SerializedMonoBehaviour
{
    public static GameController Instance;

    [Header("CurrentMatchData")]
    public string CurrentTeamA;
    public string CurrentTeamB;
    public int CurrentMatchID;
    public string CurrentMatchTimeDuration;
    public string CurrentPoolID;
    
    [Header("ListDataFromRealDb")]
    public List<List<string>> itemsValue = new();
    public List<Player> players = new();


    [Header("DictionaryDataFromRealDb")]
    public Dictionary<string,Team> team = new();
    public Dictionary<string,Dictionary<string,MatchStatus>> match = new ();
    public Dictionary<string,MatchPools> matchpool = new ();
    public Dictionary<string, string> countryFullName = new Dictionary<string, string>();

    [Header("DictionaryDataSetFromRealDb")]
    public Dictionary<string, MyMatchDetails> mymatch = new Dictionary<string, MyMatchDetails>();

    private void OnApplicationQuit()
    {
        FirebaseDatabase.DefaultInstance.App.Dispose();
    }

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
        
    }

    #region TEAM
    public void SubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Team")
      .ValueChanged += HandleValueChanged;
    }

    public void UnSubscribePlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Team")
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

        Debug.Log(args.Snapshot.GetRawJsonValue());
        foreach (var item in args.Snapshot.Children)
        {
            team.Add(item.Key, JsonConvert.DeserializeObject<Team>(item.GetRawJsonValue()));
        }
   
    }

    #endregion

    #region MATCH
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

    #endregion

    #region MATCHPOOLS
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

    #endregion

    #region TEAMPLAYERS
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
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

  
        string val = args.Snapshot.GetRawJsonValue();
        players = JsonConvert.DeserializeObject<List<Player>>(val);
    

    }

    #endregion

    #region LEADERBOARD
    public void SubscribeLeaderBoardDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Leader")
      .ValueChanged += HandleLeaderBoardValueChanged;
    }

    public void UnSubscribeLeaderBoardDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("Leader")
      .ValueChanged -= HandleLeaderBoardValueChanged;
    }


    void HandleLeaderBoardValueChanged(object sender, ValueChangedEventArgs args)
    {
        itemsValue = new List<List<string>>();
        Debug.Log("************ MatchDetailsListner");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        string val = args.Snapshot.GetRawJsonValue();

        Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        itemsValue = JsonConvert.DeserializeObject<List<List<string>>>(val);
        Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        //   WinnerLeaderBoard.Instance.OnValueChangeLeaderBord();
    }

    #endregion


}

