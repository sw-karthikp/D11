using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using D11;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;

public class captainSelection : UIHandler
{

    public GameObject childPrefab;
    public Transform[] parent;
    public List<Toggle> togscaptain;
    public List<Toggle> togsvcaptain;

    public static captainSelection Instance;


    private void Awake()
    {
        Instance = this;
    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

    }

    public void onClickSave()
    {
        string json = JsonConvert.SerializeObject(MatchSelection.Instance.playersForTeam, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        Player teamAplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamA);
        Player teamBplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamB);

        SelectedTeamPlayers selectedTeamA = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamA };
        SelectedTeamPlayers selectedTeamB = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamB };
        FirebaseDatabase mDatabase = FirebaseDatabase.DefaultInstance;
        string Captain = "";
        string viceCaptain = "";
        //ToDo
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
        SelectedTeam selectedTeam = new SelectedTeam() { TeamID = TeamId, Players = selectedPlayers };
        SelectedPoolID selectedPool = new SelectedPoolID() { PoolID = poolId, TeamID = TeamId };
        string playerId = PlayerPrefs.GetString("userId");
        DebugHelper.Log(playerId);
        string selectedPoolKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Push().Key;
        string selectedTeamKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Push().Key;
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Child($"{selectedPoolKey}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedPool));
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Child($"{TeamId}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedTeam));
        string val1 = GameController.Instance.CurrentMatchID.ToString();
        string val2 = GameController.Instance.myUserID.ToString();
        mDatabase.RootReference.Child("JoinedPlayers").Child($"{val1}").Push().SetValueAsync(val2);

        GameController.Instance.SubscribeSelectedMatchDetails();
        BottomHandler.Instance.ResetScreen();
    }

    private void OnEnable()
    {
        DisplayValue();
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

            }
        }
    }
  
     public void CheckForToggle1()
    {

        if (this.gameObject.activeSelf)

        {
            int count1 = 0;

            for (int i = 0; i < togscaptain.Count; i++)
            {
                if (togscaptain[i].isOn)
                {



                    if (++count1 > 1)
                    {

                        togscaptain[i].isOn = false;

                    }
                    Debug.Log(count1);
                }
            }
   
        }
    }
    public void CheckForToggle2()
    {

        if (this.gameObject.activeSelf)

        {

            int count2 = 0;

            for (int i = 0; i < togsvcaptain.Count; i++)
            {
                if (togsvcaptain[i].isOn)
                {

         
                    if (++count2 > 1)
                    {
                        togsvcaptain[i].isOn = false;
                    }
                    Debug.Log(count2);
                }
            }
        }
    }
}

