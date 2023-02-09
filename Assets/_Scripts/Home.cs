using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : UIHandler
{
    public override void ShowMe()
    {
        gameObject.SetActive(true);
        MainMenu_Handler.Instance.OnValueChange(0);

    }
    public override void HideMe()
    {
       
        gameObject.SetActive(false);
    }
    public override void OnBack()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
