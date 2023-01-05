using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class captainSelection : UIHandler
{

    public GameObject childPrefab;
    public Transform[] parent;
    public List<Toggle> togscaptain;
    public List<Toggle> togsvcaptain;
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


    private void Update()
    {
        if(togscaptain[0].isOn == true)
        {
            togscaptain[1].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[6].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if(togscaptain[1].isOn ==true)
        {
            togscaptain[0].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[6].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if (togscaptain[2].isOn == true)
        {
            togscaptain[0].isOn = false;
            togscaptain[1].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[6].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if (togscaptain[3].isOn == true)
        {
            togscaptain[0].isOn = false;
            togscaptain[1].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[6].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if (togscaptain[4].isOn == true)
        {
            togscaptain[0].isOn = false;
            togscaptain[1].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[6].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if (togscaptain[5].isOn == true)
        {
            togscaptain[0].isOn = false;
            togscaptain[1].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[6].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if (togscaptain[6].isOn == true)
        {
            togscaptain[0].isOn = false;
            togscaptain[1].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[7].isOn = false;
        }
        else if (togscaptain[7].isOn == true)
        {
            togscaptain[0].isOn = false;
            togscaptain[1].isOn = false;
            togscaptain[2].isOn = false;
            togscaptain[3].isOn = false;
            togscaptain[4].isOn = false;
            togscaptain[5].isOn = false;
            togscaptain[6].isOn = false;
        }

        ///////////////////////////////////
        if (togsvcaptain[0].isOn == true)
        {
            togsvcaptain[1].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[6].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[1].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[6].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[2].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[1].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[6].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[3].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[1].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[6].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[4].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[1].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[6].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[5].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[1].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[6].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[6].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[1].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[7].isOn = false;
        }
        else if (togsvcaptain[7].isOn == true)
        {
            togsvcaptain[0].isOn = false;
            togsvcaptain[1].isOn = false;
            togsvcaptain[2].isOn = false;
            togsvcaptain[3].isOn = false;
            togsvcaptain[4].isOn = false;
            togsvcaptain[5].isOn = false;
            togsvcaptain[6].isOn = false;
        }
    }


}
