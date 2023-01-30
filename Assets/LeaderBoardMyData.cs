using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardMyData : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text teamCount;
    public TMP_Text points;
    public TMP_Text rank;
    public Color val1;
   public Color val2;
    public void SetData(string _playerName ,int _teamCount ,string _points ,string _rank,string _playerID)
    {
        playerName.text = _playerName;
        teamCount.text = $"T{_teamCount}";
       // points.text = _points.ToString();
        rank.text = _rank.ToString();
        if(_playerID == GameController.Instance.myUserID)
        {
            this.GetComponent<Image>().color = val1;

        }
        else
        {

            this.GetComponent<Image>().color = val2;
        }
      
    }
}
