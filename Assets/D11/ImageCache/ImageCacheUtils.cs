using D11;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace D11
{
    public class CachedImage
    {
        public string uniqueId;
        public string imgUrl;
        public Sprite sprite;
        public bool isFallbackSprite = false;
        public DateTime cachedTime;
    }

    public class ImageCacheUtils : SingletonMonoBehaviour<ImageCacheUtils>
    {
        Dictionary<string, CachedImage> cachedImages = new Dictionary<string, CachedImage>();
        Dictionary<string, Action<Sprite>> imagesInProgress = new Dictionary<string, Action<Sprite>>();

        private void Start()
        {
            Application.lowMemory += OnLowMemory;
        }

        private void OnLowMemory()
        {
            LoggerUtils.LogError("Device running on low memory");
            if (cachedImages.Count > 5)
                cachedImages.Where(x => x.Value.cachedTime > DateTime.Now.AddMinutes(-10)).ToList().ForEach(x=> cachedImages.Remove (x.Key));
                
            Resources.UnloadUnusedAssets();
        }

        public void LoadFromCacheOrDownload(string url, string cacheKey = null, Sprite fallbackSprite = null, Action<Sprite> onComplete = null)
        {
            CachedImage cachedImage = new CachedImage();

            if (GetFromCachedImage((string.IsNullOrEmpty(cacheKey) ? url : cacheKey), out cachedImage))
            {
                if (onComplete != null)
                    onComplete.Invoke(cachedImage.sprite);
            }
            else
            {
                if (imagesInProgress.ContainsKey(url))
                {
                    if (onComplete == null)
                        imagesInProgress[url] = onComplete;
                    else
                        imagesInProgress[url] += onComplete;

                    return;
                }
                /* var www = UnityWebRequestTexture.GetTexture(url);
                 www.timeout = 30;
                 www.SendWebRequest(); //Web Request Sent
                                       //There may be a better wait to wait for the download to complete but this works for me for now
                                       //In an asyc function you can write an Inline Task to wait for. in this case I am waiting for the download data and waiting for 100 Milliseconds each loop
                 while (!www.isDone)
                     await Task.Delay(100);

                 while (!www.downloadHandler.isDone)
                     await Task.Delay(100);

                 if (www.result == UnityWebRequest.Result.ConnectionError
                     || www.result == UnityWebRequest.Result.ProtocolError
                     || www.result == UnityWebRequest.Result.DataProcessingError)
                 {
                     cachedImage.sprite = fallbackSprite;
                     cachedImage.isFallbackSprite = true;
                 }
                 else
                 {
                     var _tex = DoReScaleTex(((DownloadHandlerTexture)www.downloadHandler).texture, 512, 512);
                     cachedImage.sprite = CreateSpriteFromTex(_tex, _tex.width, _tex.height);
                 }

                 cachedImage.imgUrl = url;
                 cachedImage.uniqueId = (string.IsNullOrEmpty(cacheKey) ? url : cacheKey);

                 AddToCachedImage((string.IsNullOrEmpty(cacheKey) ? url : cacheKey), cachedImage, true);
                 www.Dispose();

                 onComplete.Invoke(cachedImage.sprite);*/

                imagesInProgress.Add(url, onComplete);

                WebApiManager.Instance.GetDownloadImage(url, (bool isSuccess, string error, Texture2D imageTex) => {

                    CachedImage cachedImage = new CachedImage();
                    cachedImage.imgUrl = url;
                    cachedImage.uniqueId = (string.IsNullOrEmpty(cacheKey) ? url : cacheKey);
                    cachedImage.cachedTime = DateTime.Now;

                    if (isSuccess)
                    {
                        imageTex = DoReScaleTex(imageTex, 512, 512);
                        cachedImage.sprite = CreateSpriteFromTex(imageTex, imageTex.width, imageTex.height);
                        imagesInProgress.Remove(url);
                    }
                    else
                    {
                        cachedImage.sprite = fallbackSprite;
                        cachedImage.isFallbackSprite = true;
                    }

                    AddToCachedImage((string.IsNullOrEmpty(cacheKey) ? url : cacheKey), cachedImage, true);

                    if(onComplete != null)
                        onComplete.Invoke(cachedImage.sprite);
                });

            }

            //return await Task.FromResult(cachedImage.sprite != null);
        }

        public void RetryFallbackImages()
        {
            cachedImages.Values.Where(x => x.isFallbackSprite).ToList().ForEach(x => LoadFromCacheOrDownload(x.imgUrl, x.uniqueId, x.sprite));
        }

        public void AddToCachedImage(string key, CachedImage value, bool forceUpdate = false)
        {
            if (!HasCachedImage(key))
            {
                cachedImages.Add(key, value);
            }
            else 
            if (forceUpdate)
            {
                cachedImages[key] = value;
            }
        }

        public bool GetFromCachedImage(string key, out CachedImage cachedImage)
        {
            return cachedImages.TryGetValue(key, out cachedImage);
        }

        public bool HasCachedImage(string key)
        {
            return cachedImages.ContainsKey(key);
        }

        public bool DeleteCachedImage(string key)
        {
            if(HasCachedImage(key))
                return cachedImages.Remove(key);

            return false;
        }

        public void DeleteAllCacheImages()
        {
            cachedImages.Clear();
        }

        public string ImageToBase64String(Texture2D tex)
        {
            // First convert the picture to byte[]
            byte[] imgByte = TextureToBytes(tex);
            //then
            return Convert.ToBase64String(imgByte);
        }
        public Texture2D TextureFromFile(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;

            if (System.IO.File.Exists(filePath))
            {
                fileData = System.IO.File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }

            return tex;
        }

        public byte[] TextureToBytes(Texture2D imageSource)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(imageSource.width, imageSource.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(imageSource, renderTex);
            //RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableTxt = new Texture2D(imageSource.width, imageSource.height);
            readableTxt.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableTxt.Apply();
            byte[] bytes = readableTxt.EncodeToPNG();
            Destroy(renderTex);
            return bytes;
        }

        public Texture2D TextureFromBytes(byte[] imageSource, int width, int height)
        {
            var tex = new Texture2D(width, height, TextureFormat.BGRA32, false);
            tex.LoadImage(imageSource);
            tex.Apply();
            return tex;
        }

        public Texture2D TextureFromBase64(string imageSource, int width, int height)
        {
            return TextureFromBytes(Convert.FromBase64String(imageSource), width, height);
        }

        public Sprite CreateSpriteFromTex(Texture2D spriteTexture, float width = 128f, float height = 128f)
        {
            return Sprite.Create(spriteTexture, new Rect(0, 0, width, height), Vector2.zero);
        }

        public Texture2D DoReScaleTex(Texture2D tex, int width, int height)
        {
            Texture2D scaled = new Texture2D(width, height, TextureFormat.BGRA32, true);
            if (Graphics.ConvertTexture(tex, scaled))
            {
                Destroy(tex);
                return scaled;
            }
            else
            {
                Destroy(scaled);
                return tex;
            }
        }
    }
}
