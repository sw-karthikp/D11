using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static MatchSelection;

public class ToggleAdditionCount : MonoBehaviour
{

    public TMP_Text[] ToggleText;
    public TMP_Text[] team;
    int wkt = 0;
    int bat = 0;
    int ar = 0;
    int bowl = 0;
    int teamA = 0;
    int teamB = 0;

    public static ToggleAdditionCount Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void SetValue(int _wkt = 0, int _bat = 0,int _ar = 0,int _bowl = 0)
    {
        wkt = _wkt;
        bat = _bat;
        ar = _ar;
        bowl = _bowl;
        ToggleText[0].text = wkt.ToString();
        ToggleText[1].text = bat.ToString();
        ToggleText[2].text = ar.ToString();
        ToggleText[3].text = bowl.ToString();
    }
    public void SetValueTeam(int _teamA = 0, int _teamB = 0)
    {
        teamA = _teamA;
        teamB = _teamB;
        team[0].text = teamA.ToString();
        team[1].text = teamB.ToString();

    }
}
