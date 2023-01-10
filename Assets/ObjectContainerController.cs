using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainerController : MonoBehaviour
{

}
#region Match

[Serializable]
public class MatchStatus
{
    public bool HotGame;
    public int ID;
    public string Time;
    public string TeamA;
    public string TeamB;
    public int Type;
}

#endregion

