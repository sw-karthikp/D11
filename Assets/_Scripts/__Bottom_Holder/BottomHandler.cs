using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class BottomHandler : MonoBehaviour
{

    public static BottomHandler Instance;
    public Toggle[] togs;
    public UIHandler[] objects;
    private void Awake()
    {
        Instance= this;
        togs[0].onValueChanged.AddListener(delegate { OnclickTogs(0); });
        togs[1].onValueChanged.AddListener(delegate { OnclickTogs(1); });
        togs[2].onValueChanged.AddListener(delegate { OnclickTogs(2); });
        togs[3].onValueChanged.AddListener(delegate { OnclickTogs(3); });
        togs[4].onValueChanged.AddListener(delegate { OnclickTogs(4); });
    }

    public void ResetScreen()
    {
        UIController.Instance.ContestPanel.HideMe();
        UIController.Instance.SelectMatchTeam.HideMe();
        togs[1].isOn = true;
    }

    public void ResetToggle()
    {
        togs[0].isOn = true;
    }

    public void OnclickTogs(int _index)
    {
        if (togs[_index].isOn)
        {
            objects[_index].ShowMe();
        }
        else
        {
            objects[_index].HideMe();
        }
    }

}


