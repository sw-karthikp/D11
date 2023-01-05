using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchHandler : UIHandler
{
    public static MatchHandler instance;

    [SerializeField] private Transform liveMatches;
    [SerializeField] private Transform upcomingMatches;
    [SerializeField] private Transform completeMatches;

    private void Start()
    {
        instance= this;
    }

    public void ChangeMatch(string status, GameObject gameObject)
    {
        switch(status)
        {
            case "LiveMatches_Holder":
                {
                    gameObject.transform.SetParent(completeMatches);
                    //gameObject.transform.Find("LiveMatchChange").GetComponent<Toggle>().isOn = false;
                    break;
                }
            case "UpComingMatches":
                {
                    gameObject.transform.SetParent(liveMatches);
                    //gameObject.transform.Find("LiveMatchChange").GetComponent<Toggle>().isOn = false;
                    break;
                }
            case "CompletedMatches":
                {
                    break;
                }
            default:
                {
                    Debug.LogError("Match status not matched...");
                    break;
                }
        }
    }

    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }

    public override void OnBack()
    {
      
    }
}
