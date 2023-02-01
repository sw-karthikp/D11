using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Firebase.Storage;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase.Extensions;
using System;
using Firebase.Firestore;

public class GameController : MonoBehaviour 
{
    public static GameController Instance;

    [Header("CurrentUserDetails")]
    public UserDetails myData;

    [Header("CurrentUserID")]
    public string myUserID;
     // public string myName;

    [Header("CurrentMatchData")]
    public string CurrentTeamA;
    public string CurrentTeamB;
    public string CurrentMatchID;
    public string CurrentMatchTimeDuration;
    public string CurrentPoolID;
    public string CurrentPoolTypeName;
    string fileName;
    FirebaseStorage storage;
    StorageReference storageReference;
    FirebaseFirestore firestoredb;
    public MatchPools currentMatchpool;
    public Pools currentPools;
    public Action OnMatchPoolChanged,OnUserDataUpdated;



    [Header("ListDataFromRealDb")]
    public List<List<string>> itemsValue = new();
    public List<Player> players = new();


    [Header("DictionaryDataFromRealDb")]
    public Dictionary<string, Team> team = new();
    public Dictionary<string, Dictionary<string, MatchStatus>> match = new();
    public Dictionary<string, MatchPools> matchpool = new();
    public Dictionary<string, string> countryFullName = new();
    public Dictionary<string, Sprite> countrySpriteImage = new();
    public Dictionary<string, Sprite> playerSpriteImage = new();

    [Header("DictionaryDataGetFromRealDb")]
    public Dictionary<string, MatchID> selectedMatches = new();
    public Dictionary<string,Dictionary<string,Dictionary<string,string>>> _joinedPlayers = new();

    [Header("MyMatchesForGlobalReferanceFromDb")]
    public Dictionary<string, Dictionary<string, MatchStatus>> mymatchesGlobalRef = new Dictionary<string, Dictionary<string, MatchStatus>>();



    [Header("DictionaryDataGetFromRealDb")]
    public LiveMatchScoreCard scoreCard = new();
    public Dictionary<string, Color> color = new Dictionary<string, Color>() { { "AUS", new Color(0.62f,0.85f,1f,1) }, { "IND", new Color(1f,0.89f,0.61f,1)},
        { "PAK", new Color(0.32f,0.65f,0.52f,1) },{ "ENG",new Color(0.92f,0.34f,0.40f,1f) }};

    [Header("Referance")]
    public MyMatches mymatches;


    public List<CountryPic> countryPic;

    public List<PlayerPic> playerPic;

    DatabaseReference referenceRealDb; 

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
        referenceRealDb = FirebaseDatabase.DefaultInstance.RootReference;
        countrySpriteImage = new Dictionary<string, Sprite>();
        firestoredb = FirebaseFirestore.DefaultInstance;
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

        if (args.DatabaseError != null)
        {
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DebugHelper.Log(args.Snapshot.GetRawJsonValue());
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
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
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
        MainMenu_Handler.Instance.OnValueChange(0);
        MainMenu_Handler.Instance.OnValueChange(1);
        MainMenu_Handler.Instance.OnValueChange(2);
        MainMenu_Handler.Instance.OnValueChange(3);
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
        matchpool.Clear();
        if (args.DatabaseError != null)
        {
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DataSnapshot val = args.Snapshot;
        foreach (var item in val.Children)
        {
         
            matchpool.Add(item.Key, JsonConvert.DeserializeObject<MatchPools>(item.GetRawJsonValue()));

        }
        OnMatchPoolChanged?.Invoke();
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
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
            return;
        }


        string val = args.Snapshot.GetRawJsonValue();
        players = JsonConvert.DeserializeObject<List<Player>>(val);
        //FetchData();
        //FetchDataPlayerPic();
        SubscribeJoinedPlayerDetails();
    }

    #endregion

    #region PLAYERMATCHS
    public void SubscribeSelectedMatchDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference($"PlayerMatches/{myUserID}")
      .GetValueAsync().ContinueWithOnMainThread(task =>
      {
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
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DataSnapshot val = args.Snapshot;
        DebugHelper.Log(args.Snapshot.GetRawJsonValue());
        selectedMatches = JsonConvert.DeserializeObject<Dictionary<string, MatchID>>(val.GetRawJsonValue());
        mymatches.UpdateData();
        MyMatchData();
    }

    #endregion

    #region LIVEMATCHSCORE
    public void SubscribeLiveScoreDetails(string _matchID)
    {
        string id = _matchID;
        FirebaseDatabase.DefaultInstance
                 .GetReference($"LiveMatchScoreCard/{id}")
                 .ValueChanged += HandleLiveScoreMatch;
    }

    public void UnSubscribeLiveScoreDetails(string _matchID)
    {
        string id = _matchID;
        FirebaseDatabase.DefaultInstance
      .GetReference($"LiveMatchScoreCard/{id}")
      .ValueChanged -= HandleLiveScoreMatch;
        scoreCard = null;
    }


    void HandleLiveScoreMatch(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
            return;
        }
        DataSnapshot val = args.Snapshot;
        if (val.GetRawJsonValue() != null)
        {
            scoreCard = JsonConvert.DeserializeObject<LiveMatchScoreCard>(val.GetRawJsonValue());
            //ScoreCardPanel.Instance.InstantDataInnings1();
            //ScoreCardPanel.Instance.InstantDataInnings2();
           
                _My_Matches.Instance.Total1();
                _My_Matches.Instance.Total2();
            
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
                    DisplayImage(fileName, item2.TeamName);
                }
            }
        }
    }

    public IEnumerator LoadImage(string MediaUrl, string _teamName)
    {
        countrySpriteImage.Clear();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            DebugHelper.Log(request.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            countrySpriteImage.Add(_teamName, sprite);
        }
    }

    public void DisplayImage(string _fileName, string _teamName)
    {
        countrySpriteImage.Clear();
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
                DebugHelper.Log(task.Exception.ToString());
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
                DebugHelper.Log(fileName);
                DisplayImagePlayerPic(fileName, item1.ID);
            }

        }
    }
    public void DisplayImagePlayerPic(string _fileName, string _playerID)
    {

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
                DebugHelper.Log(task.Exception.ToString());
            }
        });
    }

    public IEnumerator LoadImagePlayerPic(string MediaUrl, string _playerID)
    {
        playerSpriteImage.Clear();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            DebugHelper.Log(request.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            playerSpriteImage.Add(_playerID, sprite);
        }
    }
    #endregion

    #region JOINEDPLAYERS
    public void SubscribeJoinedPlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("JoinedPlayers")
      .ValueChanged += JoinedPlayerValueChanged;
    }

    public void UnSubscribeJoinedPlayerDetails()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("JoinedPlayers")
      .ValueChanged -= JoinedPlayerValueChanged;
    }


    void JoinedPlayerValueChanged(object sender, ValueChangedEventArgs args)
    {
        //DebugHelper.Log(args.DatabaseError.ToString());
        if (args.DatabaseError != null)
        {
            DebugHelper.LogError(args.DatabaseError.Message + "*************");
            return;
        }

        DebugHelper.Log(args.Snapshot.GetRawJsonValue());
        _joinedPlayers = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(args.Snapshot.GetRawJsonValue());
     

    }
    #endregion

    #region SETLEADERBOARDPLAYERS

    public void writeNewUser(string userId,string key1, string key2)
    {
        Dictionary<string,object> boardData = new();
        boardData.Add("Name",GameController.Instance.myData.Name);
        boardData.Add("Value", "");
        referenceRealDb.Child("MatchPools").Child(key1).Child("Pools").Child(key2).Child("LeaderBoard").Child(GameController.Instance.myUserID).UpdateChildrenAsync(boardData);
        Debug.Log("caleed" + "***********************");
    }
    #endregion

    #region GETMYMATCHESSELECTEDDATA
    public void MyMatchData()
    {
        mymatchesGlobalRef.Clear();

        foreach (var item in GameController.Instance.selectedMatches)
        {

            foreach (var item2 in GameController.Instance.match)
            {
                foreach (var item3 in item2.Value)
                {
                    if (item3.Value.ID.ToString() == item.Key)
                    {
                        if (mymatchesGlobalRef.ContainsKey(item2.Key))
                        {
                            mymatchesGlobalRef[item2.Key].Add(item3.Key, item3.Value);
                        }
                        else
                        {
                            mymatchesGlobalRef.Add(item2.Key, new Dictionary<string, MatchStatus>() { { item3.Key, item3.Value } });
                        }

                    }
                }
            }
        }
    }
    #endregion

    #region UserDetails



    public void GetUserDetails()
    {
        if (string.IsNullOrWhiteSpace(GameController.Instance.myUserID)) return;
        
        DocumentReference docRef = firestoredb.Collection("users").Document(GameController.Instance.myUserID);
        docRef.Listen(snapshot =>
        {
            //DocumentSnapshot snapshot = task.Result;
            //Debug.Log($"<color=blue>{snapshot}</color>");
            if (snapshot.Exists)
            {
               myData = snapshot.ConvertTo<UserDetails>();
               OnUserDataUpdated?.Invoke();
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }


    public void AddAmount(int amount, int bonusAmount = 0, Action Onsuccess = null, Action onFailure = null)
    {

        //public float amount { get; set; }
        //public float bonusAmount { get; set; }
        //public float addedAmount { get; set; }
        //public float winningAmount { get; set; }

        DocumentReference docRef = firestoredb.Collection("users").Document($"{GameController.Instance.myUserID}");
       // Dictionary<string, object> obj = new();
        GameController.Instance.myData.Wallet.amount += (amount);
        GameController.Instance.myData.Wallet.addedAmount += amount;
        GameController.Instance.myData.Wallet.bonusAmount += bonusAmount;


        docRef.UpdateAsync("Wallet",GameController.Instance.myData.Wallet.ToDictionary()).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                onFailure?.Invoke();
            }
            else
            {
                Onsuccess?.Invoke();
            }
        });
     }

    public void SubtractAmount(int amount,int bonusAmount =0,Action Onsuccess = null,Action onFailure = null)
    {
        DocumentReference docRef = firestoredb.Collection("users").Document($"{GameController.Instance.myUserID}");

        GameController.Instance.myData.Wallet.amount -= (amount + bonusAmount);
        GameController.Instance.myData.Wallet.addedAmount -= amount;
        GameController.Instance.myData.Wallet.bonusAmount -= bonusAmount;

        docRef.UpdateAsync("Wallet", GameController.Instance.myData.Wallet.ToDictionary()).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {
                onFailure?.Invoke();
            }
            else
            {
                Onsuccess?.Invoke();
            }
        });
    }

    public void ShowLowbalanceScreen()
    {

    }

    #endregion

}

