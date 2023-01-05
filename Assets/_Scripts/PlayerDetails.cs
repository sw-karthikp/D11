using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static MatchSelection;

public class PlayerDetails : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text countryName;
    public TMP_Text Fpoint;
    public int type;
    public Toggle tog;
    public float CreditsLeft;
    public float TotalSelectedCredits;

    private void Awake()
    {
        tog.onValueChanged.AddListener(delegate { OnvalueChange(); });
    }

    public void SetPlayerData(string _playerName, string _countryName, string _fPoint, int _type)
    {
        playerName.text = _playerName;
        countryName.text = _countryName;
        Fpoint.text = _fPoint;
        type = _type;
    }

    public void OnvalueChange()
    {

        PlayerSelectedForMatch newMatch = new PlayerSelectedForMatch();
        newMatch.playerName = playerName.text;
        newMatch.countryName = countryName.text;
        newMatch.points = Fpoint.text;
        newMatch.type = type;
        if (tog.isOn)
        {
            if (MatchSelection.Instance.playersForTeam.Find(x => x.playerName == newMatch.playerName) == null)
            {
                MatchSelection.Instance.playersForTeam.Add(newMatch);

             
                for (int i = 0; i < MatchSelection.Instance.playersForTeam.Count; i++)
                {
                    if (MatchSelection.Instance.playersForTeam[i].type == 0)
                    {
                 
                    }

                        Sprite_Swap.Instance.objects[i].sprite = Sprite_Swap.Instance.Spritecolor[0];
                }
                MatchSelection.Instance.selectedplayerCount.text = MatchSelection.Instance.playersForTeam.Count.ToString();
                TotalSelectedCredits = 0;
                foreach (var item in MatchSelection.Instance.playersForTeam)
                {
                    TotalSelectedCredits += float.Parse(item.points);
                    Debug.Log(TotalSelectedCredits);

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


                Sprite_Swap.Instance.objects[MatchSelection.Instance.playersForTeam.Count].sprite = Sprite_Swap.Instance.Spritecolor[1];

                MatchSelection.Instance.selectedplayerCount.text = MatchSelection.Instance.playersForTeam.Count.ToString();
                TotalSelectedCredits = 0;
                foreach (var item in MatchSelection.Instance.playersForTeam)
                {
                    TotalSelectedCredits += float.Parse(item.points);
                    Debug.Log(TotalSelectedCredits);

                }
                CreditsLeft = MatchSelection.Instance.TotalCredits - TotalSelectedCredits;
                MatchSelection.Instance.CreditsLeft.text = CreditsLeft.ToString();

            }



        }
    }


}
