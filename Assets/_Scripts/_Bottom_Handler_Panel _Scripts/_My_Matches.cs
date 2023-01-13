using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class _My_Matches : UIHandler
{

    [Header("ToggleHolder")]
    public Toggle[] toggles;

    [Header("ObjectsToEnable")]
    public GameObject[] objects;


    private void Awake()
    {
        toggles[0].onValueChanged.AddListener(delegate { OnValueChange(0); });
        toggles[1].onValueChanged.AddListener(delegate { OnValueChange(1); });
        toggles[2].onValueChanged.AddListener(delegate { OnValueChange(2); });
        toggles[3].onValueChanged.AddListener(delegate { OnValueChange(3); });
    }

    public override void ShowMe()
    {
        gameObject.SetActive(true);
    }
    public override void HideMe()
    {
        gameObject.SetActive(false);
    }
    public override void OnBack()
    {
        HideMe();
    }

    public void OnValueChange(int _index)
    {
        objects[_index].SetActive(toggles[_index].isOn ? true:false);       
    }
}
