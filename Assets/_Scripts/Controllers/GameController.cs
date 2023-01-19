using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Firebase.Storage;
using UnityEditor.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase.Extensions;
using System;
using static UnityEditor.Progress;

public class GameController : SerializedMonoBehaviour
{
    public static GameController Instance;

    [Header("CurrentUserID")]
    public string myUserID;

    [Header("CurrentMatchData")]
    public string CurrentTeamA;
    public string CurrentTeamB;
    public int CurrentMatchID;
    public string CurrentMatchTimeDuration;
    public string CurrentPoolID;
    public string CurrentPoolTypeName;
    string fileName;
    FirebaseStorage storage;
    StorageReference storageReference;


    [Header("ListDataFromRealDb")]
    public List<List<string>> itemsValue = new();
    public List<Player> players = new();


    [Header("DictionaryDataFromRealDb")]
    public Dictionary<string,Team> team = new();
    public Dictionary<string,Dictionary<string,MatchStatus>> match = new ();
    public Dictionary<string,MatchPools> matchpool = new ();
    public Dictionary<string,string> countryFullName = new();
    public Dictionary<string,Sprite> countrySpriteImage = new();
    public Dictionary<string,Sprite> playerSpriteImage = new();

    [Header("DictionaryDataSetFromRealDb")]
    public Dictionary<string,MyMatchDetails> mymatch = new();

    [Header("DictionaryDataGetFromRealDb")]
    public Dictionary<string, MatchID> selectedMatches = new();

   // public Dictionary<string, LiveMatchScoreCard> liveMatchData = new();

    [Header("DictionaryDataGetFromRealDb")]
    public LiveMatchScoreCard scoreCard;


    [Header("Referance")]
    public MyMatches mymatches;

    private void OnApplicationQuit()
    {
        FirebaseDatabase.DefaultInstance.App.Dispose();
    }

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 120;
        
    }
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        countrySpriteImage = new Dictionary<string, Sprite>();
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
        match.Clear();
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
        //FetchData();
        //FetchDataPlayerPic();
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

    #region PLAYERMATCHS
    public void SubscribeSelectedMatchDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference($"PlayerMatches/{myUserID}")
      .GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
              
          }
          else if (task.IsCompleted)
          {
              FirebaseDatabase.DefaultInstance
                 .GetReference($"PlayerMatches/{myUserID}")
                 .ValueChanged += HandleValueSelectedMatch;

          }
      });
   
    }

    public void UnSubscribeSelectedMatchDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("PlayerMatches")
      .ValueChanged -= HandleValueSelectedMatch;
    }


    void HandleValueSelectedMatch(object sender, ValueChangedEventArgs args)
    {

        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DataSnapshot val = args.Snapshot;
        Debug.Log(args.Snapshot.GetRawJsonValue());
        selectedMatches = JsonConvert.DeserializeObject<Dictionary<string, MatchID>>(val.GetRawJsonValue());
        mymatches.UpdateData();
    }

    #endregion

    #region LIVEMATCHSCORE
    public void SubscribeLiveScoreDetails(string _matchID)
    {
        FirebaseDatabase.DefaultInstance
                 .GetReference($"LiveMatchScoreCard/{_matchID}")
                 .ValueChanged += HandleLiveScoreMatch;
    }

    public void UnSubscribeLiveScoreDetails(string _matchID)
    {
        FirebaseDatabase.DefaultInstance
      .GetReference($"LiveMatchScoreCard/{_matchID}")
      .ValueChanged -= HandleLiveScoreMatch;
        scoreCard = null;
    }


    void HandleLiveScoreMatch(object sender, ValueChangedEventArgs args)
    {

        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        

        DataSnapshot val = args.Snapshot;
        Debug.Log(args.Snapshot.GetRawJsonValue());
        scoreCard = JsonConvert.DeserializeObject<LiveMatchScoreCard>(val.GetRawJsonValue());
        if(ScoreCardPanel.Instance.gameObject.activeSelf)
        {
            ScoreCardPanel.Instance.InstantDataInnings1();
            ScoreCardPanel.Instance.InstantDataInnings2();
        }
  
    }

    #endregion
    #region COUNTRY FLAGS

    public void FetchData()
    {
        foreach (var item in players)
        {
            foreach (var item2 in team.Values)
            {
                if (item.TeamName == item2.TeamName)
                {
                    fileName = item2.LogoURL;
                    DisplayImage(fileName,item2.TeamName);
                }
            }   
        }
    }

    public IEnumerator LoadImage(string MediaUrl, string _teamName)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            countrySpriteImage.Add(_teamName, sprite);          
        }
    }

    public void DisplayImage(string _fileName ,string _teamName)
    {
        countrySpriteImage.Clear();
        Debug.Log(fileName + "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        StorageReference image = storageReference.Child(_fileName);
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result), _teamName));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }



    #endregion

    #region PLAYERPICS
    public void FetchDataPlayerPic()
    {
        foreach (var item in players)
        {
            foreach (var item1 in item.Players.Values)
            {
                fileName = item1.URL;
                Debug.Log(fileName);
                DisplayImagePlayerPic(fileName, item1.ID);
            }
                 
        }
    }
    public void DisplayImagePlayerPic(string _fileName, string _playerID)
    {
        countrySpriteImage.Clear();
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        StorageReference image = storageReference.Child(_fileName);
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImagePlayerPic(Convert.ToString(task.Result), _playerID));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    public IEnumerator LoadImagePlayerPic(string MediaUrl, string _playerID)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            playerSpriteImage.Add(_playerID, sprite);
        }
    }
    #endregion

}

