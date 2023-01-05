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
    public Transform parentHotTable;
    public Transform parentUpComingMatch;
    public static MainMenu_Handler Instance;
    public VerticalLayoutGroup verticle;


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameController.Instance.SubscribeMatchDetails();
        GameController.Instance.SubscribePlayerDetails();
        GameController.Instance.SubscribeMatchPools();
        GameController.Instance.SubscribePlayers();
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


    public IEnumerator SetUpcomingMatchDetails()
    {
        yield return new WaitForSeconds(0.01f);
        Debug.Log("******************");

        for (int i = 0; i < GameController.Instance.match.Upcoming.Count; i++)
        {
            Debug.Log(GameController.Instance.match.Upcoming[i].HotGame);
            if (GameController.Instance.match.Upcoming[i].HotGame)
            {
                bool canSkip = false;
                foreach (Transform child in parentHotTable)
                {
                    if (child.name.Contains(GameController.Instance.match.Upcoming[i].ID.ToString()))
                    {
                        canSkip = true;
                        break;
                    }


                }
                if (canSkip) continue;
                GameObject mPrefab = Instantiate(matchprefabHotTable, parentHotTable);
                mPrefab.name = GameController.Instance.match.Upcoming[i].ID.ToString();
                mPrefab.GetComponent<TeamHolderData>().SetDetails(GameController.Instance.match.Upcoming[i].TeamA, GameController.Instance.match.Upcoming[i].TeamB, GameController.Instance.match.Upcoming[i].ID);
                yield return new WaitForSeconds(0f);
            }
            else
            {
                bool canSkip = false;
                foreach (Transform child in parentUpComingMatch)
                {
                    if (child.name.Contains(GameController.Instance.match.Upcoming[i].ID.ToString()))
                    {
                        canSkip = true;
                        break;
                    }
                }
                if (canSkip) continue;
                yield return new WaitForSeconds(0f);
                GameObject mPrefab = Instantiate(matchprefabComingMatch, parentUpComingMatch);
                mPrefab.name = GameController.Instance.match.Upcoming[i].ID.ToString();
                mPrefab.GetComponent<TeamHolderData>().SetDetails(GameController.Instance.match.Upcoming[i].TeamA, GameController.Instance.match.Upcoming[i].TeamB, GameController.Instance.match.Upcoming[i].ID);
                yield return new WaitForSeconds(0f);
            }
        }
        verticle.childControlWidth = false;
    }
}
