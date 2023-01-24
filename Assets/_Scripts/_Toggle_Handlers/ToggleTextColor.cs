using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ToggleTextColor : MonoBehaviour
{


    public TMP_Text ToggleText;
    public Color32 CheckmarkColorOn = new Color32(231, 0, 0, 235);
    public Color32 ChecmarkColorOff = new Color32(134,134,134,255);


    private void OnEnable()
    {
        this.gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleTextColorstates(); });
    }
    public void ToggleTextColorstates()
    {
       
        if (this.GetComponent<Toggle>().isOn)
        {
            ToggleText.color = CheckmarkColorOn;
        }
        else
        {
            ToggleText.color = ChecmarkColorOff;
        }
    }


}
