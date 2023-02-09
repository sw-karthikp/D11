using System.Collections;
using System.Collections.Generic;
using TMPro;

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
        if(!string.IsNullOrWhiteSpace(_points))
            points.text = _points;
        rank.text = "#"+_rank.ToString();
        if(_playerName == GameController.Instance.myData.Name)
        {
            this.GetComponent<Image>().color = val1;

        }
        else
        {

            this.GetComponent<Image>().color = val2;
        }
      
    }
}
