//using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleFileBrowser;
using System;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
//using static System.Net.Mime.MediaTypeNames;

public class PlayerProfile : MonoBehaviour
{
    #region Another FIleBrowser
    //public RawImage rawImage;
    //public void OpenFileBrowser()
    //{
    //    var bp = new BrowserProperties();
    //    bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
    //    bp.filterIndex = 0;
    //    new FileBrowser().OpenFileBrowser(bp, path =>
    //    {
    //        StartCoroutine(LoadImage(path));
    //    });
    //}
    //IEnumerator LoadImage(string path)
    //{
    //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
    //    {
    //        yield return uwr.SendWebRequest();
    //        if (uwr.isNetworkError || uwr.isHttpError)
    //        {
    //            Debug.Log(uwr.error);
    //        }
    //        else
    //        {
    //            var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
    //            rawImage.texture = uwrTexture;
    //        }
    //    }
    //}
    #endregion

    public static PlayerProfile instance;
    public RawImage rawImage;
    //public TMP_Text path;
    public string fileName;
    public string path;
    FirebaseStorage storage;
    StorageReference storageReference;
    void Start()
    {
        instance = this;

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com"); //sw-multiplayer.appspot.com

    }
    public void OnClickUploadBtn()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
        //deleteFiles();
    }
    IEnumerator ShowLoadDialogCoroutine()
    {

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);
        StorageMetadata storageMetaData;
        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);
            Debug.Log("File Seleceted");
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            fileName = UnityEngine.Random.Range(1000, 9999).ToString();
            StorageReference uploadRef = storageReference.Child($"uploads/PlayerProfile/{fileName}.png");
            Debug.Log("File upload started");
            uploadRef.PutBytesAsync(bytes).ContinueWithOnMainThread((task) =>
            {
                //displaying URL
                StorageMetadata meta = task.Result;
                //var msg = task.Result;
                Debug.Log("Path: ---------->> "+meta.Path);
                path = meta.Path;
                //path.text = "Path :" + meta.Path;
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                }
                else
                {
                    Debug.Log("File Uploading Success" + task);
                    DisplayImage();
                }
            });
        }
    }
    public void DisplayImage()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        StorageReference image = storageReference.Child($"uploads/PlayerProfile/{fileName}.png");
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result)));

            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }
    public IEnumerator LoadImage(string MediaUrl)
    {
        Debug.Log(MediaUrl);
        
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    public IEnumerator DisplayPlayerProfile(string MediaUrl, GameObject gameObject)
    {
        Debug.Log(MediaUrl);
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://sw-d11.appspot.com");
        StorageReference image = storageReference.Child(MediaUrl);

        var task = image.GetDownloadUrlAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);
        
        if(!task.IsFaulted || !task.IsCanceled)
        {
            StartCoroutine(LoadThisProfile(task.Result.ToString(), gameObject));
        }
        else
        {
            Debug.Log(task.Exception);
        }

    }

    public IEnumerator LoadThisProfile(string MediaUrl, GameObject gameObject)
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
            
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

            Debug.Log("player profile loaded....");
        }
    }
    #region DeleteFiles
    //  public void deleteFiles()
    //  {
    //StorageReference deleteRef = storageReference.Child("uploads/");
    //deleteRef.DeleteAsync().ContinueWithOnMainThread(task =>
    //{
    //	if(task.IsCompleted)
    //          {
    //		Debug.Log("Done");
    //          }
    //	else
    //          {
    //		Debug.Log("Failed");
    //          }
    //});
    //  }
    #endregion
    public void storeData()
    {
    }
}