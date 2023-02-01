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
    public Image bg;
    public Sprite[] val;

    public void playerStatsVal(string _playerName,string _countryName ,string _points , Sprite _pic,bool selectedPlayer= false)
    {
        playerName.text = _playerName;
        if (selectedPlayer)
        {
            bg.sprite = val[0];
        }
       
        else

        {
            bg.sprite = val[1];
        }
        countryName.text= _countryName;
        points.text= _points;
        pic.sprite= _pic;
    }


}
