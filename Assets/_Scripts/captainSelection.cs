using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;
using D11;
using JetBrains.Annotations;

public class captainSelection : UIHandler
{

    public GameObject childPrefab;
    public Transform[] parent;
    public List<Toggle> togscaptain;
    public List<Toggle> togsvcaptain;
    public TMP_Text contest;
    public static captainSelection Instance;
    public ConfrmationHandler conformHandler;

    string slotKey1;
    string slotKey2;
    int slotsFilled;
    private void Awake()
    {
        Instance = this;

    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        for (int i = 0; i < togscaptain.Count; i++)
        {
            togscaptain[i].isOn = false;
            togsvcaptain[i].isOn = false;
        }


    }

    public override void OnBack()
    {

    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
        for (int i = 0; i < parent.Length; i++)
        {
            foreach (Transform child in parent[i])
            {
                Destroy(child.gameObject);
            }
        }

    }


    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(MatchSelection.Instance.playersForTeam, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        Player teamAplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamA);
        Player teamBplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamB);

        SelectedTeamPlayers selectedTeamA = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamA };
        SelectedTeamPlayers selectedTeamB = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamB };
        FirebaseDatabase mDatabase = FirebaseDatabase.DefaultInstance;
        string Captain = teamAplayers.Players.First().Value.ID;
        string viceCaptain = teamAplayers.Players.Last().Value.ID;
        string TeamId = "Team" + (GameController.Instance.selectedMatches.Count > 0 ? GameController.Instance.selectedMatches.ContainsKey(GameController.Instance.CurrentMatchID.ToString()) ? GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID.ToString()].SelectedTeam.Count + 1 : 1 : 1);
        string poolId = GameController.Instance.CurrentPoolID;
        selectedTeamA.players.Clear();
        selectedTeamB.players.Clear();
        foreach (var item in MatchSelection.Instance.playersForTeam)
        {
            if (item.isCaptain) Captain = item.PlayerID;
            if (item.isViceCaptain) viceCaptain = item.PlayerID;



            foreach (var item1 in teamAplayers.Players.Values)
            {
                if (item1.ID.Contains(item.PlayerID))
                {

                    selectedTeamA.players.Add(item.PlayerID);
                    break;
                }
            }

            foreach (var item2 in teamBplayers.Players.Values)
            {
                if (item2.ID.Contains(item.PlayerID))
                {

                    selectedTeamB.players.Add(item.PlayerID);
                    break;
                }
            }
            DebugHelper.Log(selectedTeamA.players.Count + "$$$$$$$$$" + selectedTeamB.players.Count);

        }
        SetPlayerNameToLeaderBaord();
        GetKeyForSlotFill();
        SelectedPlayers selectedPlayers = new SelectedPlayers() { Captain = Captain, ViceCaptian = viceCaptain, TeamA = selectedTeamA, TeamB = selectedTeamB };
        SelectedTeam selectedTeam = new SelectedTeam() { Players = selectedPlayers };
        SelectedPoolID selectedPool = new SelectedPoolID() { PoolID = poolId, TeamID = TeamId };
        string playerId = GameController.Instance.myUserID;
        DebugHelper.Log(playerId);
        string selectedPoolKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Push().Key;
        string selectedTeamKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Push().Key;
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Child($"{selectedPoolKey}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedPool));
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Child($"{TeamId}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedTeam));
        string val1 = GameController.Instance.CurrentMatchID.ToString();
        string val2 = GameController.Instance.CurrentPoolID.ToString();
        mDatabase.RootReference.Child("JoinedPlayers").Child($"{val1}").Child($"P{val2}").Child(GameController.Instance.myUserID.ToString()).SetValueAsync(GameController.Instance.myData.Name);
        mDatabase.RootReference.Child("MatchPools").Child(slotKey1).Child("Pools").Child(slotKey2).Child("SlotsFilled").SetValueAsync(slotsFilled + 1);
        GameController.Instance.SubscribeSelectedMatchDetails();
        BottomHandler.Instance.ResetScreen();
    }

     public void saveTeam()
    {
        string json = JsonConvert.SerializeObject(MatchSelection.Instance.playersForTeam, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        Player teamAplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamA);
        Player teamBplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamB);

        SelectedTeamPlayers selectedTeamA = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamA };
        SelectedTeamPlayers selectedTeamB = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamB };
        FirebaseDatabase mDatabase = FirebaseDatabase.DefaultInstance;
        string Captain = teamAplayers.Players.First().Value.ID;
        string viceCaptain = teamAplayers.Players.Last().Value.ID;
        string TeamId = "Team" + (GameController.Instance.selectedMatches.Count > 0 ? GameController.Instance.selectedMatches.ContainsKey(GameController.Instance.CurrentMatchID.ToString()) ? GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID.ToString()].SelectedTeam.Count + 1 : 1 : 1);
        string poolId = GameController.Instance.CurrentPoolID;
        selectedTeamA.players.Clear();
        selectedTeamB.players.Clear();
        foreach (var item in MatchSelection.Instance.playersForTeam)
        {
            if (item.isCaptain) Captain = item.PlayerID;
            if (item.isViceCaptain) viceCaptain = item.PlayerID;



            foreach (var item1 in teamAplayers.Players.Values)
            {
                if (item1.ID.Contains(item.PlayerID))
                {

                    selectedTeamA.players.Add(item.PlayerID);
                    break;
                }
            }

            foreach (var item2 in teamBplayers.Players.Values)
            {
                if (item2.ID.Contains(item.PlayerID))
                {

                    selectedTeamB.players.Add(item.PlayerID);
                    break;
                }
            }
            DebugHelper.Log(selectedTeamA.players.Count + "$$$$$$$$$" + selectedTeamB.players.Count);

        }
        SelectedPlayers selectedPlayers = new SelectedPlayers() { Captain = Captain, ViceCaptian = viceCaptain, TeamA = selectedTeamA, TeamB = selectedTeamB };
        SelectedTeam selectedTeam = new SelectedTeam() { Players = selectedPlayers };
        SelectedPoolID selectedPool = new SelectedPoolID() { PoolID = poolId, TeamID = TeamId };
        string playerId = GameController.Instance.myUserID;
        DebugHelper.Log(playerId);
        string selectedTeamKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Push().Key;
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Child($"{TeamId}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedTeam));

      

        BottomHandler.Instance.ResetScreen();
    }


    public void onClickSave()
    {
        if(ContestHandler.Instance.isCreateTeam)
        {
            saveTeam();
            HideMe();
        }
        else
        {
            conformHandler.ShowMe();
        }
     
    }


    public void SetPlayerNameToLeaderBaord()
    {
        string firstKey="";
        string secondKey="";
        Debug.Log("Callled ^^^^^^^^^^");
        foreach (var item in GameController.Instance.matchpool)
        {
            if (GameController.Instance.CurrentMatchID == item.Value.MatchID)
            {
                firstKey = item.Key;
                Debug.Log("Callled ^^^^^^^^^^" + firstKey);
                foreach (var item1 in item.Value.Pools)
                {
              
                    if (GameController.Instance.CurrentPoolID.Contains(item1.Value.PoolID.ToString()))
                    {
                        secondKey = item1.Key;
                        Debug.Log(firstKey + "^^^^^^^^^^" + secondKey);
                        GameController.Instance.writeNewUser(GameController.Instance.myUserID, firstKey, secondKey);
                        
                    }

                }

            }
        }


        Debug.Log(firstKey + "^^^^^^^^^^" + secondKey);

    }

    public void GetKeyForSlotFill()
    {
        foreach (var item in GameController.Instance.matchpool)
        {
            if(GameController.Instance.CurrentMatchID == item.Value.MatchID)
            {
                slotKey1= item.Key;
                foreach (var item1 in item.Value.Pools)
                {
                    if(GameController.Instance.CurrentPoolID == item1.Value.PoolID.ToString())
                    {
                        slotKey2 = item1.Key;
                        slotsFilled = item1.Value.SlotsFilled;
                    }
                }
            }
        }
    }


    private void OnEnable()
    {
        togscaptain.Clear();
        togsvcaptain.Clear();
        DisplayValue();
        contest.text = GameController.Instance.CurrentMatchTimeDuration;
    }


    public void DisplayValue()
    {
       
        for (int i = 0; i < MatchSelection.Instance.playersForTeam.Count; i++)
        {
            if (MatchSelection.Instance.playersForTeam[i].type == 0)
            {

                bool canSkip = false;
                foreach (Transform child in parent[0])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[0]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);
                Canvas.ForceUpdateCanvases();

            }
            else if (MatchSelection.Instance.playersForTeam[i].type == 1)
            {
                bool canSkip = false;
                foreach (Transform child in parent[1])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[1]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);
                Canvas.ForceUpdateCanvases();
            }
            else if (MatchSelection.Instance.playersForTeam[i].type == 2)
            {
                bool canSkip = false;
                foreach (Transform child in parent[2])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[2]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);
                Canvas.ForceUpdateCanvases();
            }
            else if (MatchSelection.Instance.playersForTeam[i].type == 3)
            {
                bool canSkip = false;
                foreach (Transform child in parent[3])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[3]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);
                Canvas.ForceUpdateCanvases();
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent[0].transform.parent.GetComponent<RectTransform>());
    }



    public void CheckForToggle1(Toggle toggle)
    {
        for (int i = 0; i < togscaptain.Count; i++)
        {
            if (toggle != togscaptain[i] && toggle.isOn)
                togscaptain[i].isOn = false;
        }
    }
    public void CheckForToggle2(Toggle toggle)
    {
        for (int i = 0; i < togsvcaptain.Count; i++)
        {
            if (toggle != togsvcaptain[i] && toggle.isOn)
                togsvcaptain[i].isOn = false;
        }
    }

}

