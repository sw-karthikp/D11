using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static MatchSelection;
using System.Linq;

public class captinslectionHandler : MonoBehaviour
{
    public TMP_Text _name;
    public Toggle Captain;
    public Toggle ViceCaptian;
    public PlayerSelectedForMatch val;
    public Image playerPic;


    private void Awake()
    {

 
        Captain.onValueChanged.AddListener(delegate { OnValueChangedCaptain(); });
        ViceCaptian.onValueChanged.AddListener(delegate { OnValueChangedViceCaptain(); });
    }
    public void Setval(string playerName , PlayerSelectedForMatch Data)
    {
        _name.text = playerName;
        val = Data;
        playerPic.sprite = GameController.Instance.playerPic.First(x => x.Key == Data.PlayerID).pic;
    }

    public void OnValueChangedCaptain()
    {
        captainSelection.Instance.CheckForToggle1(Captain);
        if (Captain.isOn)
        {
            val.isCaptain = true;


        }
        else
        {
            val.isCaptain = false;

        }
    }

    public void OnValueChangedViceCaptain()
    {
        captainSelection.Instance.CheckForToggle2(ViceCaptian);
        if (ViceCaptian.isOn)
        {
            val.isViceCaptain = true;

        }
        else
        {
            val.isViceCaptain = false;
  
        }
    }
}
