using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Drawing;

public class MyMatchesMyTeam : MonoBehaviour
{
    [Header("TextVariables")]
    public TMP_Text teamName;
    public TMP_Text points;
    public GameObject pointsText;
    public TMP_Text wk;
    public TMP_Text bat;
    public TMP_Text ar;
    public TMP_Text bowl;
    public TMP_Text captain;
    public TMP_Text viceCaptain;
    public TMP_Text TeamA;
    public TMP_Text TeamB;
    public TMP_Text TeamAName;
    public TMP_Text TeamBName;
    public bool isMyTeam;
    public Toggle isSelectedToggle;
    [Header("ID")]
    public string capID;
    public string viceCapID;


    [Header("Images")]
    public Image captainPic;
    public Image ViceCaptainPic;

    [Header("List of Teams")]
    public Dictionary<string, List<string>> Teams = new();

    [Header("Button")]
    public Button click;

    private void Awake()
    {
        click.onClick.AddListener(() => { OnClickMyTeams(); });
    }

    public void SetData(string _teamName,string _points,string _wk,string _bat,string _ar, string _bowl ,string _captain ,string _viceCaptain ,Dictionary<string,List<string>> _teams,string _capID,string _viceCapID ,string TeamACount ,string TeamBCount , string _TeamAName, string _TeamBName)
    {

        isSelectedToggle.group = GetComponentInParent<ToggleGroup>();
        isSelectedToggle.isOn = false;
        Teams.Clear();
        teamName.text = _teamName;
     
        wk.text = _wk;
        bat.text = _bat;
        ar.text = _ar;
        bowl.text = _bowl;
        captain.text = _captain;
        viceCaptain.text = _viceCaptain;
        Teams = _teams;
        capID = _capID;
        viceCapID= _viceCapID;

        //foreach (var item in GameController.Instance.playerSpriteImage)
        //{
        //    if(item.Key == _capID)
        //    {
        //        captainPic.sprite = item.Value;
        //    }

        //    if(item.Key == _viceCapID)
        //    {
        //        ViceCaptainPic.sprite = item.Value;
        //    }
          
        //}

        foreach (var item2 in GameController.Instance.playerPic)
        {

            if (item2.Key == _capID)
            {
                captainPic.sprite = item2.pic;
            }

            if (item2.Key == _viceCapID)
            {
                ViceCaptainPic.sprite = item2.pic;
            }

        }

        foreach (var item in GameController.Instance.match)
        {
            foreach (var item1 in item.Value.Values)
            {
                if(item1.ID == GameController.Instance.CurrentMatchID)
                {
                    if(item.Key == "Complete")
                    {
                        TeamA.gameObject.SetActive(false);
                        TeamB.gameObject.SetActive(false);
                        TeamAName.gameObject.SetActive(false);
                        TeamBName.gameObject.SetActive(false);
                        points.gameObject.SetActive(true);
                        pointsText.SetActive(true);
                        points.text = _points;
        
                    }
                    else if(item.Key == "Live")
                    {
                        TeamA.gameObject.SetActive(false);
                        TeamB.gameObject.SetActive(false);
                        TeamAName.gameObject.SetActive(false);
                        TeamBName.gameObject.SetActive(false);
                        points.gameObject.SetActive(true);
                        pointsText.SetActive(true);
                        points.text = _points;
              
                    }
                    else if(item.Key == "Upcoming")
                    {
                        points.gameObject.SetActive(false);
                        pointsText.SetActive(false);
                        TeamA.gameObject.SetActive(true);
                        TeamB.gameObject.SetActive(true);
                        TeamAName.gameObject.SetActive(true);
                        TeamBName.gameObject.SetActive(true);
                        TeamA.text = TeamACount;
                        TeamAName.text = _TeamAName;
                        TeamBName.text = _TeamBName;
                        TeamB.text = TeamBCount;
                    }

                }
            }
        }


    }

     public void OnClickMyTeams()
    {
        Debug.Log("Called");
        UIController.Instance.myTeamsPlayersHolder.ShowMe();
        MyTeamPlayersPanel.Instance.isMyMatch = false;
        MyTeamPlayersPanel.Instance.SetMySelectedPlayerList(Teams, capID, viceCapID,teamName.text);

    }


 
}
