using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using Firebase.Extensions;
using static UnityEditor.Progress;

public class MainMenu_Handler : UIHandler
{
    public ScrollRect rect;
    public TMP_Text _playerName;
    public TMP_Text _playerId;
    public GameObject matchprefabHotTable;
    public GameObject matchprefabComingMatch;
    public Transform[] parentHotTable;
    public Transform[] parentUpComingMatch;
    public static MainMenu_Handler Instance;
    public VerticalLayoutGroup[] verticle;
    public GameObject[] matchTypes;
    public Toggle[] togs;
    public Image[] img;
    public GameObject[] hotGamesObj;
    public Transform Slider;
    public GameObject[] point;
    public Ease _ease;
    FirebaseFirestore db;
    private void Awake()
    {
        Instance = this;
        togs[0].onValueChanged.AddListener(delegate { OnValueChange(0); });
        togs[1].onValueChanged.AddListener(delegate { OnValueChange(1); });
        togs[2].onValueChanged.AddListener(delegate { OnValueChange(2); });
        togs[3].onValueChanged.AddListener(delegate { OnValueChange(3); });
        db = FirebaseFirestore.DefaultInstance;
    }

    private void Start()
    {
        rect.verticalNormalizedPosition = 1;

        GameController.Instance.SubscribeMatchDetails();
        GameController.Instance.SubscribePlayerDetails();
        GameController.Instance.SubscribeMatchPools();
        GameController.Instance.SubscribePlayers();
        GameController.Instance.SubscribeSelectedMatchDetails();
       // FetchName();
    }

    public void FetchName()
    {
        DocumentReference docRef = db.Collection("users").Document(PlayerPrefs.GetString("userId"));
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> city = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city)
                {
                    Debug.Log(pair.Key);
                   // GameController.Instance.myName = city["Name"].ToString();
                }
                //    GameController.Instance.myData = snapshot.ConvertTo<UserDetails>();

            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });

    }

    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);

        _playerId.text = PlayerPrefs.GetString("userName");
        _playerName.text = PlayerPrefs.GetString("userId");
        togs[0].isOn = true;


    }


    private void OnDisable()
    {
        GameController.Instance.UnSubscribeMatchDetails();
    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);

    }
    public override void OnBack()
    {

    }

    public void OnValueChange(int _index)
    {
        if (togs[_index].isOn)
        {
            //foreach (var item in GameController.Instance.match.Values)
            //{
            //    //foreach (var item1 in item.Values)
            //    //{


            //    //    hotGamesObj[_index].SetActive(false);
            //    //    if (item1.Type == _index)
            //    //    {
            //    //        if (item1.HotGame == true)
            //    //        {
            //    //            hotGamesObj[_index].SetActive(true);
            //    //            break;

            //    //        }
            //    //    }
            //    //}
            //}
            if (GameController.Instance.mymatchesGlobalRef != null && GameController.Instance.mymatchesGlobalRef.Count>=1)
            {
                foreach (var item in GameController.Instance.mymatchesGlobalRef.Values)
                {
                    foreach (var item1 in item.Values)
                    {


                        hotGamesObj[_index].SetActive(false);
                        if (item1.Type == _index)
                        {

                            hotGamesObj[_index].SetActive(true);
                            break;


                        }
                    }
                }
            }
            else
            {
                hotGamesObj[_index].SetActive(false);
            }


            img[_index].color = new Color(0.7764707f, 0.1058824f, 0.1372549f, 1);
            matchTypes[_index].SetActive(true);
            Slider.DOKill();
            Slider.DOMove(point[_index].transform.position, 0.1f).SetEase(_ease);
            SetUpcomingMatchDetails(_index);
            StartCoroutine(MySelectedMatches(_index));
        }
        else
        {
            img[_index].color = new Color(0.5254f, 0.5254f, 0.5254f, 1);
            verticle[_index].childControlWidth = false;
            matchTypes[_index].SetActive(false);

        }
    }


    public void SetUpcomingMatchDetails(int toggleindex)
    {

        foreach (Transform child in parentHotTable[toggleindex])
        {

            child.gameObject.SetActive(false);
        }
        foreach (Transform child in parentUpComingMatch[toggleindex])
        {
            child.gameObject.SetActive(false);
        }



      






        foreach (var item in GameController.Instance.match)
        {
            foreach (var item1 in item.Value.Values)
            {
                if (item.Key == MatchTypeStatus.Upcoming.ToString())
                {
                    if (togs[toggleindex].isOn)
                    {
                        if (item1.Type == toggleindex)
                        {
                            if (item1.HotGame)
                            {


                                //PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("HotGameHolder");
                                // mprefabObj.transform.SetParent(parentHotTable[toggleindex]);
                                // mprefabObj.gameObject.SetActive(true);
                                // mprefabObj.gameObject.name = item1.ID;
                                // string timeStringVal = item1.Time;
                                // Debug.Log(item1.ID.ToString() +"*****************");
                                // mprefabObj.gameObject.GetComponent<TeamHolderData>().SetDetails(item1.TeamA, item1.TeamB, item1.ID.ToString(), timeStringVal,"ICC MENS CRICKET");
                                // Canvas.ForceUpdateCanvases();

                            }
                            else
                            {
                                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("UpcomingGameHolder");
                                mprefabObj.transform.SetParent(parentUpComingMatch[toggleindex]);
                                mprefabObj.gameObject.SetActive(true);
                                mprefabObj.gameObject.name = item1.ID;
                                string timeString = item1.Time;
                                Debug.Log(item1.ID.ToString() + "*****************");
                                mprefabObj.gameObject.GetComponent<TeamHolderData>().SetDetails(item1.TeamA, item1.TeamB, item1.ID.ToString(), timeString, "ICC MENS CRICKET");
                                Canvas.ForceUpdateCanvases();

                            }
                        }
                    }
                }
            }
        }


       
    }

    IEnumerator MySelectedMatches(int toggleindex)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var item2 in GameController.Instance.mymatchesGlobalRef)
        {

            if (togs[toggleindex].isOn)
            {

                foreach (var item3 in item2.Value)
                {
                    if (item3.Value.Type == toggleindex)
                    {
                        PoolItems mprefabObj1 = PoolManager.Instance.GetPoolObject("MyMatchData");
                        mprefabObj1.transform.SetParent(parentHotTable[toggleindex]);
                        mprefabObj1.gameObject.SetActive(true);
                        mprefabObj1.name = item3.Value.ID;
                        if (item2.Key == "Complete")
                        {
                            mprefabObj1.GetComponent<MyMatchData>().SetDetails(item3.Value.TeamA, item3.Value.TeamB, item3.Value.ID.ToString(), item3.Value.Time, "ICC MENS CRICKET", 2);

                        }

                        if (item2.Key == "Live")
                        {
                            mprefabObj1.GetComponent<MyMatchData>().SetDetails(item3.Value.TeamA, item3.Value.TeamB, item3.Value.ID.ToString(), item3.Value.Time, "ICC MENS CRICKET", 1);

                        }

                        if (item2.Key == "Upcoming")
                        {
                            mprefabObj1.GetComponent<MyMatchData>().SetDetails(item3.Value.TeamA, item3.Value.TeamB, item3.Value.ID.ToString(), item3.Value.Time, "ICC MENS CRICKET", 0);

                        }
                        Canvas.ForceUpdateCanvases();
                    }

                }
            }
        }
    }
}
