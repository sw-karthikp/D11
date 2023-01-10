using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class PlayerToggle : MonoBehaviour
{


    [Header("PlayerName")]
    public TMP_Text playerName;
    public Toggle tog;
    public string TeamName;
    public string FPoint;
    public string ID;
    public string Name;
    public string Type;
    public string URL;
    public string MatchID;
    // FireBase Database
    public DatabaseReference databaseReference;

    private void Awake()
    {
        tog.onValueChanged.AddListener(delegate { OnValueChnaged(); });
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public IEnumerator DisableTogglr()
    {
        yield return new WaitForEndOfFrame();
        tog.interactable = false;
    }


    public void SetValueToPlayerToggle(string _matchID, string _teamName, string _fpoint, string _id, string _name, string _type, string _url)
    {
        MatchID = _matchID;
        TeamName = _teamName;
        FPoint = _fpoint;
        ID = _id;
        Name = _name;
        Type = _type;
        URL = _url;
        playerName.text = _name;
    }



    public void OnValueChnaged()
    {
       
        PlayerMatchDetailsBatting bat = new();
        string matchJsonBat = JsonUtility.ToJson(bat);
        PlayerMatchDetailsBowling bowl = new();
        string matchJsonBowl = JsonUtility.ToJson(bowl);
        if (TeamName == ScoreSettingPanel.Instance.tossWon)
        {

            if (ScoreSettingPanel.Instance.isBatting)
            {
                foreach (var item1 in ScoreSettingPanel.Instance.batsmansToggles)
                {
                    if (item1.isOn)
                    {
                         

                        if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count == 1)
                        {
                            Debug.Log("Called One");
                            ScoreSettingPanel.Instance.strickStar[0].SetActive(true);
                            ScoreSettingPanel.Instance.strickStar[1].SetActive(false);
                            ScoreSettingPanel.Instance.batsMan1Display.text = Name;
                            databaseReference.Child("Ram/Live Match ScoreCard/" + MatchID + $"/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Batting/Score").Child(ID).SetRawJsonValueAsync(matchJsonBat);
                            ScoreSettingPanel.Instance.PlayerBatterOneID = ID;
                            ScoreSettingPanel.Instance.CurrentStrickerID = ID;


                        }
                        else if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count == 2)
                        {
                            Debug.Log("Called Two");
                            databaseReference.Child("Ram/Live Match ScoreCard/" + MatchID + $"/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Batting/Score").Child(ID).SetRawJsonValueAsync(matchJsonBat);
                            ScoreSettingPanel.Instance.batsMan2Display.text = Name;
                            ScoreSettingPanel.Instance.PlayerBatterTwoID = ID;
                        }
                      
                    }


                }

                foreach (var item2 in ScoreSettingPanel.Instance.batsmansToggles)
                {

                    if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count == 2)
                    {
         
                        if (!item2.isOn)
                        {
                            item2.interactable = false;

                        }
                    }
                }
            }
            else
            {
                foreach (var item2 in ScoreSettingPanel.Instance.bowlersToggles)
                {
                    if (item2.isOn)
                    {
                        ScoreSettingPanel.Instance.bowlerDisplay.text = item2.transform.name;
                        databaseReference.Child("Ram/Live Match ScoreCard/" + MatchID + $"/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Bowling/").Child(ID).SetRawJsonValueAsync(matchJsonBowl);
                        ScoreSettingPanel.Instance.playerBowlerID = ID;

                   
                    }
                    else
                    {
                        item2.interactable = false;

                    }
                }
            }

        }
        else
        {
            if (!ScoreSettingPanel.Instance.isBatting)
            {
                foreach (var item1 in ScoreSettingPanel.Instance.batsmansToggles)
                {
                    if (item1.isOn)
                    {


                        if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count == 1)
                        {
                            Debug.Log("Called One");
                            ScoreSettingPanel.Instance.strickStar[0].SetActive(true);
                            ScoreSettingPanel.Instance.strickStar[1].SetActive(false);
                            ScoreSettingPanel.Instance.batsMan1Display.text = Name;
                            databaseReference.Child("Ram/Live Match ScoreCard/" + MatchID + $"/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Batting/Score").Child(ID).SetRawJsonValueAsync(matchJsonBat);
                            ScoreSettingPanel.Instance.PlayerBatterOneID = ID;
                            ScoreSettingPanel.Instance.CurrentStrickerID = ID;

                        }
                        else if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count == 2)
                        {
                            Debug.Log("Called Two");
                            ScoreSettingPanel.Instance.batsMan2Display.text = Name;
                            databaseReference.Child("Ram/Live Match ScoreCard/" + MatchID + $"/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Batting/Score").Child(ID).SetRawJsonValueAsync(matchJsonBat);
                            ScoreSettingPanel.Instance.PlayerBatterTwoID = ID;
                        }
                    }


                }

                foreach (var item2 in ScoreSettingPanel.Instance.batsmansToggles)
                {
                    if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count == 2)
                    {
         
                        if (!item2.isOn)
                        {
                            item2.interactable = false;

                        }
                    }
                }
            }
            else
            {
                foreach (var item2 in ScoreSettingPanel.Instance.bowlersToggles)
                {
                    if (item2.isOn)
                    {
                        ScoreSettingPanel.Instance.bowlerDisplay.text = item2.transform.name;
                        databaseReference.Child("Ram/Live Match ScoreCard/" + MatchID + $"/MatchDetails/Innings{ScoreSettingPanel.Instance.CurrentInnings}/Bowling/").Child(ID).SetRawJsonValueAsync(matchJsonBowl);
                        ScoreSettingPanel.Instance.playerBowlerID = ID;
                  
                    }
                    else
                    {
                        item2.interactable = false;


                    }
                }
            }
        }


        if (ScoreSettingPanel.Instance.batsmansToggles.FindAll(x => x.isOn).Count + ScoreSettingPanel.Instance.bowlersToggles.FindAll(x => x.isOn).Count >= 3)
        {
            ScoreSettingPanel.Instance.SetToggleIntractability();
        }

    }
}
