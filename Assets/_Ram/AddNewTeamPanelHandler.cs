using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using UnityEditor;
using System.IO;
using System;
using Newtonsoft.Json;
using Firebase.Extensions;

using UnityEngine.UI;
using Firebase.Storage;
using UnityEngine.Networking;

public class AddNewTeamPanelHandler : UIHandler
{
    public static AddNewTeamPanelHandler instance;

    public DatabaseReference database;

    public TMP_InputField teamName;
    [SerializeField] private string teamFormat;
    [SerializeField] private string teamLeagueFormat;
    [SerializeField] private string colorName;

    public List<TeamDetail> teamDetails;

    private int teamCount;

    [Header("----------Team Display--------")]
    //private GameObject emptyObject;
    [SerializeField] private GameObject teamDisplayPref;
    [SerializeField] private Transform teamDisplayParent;
    private GameObject currentMatchTypeToggle;

    private bool isSubscribed;

    // ----- Storage ----- //
    FirebaseStorage storage;
    StorageReference storageReference;

    [Header("Logo Reference")]
    public List<Sprite> logoSprites;

    private void Start()
    {
        instance = this;

        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateNewTeamInFirebase()
    {
        FirebaseDatabase.DefaultInstance.GetReference("CommonValues").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                IDictionary data1 = (IDictionary)dataSnapshot.Value;

                if (string.IsNullOrEmpty(data1["TeamCount"].ToString()))
                {
                    data1["TeamCount"] = 0;
                }
                else
                {
                    teamCount = int.Parse(data1["TeamCount"].ToString());
                    teamCount++;
                }

                CreateThisData(teamCount.ToString());
            }
        });
    }

    private void CreateThisData(string teamKey)
    {
        string teamKeyIs;
        
        FirebaseDatabase.DefaultInstance.GetReference("Ram/Team").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                foreach (var item in dataSnapshot.Children)
                {
                    teamKeyIs = item.Key;

                    IDictionary dataa = (IDictionary)item.Value;

                    if (teamName.text == dataa["TeamName"].ToString())
                    {
                        Debug.Log("Matched \\\\\\ [TeamKey] is: " + teamKeyIs);

                        Dictionary<string, object> updateTeamData = new Dictionary<string, object>();
                        updateTeamData.Add(teamFormat, "Type");

                        database.Child("Ram").Child("Team").Child(teamKeyIs).Child("PlayerDetails").UpdateChildrenAsync(updateTeamData);
                        return;
                    }
                    else
                    {
                        Debug.Log("Not matched //////");
                    }
                }

                TeamDetail teamDetail = new TeamDetail();

                teamDetail.PlayerDetails.Add(teamFormat, "t20Value");
                teamDetail.TeamName = teamName.text;
                teamDetail.LogoURL = FileBrowserUpdate.instance.path;

                string json = JsonConvert.SerializeObject(teamDetail);

                Debug.Log(json);

                //teamDetails.Add(JsonConvert.DeserializeObject<TeamDetail>(json));
                database.Child("Ram").Child("Team").Child(teamKey).SetRawJsonValueAsync(json);
                database.Child("CommonValues").Child("TeamCount").SetRawJsonValueAsync(teamKey);
            }
        });

        Debug.Log("Successfully team created... " + teamKey);
    }

    public void ShowThisMatchTypeTeams(GameObject gameObject)
    {
        string matchType = gameObject.GetComponent<Transform>().Find("Text (TMP)").GetComponent<TMP_Text>().text;
        string url = "";

        if (gameObject.GetComponent<Toggle>().isOn)
        {
            FirebaseDatabase.DefaultInstance.GetReference("Ram/Team").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot dataSnapshot = task.Result;

                    foreach (var item in dataSnapshot.Children)
                    {
                        foreach (var item1 in item.Children)
                        {
                            if(item1.Key == "LogoURL")
                            {
                                url = item1.Value.ToString();
                            }

                            foreach (var item2 in item1.Children)
                            {
                                if (item2.Key == matchType)
                                {
                                    GameObject gameObject1 = Instantiate(teamDisplayPref, teamDisplayParent);
                                    gameObject1.name = url;

                                    Color newColor2 = Color.red;
                                    
                                    if (ColorUtility.TryParseHtmlString(colorName, out newColor2))
                                    {
                                        gameObject1.transform.GetChild(0).GetComponent<Image>().color = newColor2;   
                                    }

                                    storage = FirebaseStorage.DefaultInstance;
                                    storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
                                    StorageReference image = storageReference.Child(url);
                                    image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
                                    {
                                        if (!task.IsFaulted && !task.IsCanceled)
                                        {
                                            StartCoroutine(LoadImage(task.Result.ToString(), gameObject1));
                                        }
                                        else
                                        {
                                            Debug.Log(task.Exception);
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            });
        }
        else
        {
            for (int i = 0; i < teamDisplayParent.childCount; i++)
            {
                Destroy(teamDisplayParent.transform.GetChild(i).gameObject);
            }
        }
    }

    public IEnumerator LoadImage(string MediaUrl, GameObject gameObject)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

            gameObject.GetComponent<Image>().sprite = sprite;
        }
    }

    //public void SubscribePlayerDetails()
    //{

    //    FirebaseDatabase.DefaultInstance
    //  .GetReference("Ram/Team")
    //  .ValueChanged += HandleValueChanged;
    //}

    //public void UnSubscribePlayerDetails()
    //{
    //    FirebaseDatabase.DefaultInstance
    //  .GetReference("Ram/Team")
    //  .ValueChanged -= HandleValueChanged;
    //}


    //void HandleValueChanged(object sender, ValueChangedEventArgs args)
    //{
    //    ShowThisMatchTypeTeams(currentMatchTypeToggle);
    //}

    #region other func
    public void SelectedColor(string color)
    {
        Color newColor1;
        colorName = color;
        string colorCode;

        switch (color)
        {
            case "Red":
                {
                    colorCode = "#FF0606";
                    break;
                }
            case "Yellow":
                {
                    colorCode = "#FCD527";
                    break;
                }
            case "Blue":
                {
                    colorCode = "#5886FF";
                    break;
                }
            case "LightBlue":
                {
                    colorCode = "#496DFF";
                    break;
                }
            case "LightViolet":
                {
                    colorCode = "#E44BFF";
                    break;
                }
            case "DarkViolet":
                {
                    colorCode = "#703ABA";
                    break;
                }
            case "Orange":
                {
                    colorCode = "#F87947";
                    break;
                }
            case "Green":
                {
                    colorCode = "#56F556";
                    break;
                }
            default:
                {
                    colorCode = "#FFFFFF";
                    break;
                }

        }

        if(ColorUtility.TryParseHtmlString(colorCode, out newColor1))
        {
            foreach (Transform item in teamDisplayParent)
            {
                item.transform.GetChild(0).GetComponent<Image>().color = newColor1;//;(Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
            }
        }
    }

    public void OnCategoryChange(GameObject gameObject)
    {
        if(gameObject.transform.name == "TeamFormatselection")
        {
            int value = gameObject.GetComponent<TMP_Dropdown>().value;
            teamFormat = gameObject.GetComponent<TMP_Dropdown>().options[value].text;
        }
        else
        {
            int value = gameObject.GetComponent<TMP_Dropdown>().value;
            teamLeagueFormat = gameObject.GetComponent<TMP_Dropdown>().options[value].text;
        }
    }

    [Serializable]
    public class TeamDetail
    {
        public Dictionary<string, string> PlayerDetails = new Dictionary<string, string>();

        public string TeamName;        
        public string LogoURL;
    }

    public override void HideMe()
    {
        AdminUIController.Instance.RemoveFromOpenPages(this);
        this.gameObject.SetActive(false);
    }
    public override void ShowMe()
    {
        AdminUIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);
    }

    public override void OnBack()
    {

    }
    #endregion
}
