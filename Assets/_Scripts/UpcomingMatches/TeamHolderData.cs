using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeamHolderData : MonoBehaviour
{
  
    public TMP_Text teamA;
    public TMP_Text teamB;
    public string TeamA;
    public string TeamB;
    public int ID;
    public bool isPrimeGame;
    public Button Click;

    private void Awake()
    {
        Click.onClick.AddListener(() => { OnClickButton(); });
    }

    public void SetDetails(string teamAval ,string teamBval ,int id)
    {
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;

  
    }

    public void OnClickButton()
    {

        UIController.Instance.ContestPanel.ShowMe();
        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(ID,TeamA,TeamB));
        UIController.Instance.MainMenuScreen.HideMe();
    }

}
