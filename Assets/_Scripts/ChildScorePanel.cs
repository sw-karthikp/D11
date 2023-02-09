using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChildScorePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;

    public void SetValueToChildContainer(string _run)
    {
        valueText.text = _run;
    }
}
