//using PlayFab;
using D11;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Xml;

class MyCustomBuilder : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{

    //List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        // Set the Build Settings window Scene list
        //EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        //editorBuildSettingsScenes.Clear();
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        //Debug.Log("MyCustomBuildProcessor.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);

        Debug.Log("MyCustomBuilder changing values");
        
        //if (!string.IsNullOrEmpty(StaticStrings.PhotonAppID))
        //{
        //    var myAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        //    myAppSettings.AppIdRealtime = StaticStrings.PhotonAppID;
        //}

        //if (!string.IsNullOrEmpty(StaticStrings.PlayfabTitleID))
        //{
        //    PlayFabSettings.TitleId = StaticStrings.PlayfabTitleID;
        //    PlayFabSettings.DeveloperSecretKey = StaticStrings.PlayfabSecretKey;
        //}


        // instantiate XmlDocument and load XML from file
        XmlDocument doc = new XmlDocument();
        string path = (Application.dataPath).Replace("/Assets", "");
        if (path.Contains("_clone_"))
        {
            path = path.Substring(0, path.Length - 8);
        }
        path = path + "/AndroidManifest.xml";
        doc.Load(path);

//        get a list of nodes - in this case, I'm selecting all <AID> nodes under
//         the<GroupAIDs> node -change to suit your needs

//         loop through all AID nodes
        XmlNodeList aNodes = doc.SelectNodes("/manifest/application/provider");
        foreach (XmlNode aNode in aNodes)
        {
            XmlAttribute idAttribute = aNode.Attributes["android:authorities"];

            if (idAttribute != null)
            {
                // if yes - read its current value
                string currentValue = idAttribute.Value;
                Debug.Log("Attribut found ::: " + currentValue);
                idAttribute.Value = StaticStrings.AppId;
            }
        }

        doc.Save((Application.dataPath) + "/Plugins/Android/AndroidManifest.xml");


        //if (!string.IsNullOrEmpty(StaticStrings.AppSceneName))
        //{
        //    EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        //    editorBuildSettingsScenes.Clear();
        //    editorBuildSettingsScenes.AddRange(scenes);

        //    var currentGameScenes = new List<EditorBuildSettingsScene>();
        //    foreach (EditorBuildSettingsScene gameScene in scenes)
        //    {
        //        //gameScene.enabled = gameScene.path.Equals(StaticStrings.AppSceneName);
        //        if (gameScene.path.Equals(StaticStrings.AppSceneName))
        //        {
        //            currentGameScenes.Add(new EditorBuildSettingsScene(gameScene.path, true));
        //            //currentGameScenes.Add(gameScene);
        //        }
        //    }

        //    /*foreach (var sceneAsset in m_SceneAssets)
        //    {
        //        string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        //        if (!string.IsNullOrEmpty(scenePath))
        //            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        //    }*/

        //    // Set the Build Settings window Scene list
        //    EditorBuildSettings.scenes = currentGameScenes.ToArray();
        //}


        if (!string.IsNullOrEmpty(StaticStrings.AppIconResName))
        {
            Texture2D icon = Resources.Load(StaticStrings.AppIconResName) as Texture2D;

            var platform = BuildTargetGroup.Android;
            var kind = UnityEditor.Android.AndroidPlatformIconKind.Legacy;
            var icons = PlayerSettings.GetPlatformIcons(platform, kind);

            //Assign textures to each available icon slot.
            for (var i = 0; i < icons.Length; i++)
            {
                icons[i].SetTextures(icon);
            }
            PlayerSettings.SetPlatformIcons(platform, kind, icons);
            /*List<Texture2D> iconList = new List<Texture2D>();
            for(int i = 0; i < icons.Length; i++)
            {
                iconList.Add(icon);
            }
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, iconList.ToArray(), IconKind.Application);*/
        }

        //if (!string.IsNullOrEmpty(StaticStrings.SplashBgName))
        //{
        //    Texture2D splashBG = Resources.Load(StaticStrings.SplashBgName) as Texture2D;
        //    Rect rec = new Rect(0, 0, splashBG.width, splashBG.height);

        //    PlayerSettings.SplashScreen.background = Sprite.Create(splashBG, rec, new Vector2(0, 0), .01f);
        //}

        //if (!string.IsNullOrEmpty(StaticStrings.SplashIconName))
        //{

        //    Texture2D splashBG = Resources.Load(StaticStrings.SplashIconName) as Texture2D;
        //    Rect rec = new Rect(0, 0, splashBG.width, splashBG.height);

        //    PlayerSettings.SplashScreenLogo splashlogo = new PlayerSettings.SplashScreenLogo();

        //    splashlogo.logo = Sprite.Create(splashBG, rec, new Vector2(0, 0), .01f);
        //    splashlogo.duration = 3.5f;

        //    Debug.LogError(splashBG.name);

        //    PlayerSettings.SplashScreen.logos[0] = splashlogo;
        //    Debug.Log(PlayerSettings.SplashScreen.logos[0].duration);
        //}


        if (!string.IsNullOrEmpty(StaticStrings.RoundedAppIconResName))
        {
            Texture2D icon = Resources.Load(StaticStrings.RoundedAppIconResName) as Texture2D;

            var platform = BuildTargetGroup.Android;
            var kind = UnityEditor.Android.AndroidPlatformIconKind.Round;
            var icons = PlayerSettings.GetPlatformIcons(platform, kind);

            //Assign textures to each available icon slot.
            for (var i = 0; i < icons.Length; i++)
            {
                icons[i].SetTextures(icon);

            }
            PlayerSettings.SetPlatformIcons(platform, kind, icons);
        }

        PlayerSettings.productName = StaticStrings.AppName;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, StaticStrings.AppId);
    }
}