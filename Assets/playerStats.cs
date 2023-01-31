using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerStats : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text countryName;
    public TMP_Text points;
    public Image pic;

    public void playerStatsVal(string _playerName,string _countryName ,string _points , Sprite _pic,bool selectedPlayer= false)
    {
        if(selectedPlayer)
        playerName.text= _playerName;
        else
            playerName.text = "Works";
        countryName.text= _countryName;
        points.text= _points;
        pic.sprite= _pic;
    }


}
