using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

using Newtonsoft.Json.Serialization;
using Unity.VisualScripting;
using TMPro;

public class GetMatchDetails : MonoBehaviour
{
    public DatabaseReference database;

    [SerializeField] private GameObject upcomingToggle;
    [SerializeField] private Transform tranParent;
    [SerializeField] private GameObject test;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;

        StartCoroutine(MatchDetails());
    }

    private IEnumerator MatchDetails()
    {
        var task = FirebaseDatabase.DefaultInstance.GetReference("Matches").Child("Upcoming").GetValueAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception == null)
        {
            DataSnapshot dataSnapshot = task.Result;

            foreach(var item in dataSnapshot.Children)
            {
                //Debug.Log("Parent Key: " + item.Key);
                string teamA = " ", teamB = " ", matchId = " ", hotGame = " ", matchDate = " ", matchTime = " ", matchType = " ";

                foreach (var item2 in item.Children)
                {

                    switch (item2.Key)
                    {
                        case "TeamA":
                            {
                                teamA = item2.Value.ToString();
                                break;
                            }

                        case "TeamB":
                            {
                                teamB = item2.Value.ToString();
                                break;
                            }
                        case "MatchId":
                            {
                                matchId = item2.Value.ToString();
                                break;
                            }
                        case "Type":
                            {
                                matchType = item2.Value.ToString();
                                break;
                            }
                        case "HotGame":
                            {
                                hotGame = item2.Value.ToString();
                                break;
                            }
                        case "MatchDate":
                            {
                                matchDate = item2.Value.ToString();
                                break;
                            }
                        case "MatchTime":
                            {
                                matchTime = item2.Value.ToString();
                                break;
                            }
                        default:
                            {
                                Debug.LogError("Data not matched");
                                break;
                            }
                    }
                }

                GameObject gameObject = Instantiate(upcomingToggle, tranParent);

                gameObject.GetComponent<Transform>().Find("DateTxt").GetComponent<TMP_Text>().text = matchDate;
                gameObject.GetComponent<Transform>().Find("T20Txt").GetComponent<TMP_Text>().text = matchType;
                gameObject.GetComponent<Transform>().Find("TeamImage1").GetComponentInChildren<TMP_Text>().text = teamA;
                gameObject.GetComponent<Transform>().Find("TeamImage2").GetComponentInChildren<TMP_Text>().text = teamB;

            }
        }
    }
}
