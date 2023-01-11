using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Firebase.Storage;
using UnityEditor.SceneManagement;
using UnityEngine.Networking;
using Firebase.Extensions;




public class TeamHolderData : MonoBehaviour
{

    public TMP_Text teamA;
    public TMP_Text teamB;
    public TMP_Text Match;
    public TMP_Text teamAFullName;
    public TMP_Text teamBFullName;
    public string TeamA;
    public string TeamB;
    public string ID;
    public bool isPrimeGame;
    public Button Click;
    public Button MyMatchDetails;
    public TMP_Text time;
    public Image[] Image;
    bool isCount = false;
    public string timeValSave;
    public string timeFormat;
    string fileName;
    FirebaseStorage storage;
    StorageReference storageReference;
    private void Awake()
    {
        Click.onClick.AddListener(() => { OnClickButton(); });
        //  MyMatchDetails.onClick.AddListener(() => { OnClickButton();  })
        ;
    }

    private void OnDestroy()
    {
        GameController.Instance.UnSubscribeMatchDetails();
    }
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        if (isCount)
        {
            StopCoroutine(Timer(timeValSave));
            StartCoroutine(Timer(timeValSave));

        }

    }
    public void SetDetails(string teamAval, string teamBval, string id, string timeval, string _matchName)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        ID = id;
        TeamA = teamAval;
        TeamB = teamBval;


        foreach (var item in GameController.Instance.team)
        {

            if (item.Value.TeamName == teamAval)
            {
                fileName = item.Value.LogoURL;
                DisplayImage(Image[0]);
            }
            else if (item.Value.TeamName == teamBval)
            {
                fileName = item.Value.LogoURL;
                DisplayImage(Image[1]);
            }
        }


        if (gameObject.activeInHierarchy)
        {
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }

        isCount = true;

        SetFullCountryName();
    }


    public void SetFullCountryName()
    {
        GameController.Instance.countryFullName.Clear();
        GameController.Instance.countryFullName = new Dictionary<string, string>() { { "AUS", "Australia" }, { "IND", "India" },
        { "PAK", "Pakistan" },{ "ENG", "England" }};

        foreach (var item in GameController.Instance.countryFullName)
        {


            if (item.Key == TeamA)
            {
                teamAFullName.text = item.Value;
            }
            else if (item.Key == TeamB)
            {
                teamBFullName.text = item.Value;
            }
        }
    }

    public void OnClickButton()
    {

        UIController.Instance.ContestPanel.ShowMe();
        StartCoroutine(ContestHandler.Instance.SetUpcomingMatchPoolDetails(int.Parse(ID), TeamA, TeamB, timeFormat));
        UIController.Instance.MainMenuScreen.HideMe();
    }




    public void SetDetailsMymatches(string teamAval, string teamBval, string id, string timeval, string _matchName)
    {
        Match.text = _matchName;
        teamA.text = teamAval;
        teamB.text = teamBval;
        TeamA = teamAval;
        TeamB = teamBval;
        ID = id;
        Debug.Log(teamA);



        foreach (var item in GameController.Instance.countryFullName)
        {

            if (item.Key == TeamA)
            {
                teamAFullName.text = GameController.Instance.countryFullName[item.Value];
            }
            else if (item.Key == TeamB)
            {
                teamBFullName.text = GameController.Instance.countryFullName[item.Value];
            }
        }


        foreach (var item in GameController.Instance.team)
        {

            if (item.Value.TeamName == teamAval)
            {
                fileName = item.Value.LogoURL;
                DisplayImage(Image[0]);
            }
            else if (item.Value.TeamName == teamBval)
            {
                fileName = item.Value.LogoURL;
                DisplayImage(Image[1]);
            }
        }
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine(Timer(timeval));
            StartCoroutine(Timer(timeval));
        }
        isCount = true;



    }

    public void DisplayImage(Image _image)
    {
        Debug.Log(fileName + "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        StorageReference image = storageReference.Child(fileName);
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result), _image));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }
    public IEnumerator LoadImage(string MediaUrl, Image _image)
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
            _image.sprite = sprite;
        }
    }

    public IEnumerator Timer(string timeString)
    {

        timeValSave = timeString;
        if (string.IsNullOrWhiteSpace(timeValSave)) yield break;



        string timeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");



        timeValSave = timeValSave.Replace('/', '-');
        timeNow = timeNow.Replace('/', '-');


        var matchduration = DateTime.Parse(timeValSave) - DateTime.Parse(timeNow);

        var TimeDifference = matchduration;

        if (TimeDifference.Days * 24 + TimeDifference.Hours <= 0)
        {
            if (TimeDifference.Minutes <= 0 && TimeDifference.Seconds <= 0)
            {
                time.text = "Live";
                timeFormat = "Live";
            }

            else
            {
                time.text = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
                timeFormat = TimeDifference.Minutes + "m" + TimeDifference.Seconds + "s";
            }

        }
        else
        {
            time.text = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
            timeFormat = (TimeDifference.Days * 24 + TimeDifference.Hours) + "h" + TimeDifference.Minutes + "m";
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer(timeString));

    }

}
