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
    public TMP_Text toggleInformationText;
    public float TotalCredits = 100.0f;
    public Button next;
    public ScrollRect rect;
    Sprite playerPic;
    public Image teamA;
    public Image teamB;
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
        tog[0].onValueChanged.AddListener(delegate { PlayerSelectionToggle(0,3); SetToggleUnActive(0); TextAllocator(0); });
        tog[1].onValueChanged.AddListener(delegate { PlayerSelectionToggle(1,0); SetToggleUnActive(1); TextAllocator(1); });
        tog[2].onValueChanged.AddListener(delegate { PlayerSelectionToggle(2,2); SetToggleUnActive(2); TextAllocator(2); });
        tog[3].onValueChanged.AddListener(delegate { PlayerSelectionToggle(3,1); SetToggleUnActive(3); TextAllocator(3); });

    }

    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        playersForTeam.Clear();
  
        selectedplayerCount.text = "0";
        CreditsLeft.text = "100";
        for (int i = 0; i < Sprite_Swap.Instance.objects.Length; i++)
        {
            Sprite_Swap.Instance.objects[i].sprite = Sprite_Swap.Instance.Spritecolor[1];
        }
        PlayerSelectionToggle(0, 3);
      
        next.interactable = false;
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
        timeDuration.text = GameController.Instance.CurrentMatchTimeDuration;
        tog[0].isOn = true;
        parent[0].gameObject.SetActive(true);
        teamA.sprite = GameController.Instance.countryPic.Find(x=>x.Key == TeamA.text).pic;
        teamB.sprite = GameController.Instance.countryPic.Find(x => x.Key == TeamB.text).pic; ;

    }

    public void SetToggleUnActive(int _index)
    {
        if (playersForTeam.Count == 11 && parent[_index].gameObject.activeSelf)
        {
            for (int i = 0; i < parent[_index].childCount; i++)
            {
                if (parent[_index].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[_index].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < parent[_index].childCount; i++)
            {
                if (parent[_index].GetChild(i).GetChild(0).GetComponent<Toggle>().isOn == false)
                {
                    parent[_index].GetChild(i).GetChild(0).GetComponent<Toggle>().interactable = true;
                }
            }
        }
    }

    public void TextAllocator(int _index)
    {
        switch (_index)
        {
            case 0:
                toggleInformationText.text = "Select 1 - 4 Wicket-Keepers";
                break;
            case 1:
                toggleInformationText.text = "Select 3 - 6 Batters";
                break;
            case 2:
                toggleInformationText.text = "Select 1 - 4 All-Rounders";
                break;
            case 3:
                toggleInformationText.text = "Select 3 - 6 Bowlers";
                break;
        }
    }

 
    public void PlayerSelectionToggle(int _index , int _indexType)
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
                        if (item1.Value.Type == _indexType)
                        {

                            bool canSkip = false;
                            foreach (Transform child in parent[_index])
                            {
                                if (child.name.Contains(item1.Value.Name))
                                {
                                    canSkip = true;
                                    break;
                                }
                            }
                            if (canSkip) continue;
                            // PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("MatchSelection");
                            GameObject mprefabObj = Instantiate(childPrefab, parent[_index]);
                            mprefabObj.name = item1.Value.Name;
                            // mprefabObj.transform.SetParent(parent[_index]);
                            //mprefabObj.gameObject.SetActive(true);
                            //foreach (var item2 in GameController.Instance.playerSpriteImage)
                            //{
                            //    foreach (var sprite in GameController.Instance.playerSpriteImage.Values)
                            //    {
                            //        if (item1.Value.ID == item2.Key)
                            //        {
                            //            playerPic = item2.Value;
                            //        }
                            //    }
                            //}

                            foreach (var item2 in GameController.Instance.playerPic)
                            {
                               
                                
                                    if (item1.Value.ID == item2.Key)
                                    {
                                        playerPic = item2.pic;
                                    }
                                
                            }

                            mprefabObj.GetComponent<PlayerDetails>().SetPlayerData(item1.Value.ID,item1.Value.Name, item.TeamName, item1.Value.FPoint.ToString(), item1.Value.Type, playerPic);

                        }
                    }
                }
                if (item.TeamName.Contains(GameController.Instance.CurrentTeamB))
                {
                    foreach (var item1 in item.Players)
                    {
                        if (item1.Value.Type == _indexType)
                        {

                            bool canSkip = false;
                            foreach (Transform child in parent[_index])
                            {
                                if (child.name.Contains(item1.Value.Name))
                                {
                                    canSkip = true;
                                    break;
                                }
                            }
                            if (canSkip) continue;
                            // PoolItems mprefabObj = PoolManager.Instance.GetPoolObject ("MatchSelection");
                            GameObject mprefabObj = Instantiate(childPrefab, parent[_index]);
                            mprefabObj.name = item1.Value.Name;
                            //mprefabObj.transform.SetParent(parent[_index]);
                            //mprefabObj.gameObject.SetActive(true);
                            //foreach (var item2 in GameController.Instance.playerSpriteImage)
                            //{
                            //    foreach (var sprite in GameController.Instance.playerSpriteImage.Values)
                            //    {
                            //        if (item1.Value.ID == item2.Key)
                            //        {
                            //            playerPic = item2.Value;
                            //        }
                            //    }
                            //}


                            foreach (var item2 in GameController.Instance.playerPic)
                            {


                                if (item1.Value.ID == item2.Key)
                                {
                                    playerPic = item2.pic;
                                }

                            }
                            mprefabObj.GetComponent<PlayerDetails>().SetPlayerData(item1.Value.ID, item1.Value.Name, item.TeamName, item1.Value.FPoint.ToString(), item1.Value.Type, playerPic);
                        }
                    }
                }
            }
        }
        else
        {
            parent[_index].gameObject.SetActive(false);
         
        }
    }
}
