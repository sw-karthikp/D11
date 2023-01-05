using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MyTeamPlayersPanel : UIHandler
{
    public TMP_Text TeamA;
    public TMP_Text TeamB;
    public GameObject ChildPrefab;
    public Transform[] parent;

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

        for (int i = 0; i < parent.Length; i++)
        {
            foreach (Transform child in parent[i])
            {
                Destroy(child.gameObject);
            }
        }
    
    }

    public void OnEnable()
    {
        SetMySelectedPlayers();
    }

    public void SetMySelectedPlayers()
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
                GameObject mprefab = Instantiate(ChildPrefab, parent[0]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName , MatchSelection.Instance.playersForTeam[i].isCaptain ,MatchSelection.Instance.playersForTeam[i].isViceCaptain);

            }
            if (MatchSelection.Instance.playersForTeam[i].type == 1)
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
                GameObject mprefab = Instantiate(ChildPrefab, parent[1]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain);

            }
            if (MatchSelection.Instance.playersForTeam[i].type == 2)
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
                GameObject mprefab = Instantiate(ChildPrefab, parent[2]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain);

            }
            if (MatchSelection.Instance.playersForTeam[i].type == 3)
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
                GameObject mprefab = Instantiate(ChildPrefab, parent[3]);
                mprefab.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefab.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain);

            }
        }
       
    }

}
