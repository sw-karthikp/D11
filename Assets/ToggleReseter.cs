using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleReseter : MonoBehaviour
{
    public ScrollRect Verticle;
    public ScrollRect Horizontal;


    private void OnEnable()
    {
        Verticle.verticalNormalizedPosition= 0;
        Horizontal.horizontalNormalizedPosition= 1;
    }

}
