using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Net;
using static UnityEditor.Progress;

public class MyTeamPlayersPanel : UIHandler
{
    public static MyTeamPlayersPanel Instance;
    public TMP_Text TeamA;
    public TMP_Text TeamB;
    public TMP_Text playerCount;
    public TMP_Text creditsLeft;
    public Transform[] parent;
    public bool isMyMatch;
    private void Awake()
    {
        Instance = this;
    }

    public override void ShowMe()
    {
        UIController.Instance.AddToOpenPages(this);
        this.gameObject.SetActive(true);

    
     

    }

    public override void OnBack()
    {

    }

    public override void HideMe()
    {
        UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    
    }
    public void SetMymatchBool()
    {
        isMyMatch = true;
        SetValues();
    }


    public void OnEnable()
    {
        foreach (Transform child in parent[0])
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in parent[1])
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in parent[2])
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in parent[3])
        {
            child.gameObject.SetActive(false);
        }

  
      
  

    }

    public void SetValues()
    {
        if (isMyMatch)
        {
            SetMySelectedPlayers();
        }
    }

    public void SetMySelectedPlayers()
    {

        TeamA.text = GameController.Instance.CurrentTeamA;
        TeamB.text = GameController.Instance.CurrentTeamB;
        //playerCount.text = MatchSelection.Instance.selectedplayerCount.text;
        //creditsLeft.text = MatchSelection.Instance.CreditsLeft.text;


        for (int i = 0; i < MatchSelection.Instance.playersForTeam.Count; i++)
        {


            if(MatchSelection.Instance.playersForTeam[i].type == 0)
            {
                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
                mprefabObj.transform.SetParent(parent[0]);
                mprefabObj.gameObject.SetActive(true);
                mprefabObj.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain, MatchSelection.Instance.playersForTeam[i].playerPic);

            }
            if (MatchSelection.Instance.playersForTeam[i].type == 1)
            {
                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
                mprefabObj.transform.SetParent(parent[1]);
                mprefabObj.gameObject.SetActive(true);
                mprefabObj.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain, MatchSelection.Instance.playersForTeam[i].playerPic);

            }
            if (MatchSelection.Instance.playersForTeam[i].type == 2)
            {
                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
                mprefabObj.transform.SetParent(parent[2]);
                mprefabObj.gameObject.SetActive(true);
                mprefabObj.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain, MatchSelection.Instance.playersForTeam[i].playerPic);

            }
            if (MatchSelection.Instance.playersForTeam[i].type == 3)
            {
                PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
                mprefabObj.transform.SetParent(parent[3]);
                mprefabObj.gameObject.SetActive(true);
                mprefabObj.name = MatchSelection.Instance.playersForTeam[i].playerName;
                mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(MatchSelection.Instance.playersForTeam[i].playerName, MatchSelection.Instance.playersForTeam[i].isCaptain, MatchSelection.Instance.playersForTeam[i].isViceCaptain, MatchSelection.Instance.playersForTeam[i].playerPic);

            }
           
        }
     
    }


    public void SetMySelectedPlayerList(List<string> _myteams ,string captain ,string viceCaptain)
    {
        Debug.Log("Called ************");


        foreach (var item in GameController.Instance.players)
        {
            Debug.Log("Called ************");
            foreach (var item1 in item.Players.Values)
            {
                Debug.Log("Called ************");
                foreach(string myPlayers in _myteams)
                {
                    if (myPlayers == item1.ID)
                    {
                        Debug.Log("Called ************");

                        bool isCap;
                        bool isViceCap;
                        isCap = myPlayers.ToString() == captain ? true : false;
                        isViceCap = myPlayers.ToString() == captain ? true : false;
                        SetPlayerDetailsValues(item1.Type, item1.Name, isCap, isViceCap, null);
                    }
    
                    //if (myPlayers == item1.ID)
                    //{
                       
                    //    //foreach (var sprite in GameController.Instance.playerSpriteImage)
                    //    //{
                          
                    //    //    Debug.Log("Called ************");
                    //    //    if (myPlayers.ToString() == sprite.Key)
                    //    //    {
                    //    //        SetPlayerDetailsValues(item1.Type, item1.Name, isCap, isViceCap, sprite.Value);
                    //    //        Debug.Log("Called ************ %%5");
                    //    //    }
                    //    //}
                    //}    
                }
            }

        }
    }


    public void SetPlayerDetailsValues(int _type ,string _playerName ,bool _isCap, bool _isViceCap ,Sprite _pic)
    {

        if (_type == 0)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
            mprefabObj.transform.SetParent(parent[0]);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(_playerName, _isCap, _isViceCap, _pic);
            Debug.Log("Called ************ $$$ **" + _type);

        }
        if (_type == 1)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
            mprefabObj.transform.SetParent(parent[1]);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(_playerName, _isCap, _isViceCap, _pic);
            Debug.Log("Called ************ $$$ **" + _type);

        }

        if (_type == 2)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
            mprefabObj.transform.SetParent(parent[2]);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(_playerName, _isCap, _isViceCap, _pic);

            Debug.Log("Called ************ $$$ **" + _type);
        }
        if (_type == 3)
        {
            PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("Player");
            mprefabObj.transform.SetParent(parent[3]);
            mprefabObj.gameObject.SetActive(true);
            mprefabObj.GetComponent<PlayerDisplay>().SetPlayerDetails(_playerName, _isCap, _isViceCap, _pic);
            Debug.Log("Called ************ $$$ **" + _type);
        }
      
    }
}
