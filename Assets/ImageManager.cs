using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour
{

    public static ImageManager Instance;
    string _basePath;

    private void Awake()
    {
        if(Instance !=null)
        {
            GameObject.Destroy(this);
            return;
        }
        Instance = this;
        _basePath = Application.persistentDataPath + "/Images/";
        if(!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    bool ImageExsists(string name)
    {
        return File.Exists(_basePath + name);
       
    }

   public void SaveImage(string name, byte[] bytes)
    {
        File.WriteAllBytes(_basePath + name, bytes);
    }

   public  byte[] LoadImage(string name)
    {
        byte[] bytes = new byte[0];

        if (ImageExsists(name))
        {
            bytes = File.ReadAllBytes(_basePath + name);
        }
        return bytes;

    }

    public Sprite BytesToSprite(byte[] bytes)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
     //  GameController.Instance.countrySpriteImage.Add(_teamName, sprite);
        return sprite;
    }

}
