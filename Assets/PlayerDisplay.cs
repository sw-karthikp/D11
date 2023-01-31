using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{

    public TMP_Text playerName;
    public GameObject Cp;
    public GameObject Vvcp;
    public Image pic;

    public void SetPlayerDetails(string name ,bool cp ,bool Vcp ,Sprite _pic)
    {
        playerName.text = name;
        Cp.SetActive(cp);
        Vvcp.SetActive(Vcp);
        //pic.sprite= _pic;
    }

}
