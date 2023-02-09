using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking.Types;
using System;
using UnityEngine.UI;

public class MyTeam : MonoBehaviour
{
    public static MyTeam Instance;
    public Transform parent;
    string TeamName;
    string wkSt;
    string batSt;
    string arSt;
    string bowlSt;
    string captainSt;
    string vcCaptainSt;
    string capID;
    string vcCapID;
    public Dictionary<string, List<string>> selectedPlayers = new Dictionary<string, List<string>>();
    string TeamACount;
    string TeamBCount;
    string TeamAName;
    string TeamBName;
    float Points;
    public Button Join;
    public bool isMySelectedTeam;
    public string teamNameSelected;
    private void Awake()
    {
        Instance = this;
      
    }
    private void OnEnable()
    {
        FetchAdditionalData1();
        Join.onClick.AddListener(() => { OnClickJoinWithSelected(); });
    }

    public void OnClickJoinWithSelected()
    {

        foreach (Transform child in parent)
        {
           if(child.GetComponent<MyMatchesMyTeam>().isSelectedToggle.isOn)
            {
                teamNameSelected = child.GetComponent<MyMatchesMyTeam>().teamName.text;
            }
        }

        Debug.Log(" ######## JOIN GAME WITH SELECTED TEAM AVAILABLE ######");
        isMySelectedTeam = true;
        ContestHandler.Instance.conformHandler.ShowMe();


    }

    public void FetchAdditionalData1()
    {
        selectedPlayers.Clear();
        Points = 0;

        foreach (var item1 in GameController.Instance.selectedMatches)
        {
            if (item1.Key == GameController.Instance.CurrentMatchID.ToString())
            {
                foreach (var item in item1.Value.SelectedTeam)
                {

                    List<string> teams = new List<string>();

                    foreach (var item3 in item.Value.Players.TeamA.players)
                    {
                        teams.Add(item3);
                    }

                    foreach (var item2 in item.Value.Players.TeamB.players)
                    {
                        teams.Add(item2);

                    }
                    foreach (var teamList in teams)
                    {
                        foreach (var player in GameController.Instance.players)
                        {
                            foreach (var playerVal in player.Players.Values)
                            {

                                if (teamList == playerVal.ID)
                                {
                                    if (teamList == item.Value.Players.Captain)
                                    {

                                        capID = playerVal.ID;
                                        captainSt = playerVal.Name;
                                    }
                                    else if (teamList == item.Value.Players.ViceCaptian)
                                    {

                                        vcCapID = playerVal.ID;
                                        vcCaptainSt = playerVal.Name;
                                    }
                                }

                            }
                        }
                    }

                    Dictionary<string, List<string>> val = new Dictionary<string, List<string>>() { { item.Key, teams } };

                    int bat = 0;
                    int bowl = 0;
                    int ar = 0;
                    int wicket = 0;

                    foreach (var item2 in val)
                    {
                        foreach (var player in GameController.Instance.players)
                        {
                            foreach (var playerVal in player.Players.Values)
                            {
                                foreach (var item3 in item2.Value)
                                {
                                    if (item3 == playerVal.ID)
                                    {
                                        int PlayerType = playerVal.Type;

                                        if ((PlayerRoleType)PlayerType == PlayerRoleType.Batters)
                                        {
                                            bat++;
                                            batSt = bat.ToString();
                                        }
                                        else if ((PlayerRoleType)PlayerType == PlayerRoleType.Bowlers)
                                        {
                                            bowl++;
                                            bowlSt = bowl.ToString();
                                        }
                                        else if ((PlayerRoleType)PlayerType == PlayerRoleType.AllArounder)
                                        {
                                            ar++;
                                            arSt = ar.ToString();
                                        }
                                        else if ((PlayerRoleType)PlayerType == PlayerRoleType.WicketKeeper)
                                        {
                                            wicket++;
                                            wkSt = wicket.ToString();
                                        }
                                    }
                                }
                            }
                        }

                        TeamName = item.Key;

                    }

                    List<string> selectedPlayers = new List<string>();
                    string capName = "";
                    string vcCapname = "";
                    foreach (var item3 in GameController.Instance.selectedMatches)
                    {
                        if (GameController.Instance.CurrentMatchID == item3.Key)
                        {
                            foreach (var item4 in item3.Value.SelectedTeam)
                            {
                                if (item4.Key == TeamName)
                                {
                                    selectedPlayers = selectedPlayers.Concat(item4.Value.Players.TeamA.players).ToList();
                                    selectedPlayers = selectedPlayers.Concat(item4.Value.Players.TeamB.players).ToList();
                                    TeamACount = item4.Value.Players.TeamA.players.Count.ToString();
                                    TeamBCount = item4.Value.Players.TeamB.players.Count.ToString();
                                    TeamAName = item4.Value.Players.TeamA.TeamName;
                                    TeamBName = item4.Value.Players.TeamB.TeamName;
                                    capName = item4.Value.Players.Captain;
                                    vcCapname = item4.Value.Players.ViceCaptian;


                                }
                            }
                        }
                    }


                    MatchPools pools = GameController.Instance.matchpool.First(x => x.Value.MatchID == GameController.Instance.CurrentMatchID).Value;

                    float values = 0;


                    foreach (var itemN in selectedPlayers)
                    {
                        if (pools.Stats.ContainsKey(itemN))
                        {
                            if (capName == itemN) values += (pools.Stats[itemN] * 2);
                            else if (vcCapname == itemN) values += (pools.Stats[itemN] * 1.5f);
                            else
                                values += pools.Stats[itemN];

                        }
                    }


                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchesMyTeam");
                    mprefabObj.transform.SetParent(parent);
                    mprefabObj.gameObject.SetActive(true);
                    Debug.Log(TeamName + "$$$$$&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    mprefabObj.gameObject.name = TeamName;
                    mprefabObj.GetComponent<MyMatchesMyTeam>().SetData(TeamName, values.ToString(), wkSt, batSt, arSt, bowlSt, captainSt, vcCaptainSt, val, capID, vcCapID, TeamACount, TeamBCount, TeamAName, TeamBName);

                }
            }
        }
    }
}

