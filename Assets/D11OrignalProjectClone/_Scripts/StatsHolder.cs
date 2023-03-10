using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StatsHolder : MonoBehaviour
{
    public Transform parent;
    string playerName;
    string countryName;
    Sprite pic;
    public Button sort;
    public List<GameObject> objects= new List<GameObject>();
    Dictionary<string, float> value = new();
    public List<float> val;
    public List<GameObject> obj;
    private void Awake()
    {
        sort.onClick.AddListener(() => { OnSort(); });
    }

    private void OnEnable()
    {
       
        SetStatsVal();
    }

    public void OnSort()
    {
        objects  = objects.OrderByDescending(x => x.name).ToList();
        //foreach (Transform item in parent)
        //{
                 
        //}
    }
    string teamId;
    public void SetStatsVal()
    {


        //foreach (var item in GameController.Instance.selectedMatches)
        //{
        //    if(GameController.Instance.CurrentMatchID == item.Key)
        //    {
        //        foreach (var item1 in item.Value.SelectedPools)
        //        {
        //            if(item1.Value.PoolID == GameController.Instance.CurrentPoolID)
        //            {
        //                teamId =   item1.Value.TeamID;
        //            }
        //        }
        //    }
        //}
        string teamId = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedPools.First(x => (x.Value.PoolID == GameController.Instance.CurrentPoolID)).Value.TeamID;
        SelectedTeamID teamVal = GameController.Instance.selectedMatches[GameController.Instance.CurrentMatchID].SelectedTeam.First(x => x.Key == teamId).Value;



        foreach (var item in GameController.Instance.matchpool)
        {
            if (item.Value.MatchID == GameController.Instance.CurrentMatchID)
            {
                Dictionary<string, float> stats = item.Value.Stats;
                List<KeyValuePair<string, float>> myList = stats.ToList();
                myList.Sort(
                        delegate (KeyValuePair<string, float> pair1,
                        KeyValuePair<string, float> pair2)
                        {
                            return pair1.Value.CompareTo(pair2.Value);
                        }
                    );
                myList.Reverse();
                foreach (var item1 in myList)
                {
                    bool selectedPlayer = false;

                    foreach (var players in GameController.Instance.players)
                    {

                        foreach (var playersVal in players.Players.Values)
                        {
                            if (item1.Key == playersVal.ID)
                            {
                                playerName = playersVal.Name;
                                countryName = players.TeamName;
                                selectedPlayer = teamVal.Players.TeamA.players.Contains(item1.Key) || teamVal.Players.TeamB.players.Contains(item1.Key);
                            }

                        }
                    }

                    //foreach (var sprite in GameController.Instance.playerSpriteImage)
                    //{
                    //    if (item1.Key == sprite.Key)
                    //    {
                    //        pic = sprite.Value;
                    //    }

                    //}
                    foreach (var item3 in GameController.Instance.playerPic)
                    {
                        if (item1.Key == item3.Key)
                        {
                            pic = item3.pic;
                        }

                    }


                    PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PlayerStats");
                    mprefabObj.transform.SetParent(parent);
                    mprefabObj.gameObject.SetActive(true);
                    mprefabObj.name = item1.Key.ToString();
                    obj.Add(mprefabObj.gameObject);
                    mprefabObj.GetComponent<playerStats>().playerStatsVal(playerName, countryName, item1.Value.ToString(), pic, selectedPlayer);
                    //objects.Add(mprefabObj.gameObject);
                    ////val.Add(item1.Value);
                    ////int valint = mprefabObj.transform.GetSiblingIndex();
                    ////val.Add(valint);
                    ////val.Reverse();


                }
            }
        }


    }


}


