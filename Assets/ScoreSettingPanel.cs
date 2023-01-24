using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System;
using Firebase.Database;

public class ScoreSettingPanel : UIHandler
{
    public static ScoreSettingPanel Instance;

    [Header("CurrentDetails")]
    public string CurrentTeamA;
    public string CurrentTeamB;
    public string MatchId;

    [Header("Title_Variable")]
    public TMP_Text titleText;

    [Header("Toss")]
    public Toggle tossTeamA;
    public Toggle tossTeamB;

    [Header("Toss_Variable")]
    public TMP_Text teamA;
    public TMP_Text teamB;
    public string tossWon;
    public bool isBatting;
    public Toggle batting;
    public Toggle bowling;

    [Header("Score")]
    public GameObject childPrefab;
    public Transform parent;

    [Header("Player Display")]
    public ToggleGroup togGroupTeam;
    public Toggle forTeamA;
    public Toggle forTeamB;
    public TMP_Text forTeamATxt;
    public TMP_Text forTeamBTxt;

    [Header("Display Batsman")]
    public TMP_Text batsMan1Display;
    public TMP_Text batsMan2Display;
    public List<Toggle> batsmansToggles;

    [Header("Display Bowler")]
    public TMP_Text bowlerDisplay;
    public List<Toggle> bowlersToggles;

    [Header("Batsman 1 RunDetails")]
    public TMP_Text run1;
    public TMP_Text ball1;
    public TMP_Text four1;
    public TMP_Text six1;
    public TMP_Text strikeRate1;

    [Header("Batsman 2 RunDetails")]
    public TMP_Text run2;
    public TMP_Text ball2;
    public TMP_Text four2;
    public TMP_Text six2;
    public TMP_Text strikeRate2;

    [Header("Bowler BallDetails")]
    public TMP_Text over;
    public TMP_Text maiden;
    public TMP_Text run;
    public TMP_Text wicket;
    public TMP_Text economyRate;

    [Header("TogglesToCheckScore")]
    public Toggle[] runCount;
    public ToggleGroup togGroup;

    [Header("PlayerStricker")]
    public GameObject[] strickStar;

    [Header("CurrentPlayerIdBatter")]
    public string PlayerBatterOneID;
    public string PlayerBatterTwoID;

    [Header("CurrentPlayerIdBowler")]
    public string playerBowlerID;

    [Header("CurrentStrickerID")]
    public string CurrentStrickerID;

    [Header("MatchStats")]
    public int OversCount = 0;
    public int BallsCount;
    public int CurrentInnings = 0;
    public int MaidenCount;
    public int Score;
    public int player1Score;
    public int player2Score;
    public int bowlerScore;
    public int wickets;
    public int bowlerWickets;

    // FireBase Database
    public DatabaseReference databaseReference;

    [SerializeField] private string player1Name;
    [SerializeField] private string player2Name;

    private void Awake()
    {
        Instance = this;

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
        foreach (Transform childs in parent)
        {
            Destroy(childs.gameObject);

        }
        batsmansToggles.Clear();
        bowlersToggles.Clear();
    }

    public override void OnBack()
    {

    }

    public override void ShowMe()
    {

        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
        forTeamA.interactable = false;
        forTeamB.interactable = false;
        foreach (var item in runCount)
        {

            item.interactable = false;
        }

    }

    private void OnEnable()
    {
        togGroup.SetAllTogglesOff();
        togGroupTeam.SetAllTogglesOff();
    }

    string TeamAFullName;
    string TeamBFullName;

    public void SetCurrentMacthValue(string _teamA, string _teamB, string _matchId)
    {
        CurrentTeamA = _teamA;
        CurrentTeamB = _teamB;
        MatchId = _matchId;
        teamA.GetComponentInChildren<TMP_Text>().text = CurrentTeamA;
        teamB.GetComponentInChildren<TMP_Text>().text = CurrentTeamB;
        foreach (var item in AdminController.Instance.TeamFullName)
        {
            if (item.Key == _teamA)
            {
                TeamAFullName = item.Value;
            }

            if (item.Key == _teamB)
            {
                TeamBFullName = item.Value;
            }

        }
        titleText.text = TeamAFullName + " " + "VS" + " " + TeamBFullName + " Score Sheet";


        forTeamATxt.text = CurrentTeamA;
        forTeamA.name = CurrentTeamA;
        forTeamB.name = CurrentTeamB;
        forTeamA.isOn = true;
        forTeamBTxt.text = CurrentTeamB;


    }

    public void PlayerInstantiate()
    {
        batsmansToggles.Clear();
        bowlersToggles.Clear();
        foreach (var item in AdminController.Instance.newPlayerTeamListKeyobj)
        {
            foreach (var item1 in item.Value.Players)
            {
                GameObject spawn = Instantiate(childPrefab, parent);
                spawn.name = item1.Value.Name;
                spawn.GetComponent<PlayerToggle>().SetValueToPlayerToggle(MatchId, item.Value.TeamName, item1.Value.FPoint, item1.Value.ID, item1.Value.Name, item1.Value.Type, item1.Value.URL);

                if (spawn.GetComponent<PlayerToggle>().TeamName == tossWon)
                {
                    if (isBatting)
                    {
                        batsmansToggles.Add(spawn.GetComponent<Toggle>());
                    }
                    else
                    {
                        bowlersToggles.Add(spawn.GetComponent<Toggle>());
                    }

                }
                else
                {
                    if (!isBatting)
                    {
                        batsmansToggles.Add(spawn.GetComponent<Toggle>());
                    }
                    else
                    {
                        bowlersToggles.Add(spawn.GetComponent<Toggle>());
                    }
                }
            }

            if (forTeamA.isOn)
            {
                foreach (Transform child in parent)
                {
                    if (child.GetComponentInChildren<PlayerToggle>().TeamName == CurrentTeamA)
                    {
                        child.GetComponentInChildren<Toggle>().gameObject.SetActive(true);

                    }
                    else
                    {
                        child.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void ToggleInstant(GameObject gameObject)
    {
        if (forTeamA.isOn)
        {
            forTeamATxt.color = Color.white;
            forTeamBTxt.color = Color.red;
        }
        else
        {
            forTeamBTxt.color = Color.white;
            forTeamATxt.color = Color.red;
        }

        if (gameObject.GetComponent<Toggle>().isOn)
        {
            foreach (Transform item in parent)
            {
                if (item.GetComponentInChildren<PlayerToggle>().TeamName == gameObject.name)
                {
                    item.GetComponentInChildren<Toggle>().gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform item in parent)
            {
                if (item.GetComponentInChildren<PlayerToggle>().TeamName == gameObject.name)
                {
                    item.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
                }
            }
        }
    }

    private void CreateMatchDatabase()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Ram").Child("Live Match ScoreCard").Child(MatchId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Error to creating this match database " + task.Exception);
                return;
            }

            SetScore matchData = new();
            matchData.TeamA = CurrentTeamA;
            matchData.TeamB = CurrentTeamB;
            matchData.TossWon = tossWon;


            string json = JsonUtility.ToJson(matchData);
            Debug.Log(json);
            databaseReference.Child("Ram/Live Match ScoreCard").Child(MatchId).SetRawJsonValueAsync(json);

            Debug.Log("Successfully Live Match Database Are Created...");
        });
    }

    public void TossWinTo()
    {
        if (tossTeamA.isOn)
        {
            tossWon = CurrentTeamA;
            tossTeamB.interactable = false;
            teamA.color = Color.white;
            teamB.color = Color.gray;
        }
        else
        {
            tossWon = CurrentTeamB;
            tossTeamA.interactable = false;
            teamB.color = Color.white;
            teamA.color = Color.gray;
        }
    }

    public void ChooseTo()
    {
        if (batting.isOn)
        {
            isBatting = true;

            bowling.interactable = false;
        }
        else
        {
            isBatting = false;
            batting.interactable = false;
        }
        CreateMatchDatabase();
        CurrentInnings = 1;
        SetInnings(CurrentInnings);
        PlayerInstantiate();
        forTeamA.interactable = true;
        forTeamB.interactable = true;
        togGroupTeam.allowSwitchOff = false;

    }


    public void SetToggleIntractability()
    {
        foreach (var item in runCount)
        {
            item.interactable = true;
        }
        togGroup.allowSwitchOff = false;
    }

    public void SetToggleIntractabilityFalse()
    {
        togGroup.allowSwitchOff = true;
        foreach (var item in runCount)
        {
            item.interactable = false;
            item.isOn = false;
        }

        foreach (var item in bowlersToggles)
        {

            if (playerBowlerID == item.GetComponent<PlayerToggle>().ID)
            {
                item.isOn = false;
                StartCoroutine(item.GetComponent<PlayerToggle>().DisableTogglr());               
            }
     
        }

    }

    public void SetInnings(int val)
    {
        Innings Innings = new();
        string jsonInnings = JsonUtility.ToJson(Innings);
        string _innings = $"Innings{val}";
        databaseReference.Child($"Ram/Live Match ScoreCard/{MatchId}/MatchDetails").Child(_innings).SetRawJsonValueAsync(jsonInnings);
    }

    int dotBallCount = 0;

    public void ScoreManagerController(string _dataVal, string _ballsCount, string _overCount)
    {

        int bowlerRunsGiven;
        int PlayerScore;
        int bowlerWickets = 0;

        // BallsCount  Monitor 
        BallsCount = int.Parse(_ballsCount);

        // Team Score  Monitor 
        Score += int.Parse(_dataVal);

        // Player Individual Score  Monitor 
        if (CurrentStrickerID == PlayerBatterOneID)
        {
            player1Score += int.Parse(_dataVal);
            PlayerScore = player1Score;
        }
        else
        {
            player2Score += int.Parse(_dataVal);
            PlayerScore = player2Score;
        }

        //Bowlers Individual Runs Given  Monitor 
        bowlerScore += int.Parse(_dataVal);
        bowlerRunsGiven = bowlerScore;

        // Over Ending Monitor 
        if (BallsCount == 6)
        {
            OversCount++;
            SetToggleIntractabilityFalse();
            ToggleValueSet.Instance.ClearAfterEveryOver();
            BallsCount = 0;

            // Choosing Next Bowler
            bowlerDisplay.text = "";
            foreach (var item in bowlersToggles)
            {
                item.interactable = true;
            }
            bowlerScore = 0;
            bowlerWickets = 0;
        }

        // Maiden Monitor 
        if(_dataVal == "0")
        {
            dotBallCount++;
            if(dotBallCount == 6)
            {
                MaidenCount++;
            }
        }


        // Check for Wicket Count  Monitor 
        if (_dataVal == "Out")
        {
            bowlerWickets++;
            wickets++;
         
        }

        switch (_dataVal)
        {
            case "0":
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                break;
            case "1":
                dotBallCount = 0;
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                CurrentStrickerID = CurrentStrickerID == PlayerBatterOneID ? PlayerBatterTwoID : PlayerBatterOneID;
                Instance.strickStar[0].SetActive(Instance.CurrentStrickerID == Instance.PlayerBatterOneID ? true : false);
                Instance.strickStar[1].SetActive(Instance.CurrentStrickerID == Instance.PlayerBatterTwoID ? true : false);

                break;
            case "2":
                dotBallCount = 0;
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                break;
            case "3":
                dotBallCount = 0;
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                CurrentStrickerID = CurrentStrickerID == PlayerBatterOneID ? PlayerBatterTwoID : PlayerBatterOneID;
                Instance.strickStar[0].SetActive(Instance.CurrentStrickerID == Instance.PlayerBatterOneID ? true : false);
                Instance.strickStar[1].SetActive(Instance.CurrentStrickerID == Instance.PlayerBatterTwoID ? true : false);
                break;
            case "4":
                dotBallCount = 0;
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                break;
            case "6":
                dotBallCount = 0;
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                break;
            case "Out":
                SetDataToRealDbBatsMan(BallsCount.ToString(), "0", PlayerScore.ToString(), "0", "0", "0");
                SetDataToRealDbBowler("0", MaidenCount.ToString(), $"{OversCount}.{BallsCount}", bowlerRunsGiven.ToString(), "0");
                break;
        }
    }
    public void SetDataToRealDbBatsMan(string _balls, string _four, string _score, string _six, string _status, string _statusDetails)
    {

        Dictionary<string, object> setVal = new();
        setVal["Balls"] = _balls;
        setVal["Four"] = _four;
        setVal["Score"] = _score;
        setVal["Six"] = _six;
        setVal["Status"] = _status;
        setVal["StatusDetails"] = _statusDetails;
        databaseReference.Child($"Ram/Live Match ScoreCard/{ScoreSettingPanel.Instance.MatchId}/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Batting/Score/{ScoreSettingPanel.Instance.CurrentStrickerID}").UpdateChildrenAsync(setVal);
    }

    public void SetDataToRealDbBowler(string _extra, string _mainden, string _over, string _runs, string _wicket)
    {

        Dictionary<string, object> setVal = new();
        setVal["Extra"] = _extra;
        setVal["Mainden"] = _mainden;
        setVal["Over"] = _over;
        setVal["Runs"] = _runs;
        setVal["Wicket"] = _wicket;
        databaseReference.Child($"Ram/Live Match ScoreCard/{ScoreSettingPanel.Instance.MatchId}/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Bowling/{ScoreSettingPanel.Instance.playerBowlerID}").UpdateChildrenAsync(setVal);
    }

}


[Serializable]
public class SetScore
{
    public string TeamA;
    public string TeamB;
    public string TossWon;
    public MatchDetails MatchDetails = new();
}
[Serializable]
public class MatchDetails
{


}

[Serializable]
public class Innings
{

    public Batting Batting = new();
    public Bowling Bowling = new();
}

[Serializable]
public class Batting
{
    public PlayerScore Score = new();
}
[Serializable]
public class PlayerScore
{


}
[Serializable]
public class Extras
{

}
[Serializable]
public class PlayerMatchDetailsBatting
{
    public string Score;
    public string Balls;
    public string Four;
    public string Six;
    public string Status;
    public string StatusDetails;
}
[Serializable]
public class Bowling
{

}
[Serializable]
public class PlayerMatchDetailsBowling
{
    public string Over;
    public string Mainden;
    public string Runs;
    public string Extra;
    public string Wicket;
}