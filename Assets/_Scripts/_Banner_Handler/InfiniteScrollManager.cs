using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollManager : MonoBehaviour
{
    public List<BannerData> BannerButtons;
    public Transform CenterPoint;
    public GameObject BannerPrefab;
    public List<Sprite> BannerSprites;
    public float BannerGap = 50f;

    public GameObject BannerDot;
    public Transform DotParent;
    public Sprite ActiveDot;
    public Sprite PassiveDot;

    private Vector2 PreviousMousePos = Vector2.zero;
    private Vector2 CurrentMousePos = Vector2.zero;

    private void OnEnable()
    {
        for (int i = 0; i < /*BannerSprites.Count*/ 3; i++)
        {
            GameObject banner = Instantiate(BannerPrefab, transform);
            RectTransform bannerTransform = banner.GetComponent<RectTransform>();
            bannerTransform.GetComponent<Image>().sprite = BannerSprites[0];
            if (BannerButtons.Count > 0)
            {
                bannerTransform.position = new Vector2(GetBannerPos(i - 1), bannerTransform.position.y);
            }
            else
            {
                bannerTransform.localPosition = Vector2.zero;
            }

            bannerTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            //bannerTransform.GetComponent<Button>().onClick.AddListener(/*AssignYourWebRequestFunctionHere*/);

            GameObject dot = Instantiate(BannerDot, DotParent);
            Image dotImage = dot.GetComponent<Image>();
            dotImage.sprite = i == 0 ? ActiveDot : PassiveDot;
            BannerData data = new BannerData();
            data.BannerTransform = bannerTransform;
            data.BannerDot = dotImage;
            BannerButtons.Add(data);
        }
    }

    private void OnDisable()
    {
        foreach(Transform banner in transform)
        {
            Destroy(banner.gameObject);
        }
    }

    private void OnMouseDrag()
    {
        CurrentMousePos = Input.mousePosition;
        float xMovement = CurrentMousePos.x - PreviousMousePos.x;
        transform.localPosition = new Vector2(transform.localPosition.x + xMovement, transform.localPosition.y);
    }

    private void BannerPosition()
    {
        foreach (BannerData banner in BannerButtons)
        {

        }
    }

    private float GetBannerPos(int index)
    {
        return (BannerButtons[index].BannerTransform.position.x + (BannerButtons[index].BannerTransform.sizeDelta.x / 2) + BannerGap);
    }
}

[System.Serializable]
public class BannerData
{
    public RectTransform BannerTransform;
    public Image BannerDot;
}