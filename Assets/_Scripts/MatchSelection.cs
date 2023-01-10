using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;

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
        tog[0].onValueChanged.AddListener(delegate { onTogWicketKeeper(); SetToggleUnActive0(); });
        tog[1].onValueChanged.AddListener(delegate { onTogBatting(); SetToggleUnActive1(); });
        tog[2].onValueChanged.AddListener(delegate { onTogAllRound(); SetToggleUnActive2(); });
        tog[3].onValueChanged.AddListener(delegate { onTogAllBowl(); SetToggleUnActive3(); });

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
        onTogWicketKeeper();
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

    public void onTogWicketKeeper()
    {
        if(tog[0].isOn)
        {
         
            parent[0].gameObject.SetActive(true);
            rect.content = parent[0].GetComponent<RectTransform>();
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
            rect.content = parent[1].GetComponent<RectTransform>();
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
            rect.content = parent[2].GetComponent<RectTransform>();
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
            rect.content = parent[3].GetComponent<RectTransform>();
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
    [FirestoreData]
    public class PlayerSelectedForMatch
    {
        [FirestoreProperty]
        public string playerName { get; set; }
        [FirestoreProperty]
        public string points { get; set; }
        [FirestoreProperty]
        public string countryName { get; set; }
        [FirestoreProperty]
        public int type { get; set; }
        [FirestoreProperty]
        public bool isCaptain { get; set; }
        [FirestoreProperty]
        public bool isViceCaptain{ get; set; }
    }
}
