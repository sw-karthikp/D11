using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
public class captainSelection : UIHandler
{

    public GameObject childPrefab;
    public Transform[] parent;
    public List<Toggle> togscaptain;
    public List<Toggle> togsvcaptain;

    private void Awake()
    {

    }
    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

    }

    public void onClickSave()
    {

        string json = JsonConvert.SerializeObject(MatchSelection.Instance.playersForTeam);



        //Dictionary<string,object> toDict =  new Dictionary<string, object>();

        //foreach (var item in MatchSelection.Instance.playersForTeam)
        //{
        //    toDict.Add(item.playerName, (object)item);
        //}

        //  Debug.Log(toDict.Count);

        


        //DocumentReference docref = db.Collection("users").Document(FireBaseManager.Instance.auth.CurrentUser.UserId);

        //docref.UpdateAsync(toDict).ContinueWithOnMainThread(task =>
        //{

        //    if (task.IsCanceled || task.IsFaulted)
        //    {
        //        Debug.Log("Error: " + task.Exception);
        //    }

        //    Debug.Log("Succesfully updated.... ");

        //});
    }


    private void OnEnable()
    {
        DisplayValue();
    }


    public void DisplayValue()
    {
        for (int i = 0; i < MatchSelection.Instance.playersForTeam.Count; i++)
        {
            if(MatchSelection.Instance.playersForTeam[i].type == 0)
            {

                bool canSkip = false;
                foreach (Transform child in parent[0])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[0]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName ,MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);
      
            }
            else if (MatchSelection.Instance.playersForTeam[i].type == 1)
            {
                bool canSkip = false;
                foreach (Transform child in parent[1])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[1]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);

            } else if (MatchSelection.Instance.playersForTeam[i].type == 2)
            {
                bool canSkip = false;
                foreach (Transform child in parent[2])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[2]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);

            } else if (MatchSelection.Instance.playersForTeam[i].type == 3)
            {
                bool canSkip = false;
                foreach (Transform child in parent[3])
                {
                    if (child.name.Contains(MatchSelection.Instance.playersForTeam[i].playerName))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mprefab = Instantiate(childPrefab, parent[3]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<captinslectionHandler>().Setval(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i]);
                togscaptain.Add(mprefab.GetComponent<captinslectionHandler>().Captain);
                togsvcaptain.Add(mprefab.GetComponent<captinslectionHandler>().ViceCaptian);

            }




        }






    }


    

}

