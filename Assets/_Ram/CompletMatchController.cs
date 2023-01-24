using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CompletMatchController : MonoBehaviour
{
    [Header("Variables")]
    public Image teamALogo;
    public Image teamBLogo;
    public TMP_Text teamA;
    public TMP_Text teamB;
    public TMP_Text date;
    public TMP_Text type;
    public Toggle hotGame;

    [Header("Data")]
    public string HotGame;
    public string ID;
    public string TeamA;
    public string TeamB;
    public string Time;
    public string Type;
    public string teamAUrl;
    public string teamBUrl;

    FirebaseStorage storage;
    StorageReference storageReference;

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
    }

    public void SetCompleteMatchData(string _hotGame, string _id, string _teamA, string _teamB, string _time, string _type, string _teamAUrl, string _teamBUrl)
    {
        HotGame = _hotGame;
        ID = _id;
        TeamA = _teamA;
        TeamB = _teamB;
        Time = _time;
        Type = _type == "0" ? "T20" : "ODI";
        teamAUrl = _teamAUrl;
        teamBUrl = _teamBUrl;

        teamA.text = _teamA;
        teamB.text = _teamB;
        date.text = _time;
        type.text = _type == "0" ? "T20" : "ODI";



        LoadThisURL(teamAUrl, true);
        LoadThisURL(teamBUrl, false);
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
            Sprite sprite = Sprite.Create(text, new Rect(0, 0, text.width, text.height), new Vector2(text.width / 2, text.height / 2));

            if (value)
            {
                teamALogo.sprite = sprite;
            }
            else
            {
                teamBLogo.sprite = sprite;
            }

        }
        else
        {
            Debug.LogError($"Can't load this image {request.error}");
        }
    }
}
