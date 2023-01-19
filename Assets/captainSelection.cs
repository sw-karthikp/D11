using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
public class captainSelection : UIHandler
{

    public GameObject childPrefab;
    public Transform[] parent;
    public List<Toggle> togscaptain;
    public List<Toggle> togsvcaptain;

    private void Awake()
    {

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


        string json = JsonConvert.SerializeObject(MatchSelection.Instance.playersForTeam,Formatting.None,new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        FirebaseDatabase mDatabase = FirebaseDatabase.DefaultInstance;
        Player teamAplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamA);
        Player teamBplayers = GameController.Instance.players.Find(x => x.TeamName == GameController.Instance.CurrentTeamB);

        SelectedTeamPlayers selectedTeamA = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamA };
        SelectedTeamPlayers selectedTeamB = new SelectedTeamPlayers() { TeamName = GameController.Instance.CurrentTeamB };

        string Captain = "";
        string viceCaptain = "";
        //ToDo
        string TeamId = "Team" + (GameController.Instance.mymatch.Count > 0 ? GameController.Instance.mymatch.ContainsKey(GameController.Instance.CurrentMatchID.ToString()) ? GameController.Instance.mymatch[GameController.Instance.CurrentMatchID.ToString()].SelectedTeam.Count + 1 : 1 : 1);
        string poolId = GameController.Instance.CurrentPoolID;
        foreach (var item in MatchSelection.Instance.playersForTeam)
        {
            if (item.isCaptain) Captain = item.PlayerID;
            if (item.isViceCaptain) viceCaptain = item.PlayerID;


            foreach (var item1 in teamAplayers.Players.Values)
            {
                if(item1.ID.Contains(item.PlayerID))
                {
                    selectedTeamA.players.Add(item.PlayerID);
    
                }
            }

            foreach (var item2 in teamBplayers.Players.Values)
            {
                if(item2.ID.Contains(item.PlayerID))
                {
                    selectedTeamB.players.Add(item.PlayerID);
  
                }
            }
            Debug.Log(selectedTeamA.players.Count + "$$$$$$$$$" + selectedTeamB.players.Count);
            //if (teamAplayers.Players.Find(x => x.ID == item.PlayerID) != null) selectedTeamA.players.Add(item.PlayerID);
            //if (teamBplayers.Players.Find(x => x.ID == item.PlayerID) != null) selectedTeamB.players.Add(item.PlayerID);
        }
        SelectedPlayers selectedPlayers = new SelectedPlayers() { Captain = Captain, ViceCaptian = viceCaptain, TeamA = selectedTeamA, TeamB = selectedTeamB};
        SelectedTeam selectedTeam = new SelectedTeam() { TeamID = TeamId, Players = selectedPlayers };
        SelectedPoolID selectedPool = new SelectedPoolID() { PoolID = poolId,TeamID = TeamId };
        string playerId = PlayerPrefs.GetString("userId");
        Debug.Log(playerId);
        string selectedPoolKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Push().Key;
        string selectedTeamKey = mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Push().Key;
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedPools").Child($"{selectedPoolKey}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedPool));
        mDatabase.RootReference.Child("PlayerMatches").Child($"{playerId}").Child($"{GameController.Instance.CurrentMatchID}").Child("SelectedTeam").Child($"{selectedTeamKey}").SetRawJsonValueAsync(JsonUtility.ToJson(selectedTeam));


        GameController.Instance.SubscribeSelectedMatchDetails();

    }


    private void OnEnable()
    {
        DisplayValue();
    }


    public void DisplayValue()
    {
        for (int i = 0; i < MatchSelection.Instance.playersForTeam.Count; i++)
        {
            if(MatchSelection.Instance.playersForTeam[i].type == 0)
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
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName ,MatchSelection.Instance.playersForTeam[i]);
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

            } else if (MatchSelection.Instance.playersForTeam[i].type == 2)
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

            } else if (MatchSelection.Instance.playersForTeam[i].type == 3)
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
}

