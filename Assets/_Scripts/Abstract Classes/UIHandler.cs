using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIHandler : MonoBehaviour
{  
    public abstract void ShowMe();
    public abstract void HideMe();
    public abstract void OnBack();
}