using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMatchStatus : MonoBehaviour
{
   public void ChangeMatch()
    {
        MatchHandler.instance.ChangeMatch(this.gameObject.transform.parent.name, this.gameObject);
    }
}
