using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinnerContainer : MonoBehaviour
{
    public Sprite[] _rankSprite;
    public TMP_Text rank;
    public TMP_Text entry;
    public Image _rankImage;
    public GameObject medalOne;
    

    public void setRank(string _rank ,string _entry)
    {


        medalOne.SetActive(_rank == "1" ? true : false);
        _rankImage.sprite = _rank == "1" ? _rankSprite[0] : _rankSprite[1];
        rank.text = _rank;
        entry.text = _entry;

    }

}
