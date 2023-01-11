using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using System.Security.Cryptography;
using System.Reflection;

public class MatchSelection : UIHandler
{
    public Toggle[] tog;
    public ToggleGroup togGroup;
    public GameObject childPrefab;
    public Transform[] parent;
    public List<PlayerSelectedForMatch> playersForTeam;
    public static MatchSelection Instance;
    public TMP_Text TeamA;
    public TMP_Text TeamB;
    public TMP_Text CreditsLeft;
    public TMP_Text selectedplayerCount;
    public TMP_Text wk;
    public TMP_Text batter;
    public TMP_Text allround;
    public TMP_Text bowler;
    public TMP_Text teamACount;
    public TMP_Text teamBCount;
    public TMP_Text timeDuration;
    public float TotalCredits = 100.0f;
    public Button next;
    public ScrollRect rect;
    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

        for (int i = 0; i < parent.Length; i++)
        {
            foreach (Transform Child in parent[i])
            {
                Destroy(Child.gameObject);
            }
        }
        playersForTeam.Clear();
     

    }

    private void Awake()
    {
        Instance = this;
        tog[0].onValueChanged.AddListener(delegate { PlayerSelectionToggle(0); SetToggleUnActive0(); });
        tog[1].onValueChanged.AddListener(delegate { PlayerSelectionToggle(1); SetToggleUnActive1(); });
        tog[2].onValueChanged.AddListener(delegate { PlayerSelectionToggle(2); SetToggleUnActive2(); });
        tog[3].onValueChanged.AddListener(delegate { PlayerSelectionToggle(3); SetToggleUnActive3(); });

    }

    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);

        selectedplayerCount.text = "0";
        CreditsLeft.text = "100";

        for (int i = 0; i < Sprite_Swap.Instance.objects.Length; i++)
        {


            Sprite_Swap.Instance.objects[i].sprite = Sprite_Swap.Instance.Spritecolor[1];
        }

    }

    public override void OnBack()
    {

    }

    private void OnEnable()
    {
        wk.text = "0";
        batter.text ="0";
        allround.text ="0";
        bowler.text = "0";
        teamACount.text = "0";
        teamBCount.text = "0";
        TeamA.text = GameController.Instance.CurrentTeamA;
        TeamB.text = GameController.Instance.CurrentTeamB;
        timeDuration.text = GameController.Instance.CurrentMatchTimeDuration + " " + "Left";
        togGroup.allowSwitchOff = true;
        tog[0].isOn = true;
        tog[1].isOn = false;
        tog[2].isOn = false;
        tog[3].isOn = false;
        togGroup.allowSwitchOff = false;
        parent[0].gameObject.SetActive(true);
        PlayerSelectionToggle(0);
    }

    public void SetToggleUnActive0()
    {
        if(playersForTeam.Count ==  11 && parent[0].gameObject.activeSelf)
        {
            for (int i = 0; i < parent[0].childCount; i++)
            {
              
                if (parent[0].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[0].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = false;
                }
              
            }

        }
        else
        {
            for (int i = 0; i < parent[0].childCount; i++)
            {

                if (parent[0].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[0].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = true;
                }

            }
        }
    
    }

    public void SetToggleUnActive1()
    {
        if (playersForTeam.Count == 11 && parent[1].gameObject.activeSelf)
        {
            for (int i = 0; i < parent[1].childCount; i++)
            {

                if (parent[1].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[1].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = false;
                }

            }

        }
        else
        {
            for (int i = 0; i < parent[1].childCount; i++)
            {

                if (parent[1].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[1].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = true;
                }

            }
        }


    }

    public void SetToggleUnActive2()
    {
        if (playersForTeam.Count == 11 && parent[2].gameObject.activeSelf)
        {
            for (int i = 0; i < parent[2].childCount; i++)
            {

                if (parent[2].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[2].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = false;
                }

            }

        }
        else
        {
            for (int i = 0; i < parent[2].childCount; i++)
            {

                if (parent[2].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[2].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = true;
                 
                }

            }
        }


    }

    public void SetToggleUnActive3()
    {
        if (playersForTeam.Count == 11 && parent[3].gameObject.activeSelf)
        {
            for (int i = 0; i < parent[3].childCount; i++)
            {

                if (parent[3].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[3].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = false;
                }

            }

        }
        else
        {
            for (int i = 0; i < parent[3].childCount; i++)
            {

                if (parent[3].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[3].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = true;

                }

            }
        }


    }

    Sprite playerPic;

    public void PlayerSelectionToggle(int _index)
    {


        if (tog[_index].isOn)
        {
            parent[_index].gameObject.SetActive(true);
            rect.content = parent[_index].GetComponent<RectTransform>();

            foreach (var item in GameController.Instance.players)
            {
                if (item.TeamName.Contains(GameController.Instance.CurrentTeamA))
                {
                    foreach (var item1 in item.Players)
                    {
                        if (item1.Value.Type == _index)
                        {

                            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MatchSelection", false);
                            mprefabObj.transform.SetParent(parent[_index]);
                            mprefabObj.gameObject.SetActive(true);
                            foreach (var item2 in GameController.Instance.playerSpriteImage)
                            {
                                foreach (var sprite in GameController.Instance.playerSpriteImage.Values)
                                {
                                    if (item1.Value.ID == item2.Key)
                                    {
                                        playerPic = item2.Value;
                                    }
                                }
                            }
                            mprefabObj.GetComponent<PlayerDetails>().SetPlayerData(item1.Value.Name, item.TeamName, item1.Value.FPoint.ToString(), item1.Value.Type, playerPic);

                        }
                    }
                }
                if (item.TeamName.Contains(GameController.Instance.CurrentTeamB))
                {
                    foreach (var item1 in item.Players)
                    {
                        if (item1.Value.Type == _index)
                        {
                            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MatchSelection", false);
                            mprefabObj.transform.SetParent(parent[_index]);
                            mprefabObj.gameObject.SetActive(true);
                            foreach (var item2 in GameController.Instance.playerSpriteImage)
                            {
                                foreach (var sprite in GameController.Instance.playerSpriteImage.Values)
                                {
                                    if (item1.Value.ID == item2.Key)
                                    {
                                        playerPic = item2.Value;
                                    }
                                }
                            }
                            mprefabObj.GetComponent<PlayerDetails>().SetPlayerData(item1.Value.Name, item.TeamName, item1.Value.FPoint.ToString(), item1.Value.Type, playerPic);
                        }
                    }
                }
            }
        }
        else
        {
            parent[_index].gameObject.SetActive(false);

            foreach (Transform child in parent[_index])
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
