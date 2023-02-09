using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatterContainer : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text playerStatus;
    public TMP_Text Runs;
    public TMP_Text Bowls;
    public TMP_Text fours;
    public TMP_Text sixs;
    public TMP_Text strikeRate;

    public void SetData(string _playerName,string _playerStatus,string _Runs,string _bowls,string _fours,string _six,string _strikeRate)
    {
        playerName.text = _playerName;
        playerStatus.text = _playerStatus;
        Runs.text = _Runs;
        Bowls.text = _bowls;
        fours.text = _fours;
        sixs.text = _six;
        strikeRate.text = _strikeRate;
    }
}
