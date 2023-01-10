using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MorePageHandler : UIHandler
{
    public static MorePageHandler Instance;
    public TMP_Text matchTitle;

    private void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }
}
