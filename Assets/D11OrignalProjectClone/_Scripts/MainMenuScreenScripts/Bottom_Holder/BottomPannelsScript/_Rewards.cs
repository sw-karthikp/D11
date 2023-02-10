using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Rewards : UIHandler
{
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
}
