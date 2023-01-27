using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public abstract class UIHandler : MonoBehaviour
{ 
    public abstract void ShowMe();
    public abstract void HideMe();
    public abstract void OnBack();
}