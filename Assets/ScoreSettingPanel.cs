using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ScoreSettingPanel : UIHandler
{
    public static ScoreSettingPanel Instance;

    [Header("CurrentDetails")]
    public string CurrentTeamA;
    public string CurrentTeamB;

    [Header("Title_Variable")]
    public TMP_Text titleText;

    [Header("Toss_Variable")]
    public TMP_Text teamA;
    public TMP_Text teamB;
    public string tossWinTeamName;
    public string chooseTo;

    [Header("Score")]
    public GameObject childPrefab;
    public Transform parent;

    [Header("Player Display")]
    public GameObject forTeamA;
    public GameObject forTeamB;

    // Player Names
    public List<string> playerName = new List<string>();

    private void Awake()
    {
        Instance = this;   
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }

    public override void OnBack()
    {
        
    }

    public override void ShowMe()
    {
       AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public void SetCurrentMacthValue(string _teamA, string _teamB)
    {
        CurrentTeamA = _teamA;
        CurrentTeamB = _teamB;
        teamA.GetComponentInChildren<TMP_Text>().text = CurrentTeamA;
        teamB.GetComponentInChildren<TMP_Text>().text = CurrentTeamB;
        

        tossWinTeamName = CurrentTeamA;
        chooseTo = 

        forTeamA.transform.GetChild(1).GetComponent<TMP_Text>().text = CurrentTeamA;
        forTeamA.GetComponent<Toggle>().isOn = true;
        forTeamB.transform.GetChild(1).GetComponent<TMP_Text>().text = CurrentTeamB;
    }

    public void ToggleInstant(GameObject gameObject)
    {
        string teamName = gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text;
       
        if (gameObject.GetComponent<Toggle>().isOn)
        {
            foreach(KeyValuePair<string, object> item in AdminController.Instance.newPlayerTeamListKeyobj)
            {
                Dictionary<string, object> data = item.Value as Dictionary<string, object>;

                foreach(KeyValuePair<string, object> item2 in data)
                {
                    if (item2.Key == "Players")
                    {
                        Dictionary<string, object> data2 = item2.Value as Dictionary<string, object>;

                        playerName.Clear();

                        foreach (KeyValuePair<string, object> item3 in data2)
                        {
                            IDictionary data3 = item3.Value as IDictionary;

                            playerName.Add(data3["Name"].ToString());
                        }
                    }

                    if(item2.Key == "TeamName" && item2.Value.ToString() == teamName)
                    {
                        foreach(var player in playerName)
                        {
                            GameObject spawn = Instantiate(childPrefab, parent);
                            spawn.transform.GetChild(1).GetComponent<TMP_Text>().text = player;
                        }
                    }
                }
            }
        }
        else
        {
            if (parent.childCount > 0)
            {
                foreach(Transform item in parent)
                {  
                    Destroy(item.gameObject);
                }
            }
        }
    }

    public void TossWinTo(GameObject gameObject)
    {
        if(gameObject.GetComponent<Toggle>().isOn)
        {
            gameObject.transform.GetChild(1).GetComponent<TMP_Text>().color = Color.white;
            tossWinTeamName = gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text;
        }
        else
        {
            gameObject.transform.GetChild(1).GetComponent<TMP_Text>().color = Color.red;
        }
    }

    public void ChooseTo(bool value)
    {
        if (value)
        {
            chooseTo = "Bat";
        }
        else
        {
            chooseTo = "Bowl";
        }
    }
}


