using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : UIHandler
{
    [Header("Toggle")]
    public Toggle[] mainMenuTogs;

    [Header("ToggleGroup")]
    public ToggleGroup mainMenuToggleGroup;

    [Header("MainMenuSubPanel")]
    public UIHandler matchesCreation;
    public UIHandler teamsCreation;
    public UIHandler teamPlayerList;
    public UIHandler rules;
    public UIHandler logout;

    public static MainMenuController Instance;


    private void Awake()
    {
        Instance = this;

        mainMenuTogs[0].onValueChanged.AddListener(delegate { OnClickMatches(); });
        mainMenuTogs[1].onValueChanged.AddListener(delegate { OnClickTeams();  });
        mainMenuTogs[2].onValueChanged.AddListener(delegate { OnClickPlayerList(); });
        mainMenuTogs[3].onValueChanged.AddListener(delegate { OnClickRules(); });
        mainMenuTogs[4].onValueChanged.AddListener(delegate { OnClickLogout(); });
    }


    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }


    public void OnClickMatches()
    {
        if (mainMenuTogs[0].isOn)
        {
            matchesCreation.ShowMe();
        }
        else
        {
            matchesCreation.HideMe();
        }
    }

    public void OnClickTeams()
    {
        if (mainMenuTogs[1].isOn)
        {
            teamsCreation.ShowMe();
        }
        else
        {
            teamsCreation.HideMe();
        }
    }


    public void OnClickPlayerList()
    {
        if (mainMenuTogs[2].isOn)
        {
            teamPlayerList.ShowMe();
        }
        else
        {
            teamPlayerList.HideMe();
        }
    }
    public void OnClickRules()
    {
        if (mainMenuTogs[3].isOn)
        {
            rules.ShowMe();
        }
        else
        {
            rules.HideMe();
        }
    }

    public void OnClickLogout()
    {
        if (mainMenuTogs[4].isOn)
        {
            logout.ShowMe();
        }
        else
        {
            logout.HideMe();
        }
    }

}
