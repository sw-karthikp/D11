using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

using System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

public class RulesHandler : UIHandler
{
    [SerializeField] private GameObject matchTypeToggle;
    [SerializeField] private Transform matchTypeToggleParent;

    [SerializeField] private GameObject categoryToggle;
    [SerializeField] private Transform categoryToggleParent;

    [SerializeField] private GameObject rulesDisplayToggle;
    [SerializeField] private Transform rulesDisplayToggleParent;

    [SerializeField] private GameObject ruleDetail;
    [SerializeField] private Transform ruleDetailParent;

    public DatabaseReference database;
    public List<Sport> sports= new List<Sport>();

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(GetRulesInDatabase());
    }

    private IEnumerator GetRulesInDatabase()
    {
        var task = FirebaseDatabase.DefaultInstance.GetReference("Ram").Child("Rules").GetValueAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.IsCompleted)
        {
            DataSnapshot dataSnapshot = task.Result;
            string data = dataSnapshot.GetRawJsonValue();

            //Sport sport = new Sport();
            sports = JsonConvert.DeserializeObject<List<Sport>>(data);

            DisplayProcess();    
        }
    }

    private void DisplayProcess()
    {
        foreach(var item in sports)
        {
            foreach (var item1 in item.Value)
            {
                GameObject matchType = Instantiate(matchTypeToggle, matchTypeToggleParent);
                matchType.name = item1.MatchType;
                matchType.transform.GetChild(1).GetComponent<TMP_Text>().text = item1.MatchType.ToString();
                matchType.GetComponent<Toggle>().group = matchTypeToggleParent.GetComponent<ToggleGroup>();
                matchType.GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnValueChangeMatchType(matchType); });

                //foreach (var item2 in item1.Points)
                //{
                //    GameObject category = Instantiate(categoryToggle, categoryToggleParent);
                //    category.name = item2.name;
                //    category.GetComponent<Toggle>().group = categoryToggleParent.GetComponent<ToggleGroup>();
                //    category.transform.GetChild(1).GetComponent<TMP_Text>().text = item2.name.ToString();
                //    category.GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnValueChangeCateogry(category); });
                //}
            }
        }

        foreach (Transform item in matchTypeToggleParent)
        {
            
            if (matchTypeToggleParent.GetChild(0) == item)
            {
                //Debug.Log(item.name + "***********************");
                matchTypeToggleParent.GetComponent<ToggleGroup>().allowSwitchOff = true;
                item.GetComponent<Toggle>().isOn = true;
                matchTypeToggleParent.GetComponent<ToggleGroup>().allowSwitchOff = false;
            }
        }
    }

    private void OnValueChangeMatchType(GameObject matchType)
    {
        if (matchType.GetComponent<Toggle>().isOn)
        {
            foreach (var item in sports)
            {
                foreach (var item1 in item.Value)
                {
                    if (item1.MatchType == matchType.name)
                    {
                        foreach (var item2 in item1.Points)
                        {
                            GameObject category = Instantiate(categoryToggle, categoryToggleParent);
                            category.name = item2.name;
                            category.GetComponent<Toggle>().group = categoryToggleParent.GetComponent<ToggleGroup>();
                            category.transform.GetChild(1).GetComponent<TMP_Text>().text = item2.name.ToString();
                            category.GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnValueChangeCateogry(category); });
                        }
                    }
                }
            }

       
        }
        else
        {
            if (categoryToggleParent.childCount > 0)
            {
                foreach (Transform item in categoryToggleParent)
                {
                    Destroy(item.gameObject);
                }
            }
        }

        foreach (Transform item in categoryToggleParent)
        {
            //item.GetComponent<Toggle>().isOn = true;
            if (categoryToggleParent.GetChild(0) == item)
            {
                Debug.Log(categoryToggleParent.GetChild(0).name);
                Debug.Log(item.gameObject.name + "***********************");
                //Debug.Log(item.name + "***********************");
                //  categoryToggleParent.GetComponent<ToggleGroup>().allowSwitchOff = true;
                Debug.Log(item);
                item.GetComponent<Toggle>().isOn = true;
              //  categoryToggleParent.GetComponent<ToggleGroup>().allowSwitchOff = false;
            }
        }
    }

    private void OnValueChangeCateogry(GameObject gameObject)
    {
        if (gameObject.GetComponent<Toggle>().isOn)
        {
            foreach (var item in sports)
            {
                foreach (var item1 in item.Value)
                {
                    foreach (var item2 in item1.Points)
                    {
                        if(item2.name == gameObject.name)
                        {
                            GameObject rules = Instantiate(rulesDisplayToggle, rulesDisplayToggleParent);
                            rules.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = item2.name;
                            ruleDetailParent = rules.transform.GetChild(1).transform;

                            foreach (var item3 in item2.Value)
                            {
                                // first
                                GameObject details = Instantiate(ruleDetail, ruleDetailParent);
                                details.transform.GetChild(0).GetComponent<TMP_Text>().text = item3.Detail;
                                details.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = item3.Value.ToString();
                                // second


                            }
                        }
                    }
                }
            }
        }
        else
        {
            if(rulesDisplayToggleParent.childCount > 0)
            {
                foreach(Transform item in rulesDisplayToggleParent)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }


    [Serializable]
    public class Sport
    {
        public string SportType;
        public List<ListSubValues> Value = new();
    }

    [Serializable]
    public class ListSubValues
    {
        public string MatchType;
        public List<Points> Points = new();
    }

    [Serializable]
    public class Points
    {
        public string name;
        public List<RuleDetails> Value = new();
    }

    [Serializable]

    public class RuleDetails
    {
        public string Detail;
        public string ID;
        public string Value;
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
