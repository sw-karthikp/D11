using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private void Awake()
    {
        Instance = this;
        togs[0].onValueChanged.AddListener(delegate { OnvalueChangeT20(); });
        togs[1].onValueChanged.AddListener(delegate { OnvalueChangeODI(); });
        togs[2].onValueChanged.AddListener(delegate { OnvalueChangeTEST(); });
        togs[3].onValueChanged.AddListener(delegate { OnvalueChangeT10(); });

    }

    private void Start()
    {

        GameController.Instance.SubscribeMatchDetails();
        //GameController.Instance.SubscribePlayerDetails();
        GameController.Instance.SubscribeMatchPools();
        //GameController.Instance.SubscribePlayers();
        rect.verticalNormalizedPosition = 1;




    }

    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);

        _playerId.text = PlayerPrefs.GetString("userName");
        _playerName.text = PlayerPrefs.GetString("userId");
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

    public void OnvalueChangeT20()
    {
        if (togs[0].isOn)
        {
            foreach (var item in GameController.Instance.match.Values)
            {
                foreach (var item1 in item.Values)
                {


                    hotGamesObj[0].SetActive(false);
                    if (item1.Type == 0)
                    {
                        if (item1.HotGame == true)
                        {
                            hotGamesObj[0].SetActive(true);
                            break;

                        }
                    }
                }
            }

            img[0].color = new Color(0.7764707f, 0.1058824f, 0.1372549f, 1);
            matchTypes[0].SetActive(true);
            SetUpcomingMatchDetails(0);
        }
        else
        {
            img[0].color = new Color(0.5254f, 0.5254f, 0.5254f, 1);
            verticle[0].childControlWidth = false;
            matchTypes[0].SetActive(false);

        }

    }
    public void OnvalueChangeODI()
    {
        if (togs[1].isOn)
        {

            foreach (var item in GameController.Instance.match.Values)
            {
                foreach (var item1 in item.Values)
                {
                    hotGamesObj[1].SetActive(false);
                    if (item1.Type == 1)
                    {

                        if (item1.HotGame == true)
                        {
                            hotGamesObj[1].SetActive(true);

                        }
                    }
                }
            }
            matchTypes[1].SetActive(true);
            img[1].color = new Color(0.7764707f, 0.1058824f, 0.1372549f, 1);
            SetUpcomingMatchDetails(1);
        }
        else
        {
            img[1].color = new Color(0.5254f, 0.5254f, 0.5254f, 1);
            verticle[1].childControlWidth = false;
            matchTypes[1].SetActive(false);

        }
    }
    public void OnvalueChangeTEST()
    {
        if (togs[2].isOn)
        {
            foreach (var item in GameController.Instance.match.Values)
            {
                foreach (var item1 in item.Values)
                {
                    hotGamesObj[2].SetActive(false);
                    if (item1.Type == 2)
                    {

                        if (item1.HotGame == true)
                        {
                            hotGamesObj[2].SetActive(true);

                        }
                    }
                }
            }
            img[2].color = new Color(0.7764707f, 0.1058824f, 0.1372549f, 1);
            matchTypes[2].SetActive(true);
            SetUpcomingMatchDetails(2);
        }
        else
        {
            img[2].color = new Color(0.5254f, 0.5254f, 0.5254f, 1);
            verticle[2].childControlWidth = false;
            matchTypes[2].SetActive(false);

        }
    }
    public void OnvalueChangeT10()
    {
        if (togs[3].isOn)
        {

            foreach (var item in GameController.Instance.match.Values)
            {
                foreach (var item1 in item.Values)
                {

                    hotGamesObj[3].SetActive(false);
                    if (item1.Type == 3)
                    {

                        if (item1.HotGame == true)
                        {
                            hotGamesObj[3].SetActive(true);
                            break;

                        }
                    }
                }
            }
            img[3].color = new Color(0.7764707f, 0.1058824f, 0.1372549f, 1);
            matchTypes[3].SetActive(true);
            SetUpcomingMatchDetails(3);
        }
        else
        {
            img[3].color = new Color(0.5254f, 0.5254f, 0.5254f, 1);
            verticle[3].childControlWidth = false;
            matchTypes[3].SetActive(false);

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

        foreach (var item in GameController.Instance.match.Values)
        {

            foreach (var item1 in item.Values)
            {
                Debug.Log(item1.HotGame + "********");
                if (togs[toggleindex].isOn)
                {
                    if (item1.Type == toggleindex)
                    {
                        if (item1.HotGame)
                        {
               
                           PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("HotGameHolder");
                            mprefabObj.transform.SetParent(parentHotTable[toggleindex]);
                            mprefabObj.gameObject.SetActive(true);
                            string timeStringVal = item1.Time;
                            mprefabObj.gameObject.GetComponent<TeamHolderData>().SetDetails(item1.TeamA, item1.TeamB, item1.ID, timeStringVal);

                        }
                        else
                        {
                            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("UpcomingGameHolder");
                            mprefabObj.transform.SetParent(parentUpComingMatch[toggleindex]);
                            mprefabObj.gameObject.SetActive(true);
                            string timeString = item1.Time;
                            mprefabObj.gameObject.GetComponent<TeamHolderData>().SetDetails(item1.TeamA, item1.TeamB, item1.ID, timeString);
                        }
                    }
                }
            }
        }

    }
}
