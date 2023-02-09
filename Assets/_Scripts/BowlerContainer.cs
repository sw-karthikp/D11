using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BowlerContainer : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text Overs;
    public TMP_Text Maiden;
    public TMP_Text Run;
    public TMP_Text Wicket;
    public TMP_Text Economy;

    public void SetData(string _playerName, string _over, string _maiden, string _run, string _wicket, string _economy)
    {
        playerName.text = _playerName;
        Overs.text = _over;
        Maiden.text = _maiden;
        Run.text = _run;
        Wicket.text = _wicket;
        Economy.text = _economy;
    }
}
