using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyTeam : MonoBehaviour
{

    public Transform parent;
    string wkSt;
    string batSt;
    string arSt;
    string bowlSt;
    string captainSt;
    string vcCaptainSt;
    public List<string> teams;
    private void OnEnable()
    {
        FetchAdditionalData();
        FecthData();
    }

    public void FecthData()
    {
        foreach (var item1 in GameController.Instance.selectedMatches)
        {
            if (item1.Key == GameController.Instance.CurrentMatchID.ToString())
            {
                foreach (var item in item1.Value.SelectedTeam.Values)
                {
                    DebugHelper.Log(item.Players.Captain + "#####" + item.Players.ViceCaptian);
                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MyMatchesMyTeam");
                    mprefabObj.transform.SetParent(parent);
                    mprefabObj.gameObject.SetActive(true);
                    mprefabObj.GetComponent<MyMatchesMyTeam>().SetData($"TEAM{1}", "450", wkSt, batSt, arSt, bowlSt, captainSt, vcCaptainSt, teams, item.Players.Captain, item.Players.ViceCaptian);
                }
            }
        }

    }

    public void FetchAdditionalData()
    {
        teams.Clear();
        int bat = 0;
        int bowl = 0;
        int ar = 0;
        int wicket = 0;
        foreach (var item1 in GameController.Instance.selectedMatches)
        {
            Debug.Log(item1.Key + "$$$$$$$$$$$$$$");
            if (item1.Key == GameController.Instance.CurrentMatchID.ToString())
            {


                foreach (var team in item1.Value.SelectedPools.Values)
                {
                    foreach (var team1 in item1.Value.SelectedTeam)

                        foreach (var item in item1.Value.SelectedTeam.Values)
                        {

                       
                            foreach (var item3 in item.Players.TeamA.players)
                            {
                                if (team.TeamID == team1.Key)
                                {
                                    teams.Add(item3);
                                }
                            }

                            foreach (var item2 in item.Players.TeamB.players)
                            {

                                if (team.TeamID == team1.Key)
                                {
                                    teams.Add(item2);
                                }
                                   
                            }


                            foreach (var teamList in teams)
                            {
                                foreach (var player in GameController.Instance.players)
                                {
                                    foreach (var playerVal in player.Players.Values)
                                    {

                                        if (teamList == playerVal.ID)
                                        {
                                            if (teamList == item.Players.Captain)
                                            {
                                                DebugHelper.Log(playerVal.ID + "^^^^^^^^" + playerVal.Name);
                                                captainSt = playerVal.Name;
                                            }
                                            else if (teamList == item.Players.ViceCaptian)
                                            {
                                                DebugHelper.Log(playerVal.ID + "^^^^^^^^" + playerVal.Name);
                                                vcCaptainSt = playerVal.Name;
                                            }
                                        }

                                    }
                                }
                            }

                        }

                }
            }

        }

        foreach (var item in teams)
        {
            foreach (var player in GameController.Instance.players)
            {
                foreach (var playerVal in player.Players.Values)
                {
                    if (item == playerVal.ID)
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
    }
}
