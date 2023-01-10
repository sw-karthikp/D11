using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Firebase.Storage;
using UnityEditor.SceneManagement;
using Firebase.Extensions;
using System;

public class LiveMatchController : MonoBehaviour
{
    public Button StartScore;
    [Header("Variables")]
    public TMP_Text date;
    public TMP_Text teamA;
    public TMP_Text teamB;
    public TMP_Text matchType;
    public string matchID;
    public Image teamALogo;
    public Image teamBLogo;
    public string teamAulr;
    public string teamBulr;
    //  public TMP_Text PrizeValue;

    FirebaseStorage storage;
    StorageReference storageReference;

    private void Awake()
    {
        StartScore.onClick.AddListener(() => { OnclickScore();  });
    }

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
    }


    public void OnclickScore()
    {
        AdminUIController.Instance.ScoreSettingScreen.ShowMe();
        ScoreSettingPanel.Instance.SetCurrentMacthValue(teamA.text,teamB.text, matchID);
    }

    public void SetValueToLiveMatch(string _date, string _matchType ,string  _teamA, string _teamB, string _matchId, string _teamAurl, string _teamBurl)
    {
        date.text = _date;
        teamA.text = _teamA;
        teamB.text = _teamB;
        matchType.text = _matchType == "0" ? "T20" : "ODI";
        matchID = _matchId;
        teamAulr = _teamAurl;
        teamBulr = _teamBurl;

        LoadThisURL(_teamAurl, true);
        LoadThisURL(_teamBurl, false);
    }
    public void LoadThisURL(string url, bool value)
    {
        //Debug.Log(url);

        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        StorageReference image = storageReference.Child($"{url}");
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result), value));

            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    private IEnumerator LoadImage(string url, bool value)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            Texture2D text = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(text, new Rect(0,0, text.width, text.height), new Vector2(text.width/2, text.height/2));

            if (value)
            {
                teamALogo.sprite= sprite;
            }
            else
            {
                teamBLogo.sprite= sprite;
            }

        }
        else
        {
            Debug.LogError($"Can't load this image {request.error}");
        }
    }
}
