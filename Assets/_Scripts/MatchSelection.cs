using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public float TotalCredits = 100.0f;
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
        tog[0].onValueChanged.AddListener(delegate { onTogWicketKeeper(); });
        tog[1].onValueChanged.AddListener(delegate { onTogBatting(); });
        tog[2].onValueChanged.AddListener(delegate { onTogAllRound(); });
        tog[3].onValueChanged.AddListener(delegate { onTogAllBowl(); });

    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }

    private void OnEnable()
    {
        selectedplayerCount.text = "0";
        CreditsLeft.text = "100";

        for (int i = 0; i < Sprite_Swap.Instance.objects.Length; i++)
        {


            Sprite_Swap.Instance.objects[i].sprite = Sprite_Swap.Instance.Spritecolor[1];
        }
        TeamA.text = GameController.Instance.CurrentTeamA;
        TeamB.text = GameController.Instance.CurrentTeamB;
        togGroup.allowSwitchOff = true;
        tog[0].isOn = true;
        tog[1].isOn = false;
        tog[2].isOn = false;
        tog[3].isOn = false;
        togGroup.allowSwitchOff = false;
       parent[0].gameObject.SetActive(true);
        onTogWicketKeeper();
    }



    public void onTogWicketKeeper()
    {
        if(tog[0].isOn)
        {
            parent[0].gameObject.SetActive(true);
            for (int i = 0; i < GameController.Instance.players.Count; i++)
            {
                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamA))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {

                        bool canSkip = false;
                        foreach (Transform child in parent[0])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                Debug.Log("**************");
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;

                        if (GameController.Instance.players[i].Players[j].Type == 0)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[0]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(),GameController.Instance.players[i].Players[j].Type);
                        }
                     
                    }

                }
                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamB))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {
                        bool canSkip = false;
                        foreach (Transform child in parent[0])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 0)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[0]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }
                    }

                }

            }
        }
        else
        {
           
            parent[0].gameObject.SetActive(false);
          
        }
    }


    public void onTogBatting()
    {
        if (tog[1].isOn)
        {
            parent[1].gameObject.SetActive(true);
            for (int i = 0; i < GameController.Instance.players.Count; i++)
            {

                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamA))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {

                        bool canSkip = false;
                        foreach (Transform child in parent[1])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 1)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[1]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }

                    }

                }
                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamB))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {
                        bool canSkip = false;
                        foreach (Transform child in parent[1])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 1)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[1]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }
                    }

                }

            }
        }
        else
        {
            //foreach (Transform Child in parent[1])
            //{
            //    Destroy(Child.gameObject);
            //}
            parent[1].gameObject.SetActive(false);

        }
    }

    public void onTogAllRound()
    {
        if (tog[2].isOn)
        {
            parent[2].gameObject.SetActive(true);
            for (int i = 0; i < GameController.Instance.players.Count; i++)
            {

                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamA))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {
                        bool canSkip = false;
                        foreach (Transform child in parent[2])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 2)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[2]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }

                    }

                }
                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamB))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {
                        bool canSkip = false;
                        foreach (Transform child in parent[2])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 2)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[2]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }
                    }

                }

            }
        }
        else
        {
            //foreach (Transform Child in parent[2])
            //{
            //    Destroy(Child.gameObject);
            //}
            parent[2].gameObject.SetActive(false);

        }
    }

    public void onTogAllBowl()
    {
        if (tog[3].isOn)
        {
            parent[3].gameObject.SetActive(true);
            for (int i = 0; i < GameController.Instance.players.Count; i++)
            {

                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamA))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {
                        bool canSkip = false;
                        foreach (Transform child in parent[3])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 3)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[3]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }

                    }

                }
                if (GameController.Instance.players[i].TeamName.Contains(GameController.Instance.CurrentTeamB))
                {
                    for (int j = 0; j < GameController.Instance.players[i].Players.Count; j++)
                    {
                        bool canSkip = false;
                        foreach (Transform child in parent[3])
                        {
                            if (child.name.Contains(GameController.Instance.players[i].Players[j].Name))
                            {
                                canSkip = true;
                                break;
                            }
                        }
                        if (canSkip) continue;
                        if (GameController.Instance.players[i].Players[j].Type == 3)
                        {
                            GameObject mPrefab = Instantiate(childPrefab, parent[3]);
                            mPrefab.name = GameController.Instance.players[i].Players[j].Name;
                            mPrefab.GetComponent<PlayerDetails>().SetPlayerData(GameController.Instance.players[i].Players[j].Name, GameController.Instance.players[i].TeamName, GameController.Instance.players[i].Players[j].FPoint.ToString(), GameController.Instance.players[i].Players[j].Type);
                        }
                    }

                }

            }
        }
        else
        {
            //foreach (Transform Child in parent[3])
            //{
            //    Destroy(Child.gameObject);
            //}
            parent[3].gameObject.SetActive(false);

        }
    }

    [Serializable]
    public class PlayerSelectedForMatch
    {
        public string playerName;
        public string points;
        public string countryName;
        public int type;
        public bool isCaptain;
        public bool isViceCaptain;
    }
}
