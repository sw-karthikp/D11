using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerDetails : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text countryName;
    public TMP_Text Fpoint;
    public string playerID;
    public Image _profilePic;
    public int type;
    public Toggle tog;
    public float CreditsLeft;
    public float TotalSelectedCredits;

    private void OnEnable()
    {
        tog.onValueChanged.AddListener(x => { OnvalueChange(); OnvalueChangeCountPlayerType(); OnvalueChangeTeam(); playerCount();
        });
    }

    public void SetPlayerData(string _playerID,string _playerName, string _countryName, string _fPoint, int _type ,Sprite pic)
    {
        playerName.text = _playerName;
        playerID = _playerID;
        countryName.text = _countryName;
        Fpoint.text = _fPoint;
        type = _type;
        _profilePic.sprite = pic;
    }

    public void playerCount()
    {
        MatchSelection.Instance.CheckForPlayerSelection();
        if (MatchSelection.Instance.playersForTeam.Count == 11)
        {
            MatchSelection.Instance.next.interactable = true;
        }
        else
        {
            MatchSelection.Instance.next.interactable = false;
        }
        MatchSelection.Instance.SetToggleUnActive(0);
        MatchSelection.Instance.SetToggleUnActive(1);
        MatchSelection.Instance.SetToggleUnActive(2);
        MatchSelection.Instance.SetToggleUnActive(3);

    }
    public void OnvalueChange()
    {

        PlayerSelectedForMatch newMatch = new PlayerSelectedForMatch();
        newMatch.playerName = playerName.text;
        newMatch.PlayerID = playerID;
        newMatch.countryName = countryName.text;
        newMatch.points = Fpoint.text;
        newMatch.type = type;
        newMatch.playerPic = _profilePic.sprite;
        if (tog.isOn)
        {
            if (MatchSelection.Instance.playersForTeam.Find(x => x.playerName == newMatch.playerName) == null)
            {
                MatchSelection.Instance.playersForTeam.Add(newMatch);

                switch (newMatch.type)
                {
                    case 3:
                        MatchSelection.Instance.Keeper.Add(newMatch);

                        break;
                    case 0:
                        MatchSelection.Instance.Batter.Add(newMatch);
                        break;
                    case 2:
                        MatchSelection.Instance.AllRound.Add(newMatch);
                        break;
                    case 1:
                        MatchSelection.Instance.Bowler.Add(newMatch);
                        break;
                }


                for (int i = 0; i < MatchSelection.Instance.playersForTeam.Count; i++)
                {
                    Sprite_Swap.Instance.objects[i].sprite = Sprite_Swap.Instance.Spritecolor[0];
                }


                MatchSelection.Instance.selectedplayerCount.text = MatchSelection.Instance.playersForTeam.Count.ToString();
                TotalSelectedCredits = 0;
                foreach (var item in MatchSelection.Instance.playersForTeam)
                {
                    TotalSelectedCredits += float.Parse(item.points);
                }
                CreditsLeft = MatchSelection.Instance.TotalCredits - TotalSelectedCredits;
                MatchSelection.Instance.CreditsLeft.text = CreditsLeft.ToString();


            }


        }
        else
        {

            if (MatchSelection.Instance.playersForTeam.Find(x => x.playerName == newMatch.playerName) != null)
            {
                MatchSelection.Instance.playersForTeam.Remove(MatchSelection.Instance.playersForTeam.First(x => x.playerName == newMatch.playerName));


                switch (newMatch.type)
                {
                    case 3:
                        {
                            MatchSelection.Instance.Keeper.Remove(MatchSelection.Instance.Keeper.First(x => x.playerName == newMatch.playerName));
                            break;
                        }

                    case 0:
                        {
                            MatchSelection.Instance.Batter.Remove(MatchSelection.Instance.Batter.First(x => x.playerName == newMatch.playerName));
                            break;
                        }

                    case 2:
                        {
                            MatchSelection.Instance.AllRound.Remove(MatchSelection.Instance.AllRound.First(x => x.playerName == newMatch.playerName));
                            break;
                        }

                    case 1:
                        {
                            MatchSelection.Instance.Bowler.Remove(MatchSelection.Instance.Bowler.First(x => x.playerName == newMatch.playerName));
                            break;
                        }
                }

                MatchSelection.Instance.SetToggleActiveForParentOn();
                Sprite_Swap.Instance.objects[MatchSelection.Instance.playersForTeam.Count].sprite = Sprite_Swap.Instance.Spritecolor[1];
                MatchSelection.Instance.selectedplayerCount.text = MatchSelection.Instance.playersForTeam.Count.ToString();
                TotalSelectedCredits = 0;
                foreach (var item in MatchSelection.Instance.playersForTeam)
                {
                    TotalSelectedCredits += float.Parse(item.points);
                }
                CreditsLeft = MatchSelection.Instance.TotalCredits - TotalSelectedCredits;
                MatchSelection.Instance.CreditsLeft.text = CreditsLeft.ToString();
            }
        }
    }

    void OnvalueChangeCountPlayerType()
    {

        int wkt = 0, bat = 0, overall = 0, bowling = 0;
        foreach (var item in MatchSelection.Instance.playersForTeam)
        {


            if (item.type == 3)
            {
               
                wkt++;
               
            }
            else if (item.type == 0)
            {
                bat++;
             
            }
            else if (item.type == 2)
            {
                overall++;
               
            }
            else if (item.type == 1)
            {
                bowling++;
                
            }


        }
        ToggleAdditionCount.Instance.SetValue(wkt, bat, overall, bowling);

    }
    void OnvalueChangeTeam()
    {

        int TeamA = 0,  TeamB = 0;
        foreach (var item in MatchSelection.Instance.playersForTeam)
        {


            if (item.countryName == GameController.Instance.CurrentTeamA)
            {

                TeamA++;

            }
            else if (item.countryName ==  GameController.Instance.CurrentTeamB)
            {
                TeamB++;

            }
     

        }
        ToggleAdditionCount.Instance.SetValueTeam(TeamA, TeamB );

    }


}
