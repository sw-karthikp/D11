using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using Unity.VisualScripting;
using Newtonsoft.Json;
using static AddNewTeamPanelHandler;
using System;
using Google.MiniJSON;
using UnityEngine.Networking;

public class AddNewPlayerHandler : UIHandler
{
    public DatabaseReference database;

    [Header("For team select")]
    [SerializeField] private GameObject playerProfileDisplayPrefab;
    [SerializeField] private Transform playerProfileDisplayParent;

    [SerializeField] private GameObject teamNameDisplayPrefab;
    [SerializeField] private Transform teamNameDisplayParent;

    [SerializeField] private string selectTeamNameIs;
    [SerializeField] private string matchType;
    [SerializeField] private string cricketType;

    [SerializeField] List<Sprite> teamNameLabel;

    [Header("Player Details Input")]
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private string playerType;
    [SerializeField] private TMP_InputField playerRun;
    [SerializeField] private TMP_InputField playerID;

    private int teamPlayerCount;

    [Header("Player Profile Display")]
    [SerializeField] private GameObject profileDisplay;
    [SerializeField] private Transform profileDisplayParent;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void CreateTeamPlayer(string teamCount, string teamPlayerCount)
    {
        FirebaseDatabase.DefaultInstance.GetReference("Ram/TeamPlayers").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                foreach(var item in dataSnapshot.Children)
                {
                    string teamIndex = item.Key;

                    IDictionary data = (IDictionary)item.Value;
                    
                    if (data["TeamName"].ToString() == selectTeamNameIs)
                    {
                        Debug.Log(" Team data is already existed...");

                        Dictionary<string, object> newData = new Dictionary<string, object>();
                        newData.Add("FPoint", playerRun.text);
                        newData.Add("Name", playerName.text);
                        newData.Add("ID", selectTeamNameIs + teamPlayerCount);
                        newData.Add("Type", playerType);
                        newData.Add("URL", PlayerProfile.instance.path);

                        database.Child("Ram").Child("TeamPlayers").Child(teamIndex).Child("Players").Push().UpdateChildrenAsync(newData);

                        Dictionary<string, object> updateData = new Dictionary<string, object>();
                        updateData.Add(selectTeamNameIs, teamPlayerCount);

                        database.Child("CommonValues").Child("TeamPlayers").UpdateChildrenAsync(updateData);

                        Debug.Log($"data updated------ ");
                        GetTeamPlayersProfile();
                        return;
                    }
                }

                Team team = new Team();
                team.TeamName = selectTeamNameIs;
                team.Players = null;

                string json = JsonUtility.ToJson(team); 

                Dictionary<string, object> newData2 = new Dictionary<string, object>();
                newData2.Add("FPoint", playerRun.text);
                newData2.Add("Name", playerName.text);
                newData2.Add("ID", selectTeamNameIs + teamPlayerCount);
                newData2.Add("Type", playerType);
                newData2.Add("URL", PlayerProfile.instance.path);

                database.Child("Ram").Child("TeamPlayers").Child(teamCount).SetRawJsonValueAsync(json);
                database.Child("Ram").Child("TeamPlayers").Child(teamCount).Child("Players").Push().UpdateChildrenAsync(newData2);
                
                Dictionary<string, object> updateData2 = new Dictionary<string, object>();
                updateData2.Add(selectTeamNameIs, teamPlayerCount);

                database.Child("CommonValues").Child("TeamPlayers").UpdateChildrenAsync(updateData2);
                database.Child("CommonValues").Child("TeamPlayerCount").SetRawJsonValueAsync(teamCount);
                Debug.Log($"New Data Created ========= ");
          
                GetTeamPlayersProfile();
            }
        });
    }

    public void GetTeamPlayerIndexAndCreateData()
    {
        int teamPlayerTeamCount = 0;

        FirebaseDatabase.DefaultInstance.GetReference("CommonValues").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                foreach(var item1 in dataSnapshot.Children)
                {
                    if (item1.Key == "TeamPlayerCount")
                    {
                        if (string.IsNullOrEmpty(item1.Value.ToString()))
                        {
                            teamPlayerTeamCount = 0;
                        }
                        else 
                        {
                            teamPlayerTeamCount = int.Parse(item1.Value.ToString());
                            teamPlayerTeamCount++;
                        }  
                    }

                    if (item1.Key == "TeamPlayers")
                    {
                        IDictionary data = (IDictionary)item1.Value;

                        if (string.IsNullOrEmpty(data[selectTeamNameIs].ToString()))
                        {
                            teamPlayerCount = 0;
                        }
                        else
                        {
                            teamPlayerCount = Convert.ToInt32(data[selectTeamNameIs]);
                            teamPlayerCount++;
                        }

                        CreateTeamPlayer(teamPlayerTeamCount.ToString(), teamPlayerCount.ToString());
                    }
                }
            }
        });
    }

    private void GetTeamPlayersProfile()
    {
        Debug.Log($"12345 Entering GetTeamPlayersProfile");

        FirebaseDatabase.DefaultInstance.GetReference("Ram/TeamPlayers").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                foreach(var item in dataSnapshot.Children)
                {
                    foreach(var item2 in item.Children)
                    { 
                        if (item2.Value.ToString() == selectTeamNameIs)
                        {
                            foreach (var item3 in item.Children)
                            {
                                if(item3.Key == "Players")
                                {
                                    int totalTeams = Convert.ToInt32(item3.ChildrenCount);
                                    Debug.Log("12345 total teams" + totalTeams);

                                    foreach (var item4 in item3.Children)
                                    {
                                        IDictionary data = (IDictionary)item4.Value;
                                        string url = data["URL"].ToString();

                                        bool alreadySpawned = false;

                                        Debug.Log($" 12345 key name is {item4.Key}");

                                        if (profileDisplayParent.childCount != 0)
                                        {
                                            for (int i = 0; i < profileDisplayParent.childCount; i++)
                                            {
                                                if (profileDisplayParent.GetChild(i).name == item4.Key)
                                                {
                                                    Debug.Log($" 12345 This profile is already created {item4.Key}");
                                                    alreadySpawned = true;
                                                }
                                            }

                                            if (!alreadySpawned)
                                            {
                                                GameObject gameObject = Instantiate(profileDisplay, profileDisplayParent);
                                                gameObject.name = item4.Key;
                                                StartCoroutine(PlayerProfile.instance.DisplayPlayerProfile(url, gameObject));

                                                Debug.Log("12345 Here calling to profile loading...");
                                            }
                                        }
                                        else
                                        {
                                            GameObject gameObject1 = Instantiate(profileDisplay, profileDisplayParent);
                                            gameObject1.name = item4.Key;
                                            StartCoroutine(PlayerProfile.instance.DisplayPlayerProfile(url, gameObject1));

                                            Debug.Log("12345 Here calling to profile loading...");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }

    private void GetTeamDataAndDisplay()
    {
        string existTeamName = " ";

        FirebaseDatabase.DefaultInstance.GetReference("Ram/Team").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                foreach(var item in dataSnapshot.Children)
                {
                    IDictionary data = (IDictionary)item.Value;

                    existTeamName = data["TeamName"].ToString();

                    foreach (var item1 in item.Children)
                    {
                        if (item1.Key == "PlayerDetails")
                        {
                            foreach (var item2 in item1.Children)
                            {
                                if (item2.Key == matchType)
                                {
                                    GameObject gameObject = Instantiate(teamNameDisplayPrefab, teamNameDisplayParent);
                                    gameObject.name = existTeamName;
                                    gameObject.GetComponent<Toggle>().isOn = false;
                                    gameObject.GetComponent<Toggle>().group = teamNameDisplayParent.GetComponent<ToggleGroup>();
                                    gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = existTeamName;

                                    gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckOnValueChanged(gameObject.GetComponent<Toggle>(), gameObject.name); });
                                }
                            }
                        }
                    }
                }
             }
            
        });
    }

    public void CheckOnValueChanged(Toggle toggle, string teamName) 
    {
        if (toggle.isOn)
        {
            selectTeamNameIs = teamName;

            if(selectTeamNameIs != null && matchType != null && cricketType != null)
            {
                GetTeamPlayersProfile();
            }
        }
        else
        {
            selectTeamNameIs = null;

            for(int  i = 0; i < profileDisplayParent.childCount; i++)
            {
                Destroy(profileDisplayParent.transform.GetChild(i).gameObject);
            }
        }

    }

    public void SelectedMatchTypeIs(GameObject gameObject)
    {
        if (gameObject.transform.GetComponent<Toggle>().isOn)
        {
            matchType = gameObject.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text;

            if (!string.IsNullOrEmpty(matchType) && !string.IsNullOrEmpty(cricketType))
            {
                GetTeamDataAndDisplay();
            }
        }
        else
        {
            matchType = null;

            DestroyDatas();
        }
    }

    public void SelectedCricketTypeIs(GameObject gameObject)
    {
        if (gameObject.transform.GetComponent<Toggle>().isOn)
        {
            cricketType = gameObject.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text;

            if (!string.IsNullOrEmpty(matchType) && !string.IsNullOrEmpty(cricketType))
            {
                GetTeamDataAndDisplay();
            }
        }
        else
        {
            cricketType = null;

            DestroyDatas();
        }
    }

    private void DestroyDatas()
    {
        for (int i = 0; i < teamNameDisplayParent.childCount; i++)
        {
            Destroy(teamNameDisplayParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerProfileDisplayParent.childCount; i++)
        {
            Destroy(playerProfileDisplayParent.GetChild(i).gameObject);
        }
    }

    public void PlayerTypeIsChanged(GameObject gameObject)
    {
        int num = gameObject.GetComponent<TMP_Dropdown>().value;
        playerType = gameObject.GetComponent<TMP_Dropdown>().options[num].text;
    }

    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }

    public override void OnBack()
    {
       
    }

    [Serializable]
    public class Team
    {
        public string Players;
        public string TeamName;
    }
}
