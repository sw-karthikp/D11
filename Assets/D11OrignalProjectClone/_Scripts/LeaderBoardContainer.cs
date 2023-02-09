using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardContainer : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text Points;
  

    public void SetLeaderBoard(string _playerName ,string _points)
    {
        playerName.text = _playerName;
        Points.text = _points;
    }
}
