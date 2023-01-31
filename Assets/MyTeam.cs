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




                                foreach (var item3 in GameController.Instance.selectedMatches)
                                {
                                    if (GameController.Instance.CurrentMatchID == item3.Key)
                                    {
                                        foreach (var item4 in item3.Value.SelectedTeam)
                                        {
                                            if (item4.Key == TeamName)
                                            {
                                                TeamACount = item4.Value.Players.TeamA.players.Count.ToString();
                                                TeamBCount = item4.Value.Players.TeamB.players.Count.ToString();
                                                TeamAName = item4.Value.Players.TeamA.TeamName;
                                                TeamBName = item4.Value.Players.TeamB.TeamName;
                                                Debug.Log(TeamACount + "%%%%%%%%%%%" + TeamBCount);
                                            }
                                        }
                                    }
                                }

                                Points = 0;

                                foreach (var item5 in GameController.Instance.matchpool)
                                {
                                    if (item1.Key == GameController.Instance.CurrentMatchID.ToString())
                                    {


                                        foreach (var item6 in item5.Value.Stats)
                                        {

                                            try
                                            {
                                                SelectedTeamID players = item1.Value.SelectedTeam.Values.First(x => x.Players.TeamA.players.Contains(item6.Key));
                                                Points += item6.Value;
                                            }
                                            catch (Exception e)
                                            {

                                            }


                                            try
                                            {
                                                SelectedTeamID players = item1.Value.SelectedTeam.Values.First(x => x.Players.TeamB.players.Contains(item6.Key));
                                                Points += item6.Value;
                                            }
                                            catch (Exception e)
                                            {

                                            }







                                            // foreach (var item8 in item1.Value.SelectedTeam.Values)
                                            // {
                                            //   foreach (var item9 in item8.Players.TeamA.players)
                                            //  {
                                            //if (item6.Key == item9)
                                            //        {
                                            //            Points += item6.Value;
                                            //            Debug.Log(Points + "%%%%%%%%%%%%%@@@@@@@@@@@@@@@@@@@@@@@@@");
                                            //        }
                                            //    //}
                                            //   // foreach (var item10 in item8.Players.TeamB.players)
                                            //   // {
                                            //        if (item6.Key == item10)
                                            //        {
                                            //    Points += item6.Value;
                                            //    Debug.Log(Points + "%%%%%%%%%%%%%@@@@@@@@@@@@@@@@@@@@@@@@@");
                                            //}
                                            // }



                                        }

                                    }

                                }
                                Debug.Log(Points + "%%%%%%%%%%%%%");

                                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchesMyTeam");
                                mprefabObj.transform.SetParent(parent);
                                mprefabObj.gameObject.SetActive(true);
                                mprefabObj.gameObject.name = TeamName;
                                mprefabObj.GetComponent<MyMatchesMyTeam>().SetData(TeamName,"20", wkSt, batSt, arSt, bowlSt, captainSt, vcCaptainSt, val, capID, vcCapID, TeamACount, TeamBCount, TeamAName, TeamBName);
                            }


                        }
                    }
                }
            }
        }


    }
}

