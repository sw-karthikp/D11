using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleValueSet : MonoBehaviour
{
    public string runReport;
    public GameObject childCircle;
    public GameObject childellipse;
    public Transform parent;
    public DatabaseReference databaseReference;
    public static ToggleValueSet Instance;
    private void Awake()
    {
        Instance = this;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void StringToSet(string val)
    {
        runReport = val;
    }

    public void ToggleState(Toggle _tog)
    {
        if(_tog.isOn)
        {
            InstantiateChild(runReport);
        }
    }
    public void InstantiateChild(string val)
    {
        GameObject mprefab = Instantiate(childCircle, parent);
        ScoreSettingPanel.Instance.BallsCount++;
        mprefab.GetComponent<ChildScorePanel>().SetValueToChildContainer(val);
        ScoreSettingPanel.Instance.ScoreManagerController(val, ScoreSettingPanel.Instance.BallsCount.ToString(), ScoreSettingPanel.Instance.OversCount.ToString());

    }

    public void ClearAfterEveryOver()
    {
        foreach (Transform item in parent)
        {
            Destroy(item.gameObject);
        }
    }

   
}
