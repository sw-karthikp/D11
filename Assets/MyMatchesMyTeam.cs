using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class MyMatchesMyTeam : MonoBehaviour
{
    [Header("TextVariables")]
    public TMP_Text teamName;
    public TMP_Text points;
    public TMP_Text wk;
    public TMP_Text bat;
    public TMP_Text ar;
    public TMP_Text bowl;
    public TMP_Text captain;
    public TMP_Text viceCaptain;

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

    public void SetData(string _teamName,string _points,string _wk,string _bat,string _ar, string _bowl ,string _captain ,string _viceCaptain ,Dictionary<string,List<string>> _teams,string _capID,string _viceCapID)
    {
        Teams.Clear();
        teamName.text = _teamName;
        points.text = _points;
        wk.text = _wk;
        bat.text = _bat;
        ar.text = _ar;
        bowl.text = _bowl;
        captain.text = _captain;
        viceCaptain.text = _viceCaptain;
        Teams = _teams;
        capID = _capID;
        viceCapID= _viceCapID;

        foreach (var item in GameController.Instance.playerSpriteImage)
        {
            if(item.Key == _capID)
            {
                captainPic.sprite = item.Value;
            }

            if(item.Key == _viceCapID)
            {
                ViceCaptainPic.sprite = item.Value;
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
