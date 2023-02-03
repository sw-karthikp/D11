using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using static AddNewPlayerHandler;
using UnityEngine.Networking.Types;
using System;

public class MyTeam : MonoBehaviour
{

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
    private void OnEnable()
    {
        FetchAdditionalData1();

    }


    public void FetchAdditionalData1()
    {
        selectedPlayers.Clear();
        Points = 0;



        foreach (var item1 in GameController.Instance.selectedMatches)
        {
            if (item1.Key == GameController.Instance.CurrentMatchID.ToString())
            {
                foreach (var team in item1.Value.SelectedPools)
                {
                    foreach (var item in item1.Value.SelectedTeam)
                    {
                        if (item.Key == team.Value.TeamID)
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

                            Dictionary<string, List<string>> val = new Dictionary<string, List<string>>() { { team.Value.TeamID, teams } };

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

                                TeamName = team.Value.TeamID;
                                /////////////////////////R
                                Debug.Log(item.Key);
                               

                            }

                            List<string> selectedPlayers = new List<string>();
                            string capName ="";
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
                                            Debug.Log(TeamACount + "%%%%%%%%%%%" + TeamBCount+ "%%%%%%"+selectedPlayers.Count);

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







                            //foreach (var item4 in GameController.Instance.matchpool)
                            //{
                            //    if (item4.Value.MatchID == GameController.Instance.CurrentMatchID)
                            //    {
                            //        Dictionary<string, float> stats = item4.Value.Stats;
                            //        List<KeyValuePair<string, float>> myList = stats.ToList();
                            //        foreach (var item7 in myList)
                            //        {

                            //            bool selectedPlayer = false;
                            //            foreach (var teamList in teams)
                            //            {
                            //                foreach (var players in GameController.Instance.players)
                            //                {

                            //                    foreach (var playersVal in players.Players.Values)
                            //                    {

                            //                        if (teamList == playersVal.ID)
                            //                        {
                            //                            Debug.Log(teamList + "$$$$$$$$$$$$$$$");

                            //                            selectedPlayer =  .Players.TeamA.players.Contains(teamList) || teamVal.Players.TeamB.players.Contains(teamList);

                            //                        }
                            //                        Debug.Log(selectedPlayer + "^^^^^^^^^^");
                            //                    }
                            //                }
                            //            }

                            //            if (selectedPlayer)
                            //            {
                            //                Debug.Log(item7.Value + "#########");
                            //                Points += item7.Value;
                            //            }
                            //        }
                            //    }
                            //}




                            Debug.Log(values + "%%%%%%%%%%%%%");

                            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchesMyTeam");
                            mprefabObj.transform.SetParent(parent);
                            mprefabObj.gameObject.SetActive(true);
                            mprefabObj.gameObject.name = TeamName;
                            mprefabObj.GetComponent<MyMatchesMyTeam>().SetData(TeamName, values.ToString(), wkSt, batSt, arSt, bowlSt, captainSt, vcCaptainSt, val, capID, vcCapID, TeamACount, TeamBCount, TeamAName, TeamBName);
                        }
                    }
                }
            }
        }
    }  

}

