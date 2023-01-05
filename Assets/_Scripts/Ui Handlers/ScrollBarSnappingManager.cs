using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScrollBarSnappingManager : MonoBehaviour
{
    public Scrollbar MyScrollbar;
    private Tween ScrollbarTween;
    public ScrollPanelData welcomePanel;
    public ScrollPanelData selectMatchPanel;
    //public RectTransform TitleText;
    public ScrollPanelData joinContestsPanel;
    public ScrollPanelData createTeamsPanel;

    void OnEnable()
    {
        ResetScrollBar();
    }
    public void ScrollEffect()
    {
        PanelEffects(welcomePanel, 0);
        PanelEffects(selectMatchPanel, 0.5f);
        PanelEffects(joinContestsPanel, 0.75f);
        PanelEffects(createTeamsPanel, 1f);
    }
    private void PanelEffects(ScrollPanelData panel,float pos)
    {
        float dis = Mathf.Abs(MyScrollbar.value - pos);
        if (dis <= 0.5f)
        {
            panel.Panel.localScale = Vector2.Lerp(Vector2.one, Vector2.one * 1f, Mathf.InverseLerp(0, 0.5f, dis));
            //float colorAlpha = Mathf.Lerp(1, 1f, Mathf.InverseLerp(0, 0.25f, dis));
            //panel.BG.color = new Color(panel.BG.color.r, panel.BG.color.g, panel.BG.color.b, colorAlpha);
            //panel.TitleTxt.color = new Color(panel.TitleTxt.color.r, panel.TitleTxt.color.g, panel.TitleTxt.color.b, colorAlpha);
            //panel.InfoTxt.color = new Color(panel.InfoTxt.color.r, panel.InfoTxt.color.g, panel.InfoTxt.color.b, colorAlpha);
        }
        else
        {
            panel.Panel.localScale=Vector2.one*0.85f;
            //panel.BG.color = new Color(panel.BG.color.r, panel.BG.color.g, panel.BG.color.b, 0.75f);
            //panel.TitleTxt.color = new Color(panel.TitleTxt.color.r, panel.TitleTxt.color.g, panel.TitleTxt.color.b, 0.75f);
            //panel.InfoTxt.color = new Color(panel.InfoTxt.color.r, panel.InfoTxt.color.g, panel.InfoTxt.color.b, 0.75f);
        }
    }
    public async void SnapScrollbar()
    {
        await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(0.25f));
        float snapvalue = 0;
        if (MyScrollbar.value <= 0.25f)
        {
            snapvalue = 0;
        }
        else if (MyScrollbar.value > 0.25f && MyScrollbar.value <= 0.5f)
        {
            snapvalue = 0.3338f;
            //TitleText.DOScaleZ(1,1f);
        }
        else if (MyScrollbar.value > 0.5f && MyScrollbar.value <= 0.75f)
        {
            snapvalue = 0.6666f;
        }
        else
        {
            snapvalue = 1;
        }
        if (ScrollbarTween != null)
        {
            ScrollbarTween.Kill();
        }
        ScrollbarTween = DOTween.To(() => MyScrollbar.value, x => MyScrollbar.value = x, snapvalue, 0.25f).OnUpdate(() =>
        {
            if (Input.GetMouseButton(0))
            {
                ScrollbarTween.Kill();
                return;
            }
        }).OnComplete(() => { ScrollbarTween.Kill(); ScrollbarTween = null; });
    }
    public void ResetScrollBar()
    {
        DOTween.To(() => MyScrollbar.value, x => MyScrollbar.value = x, -5, 0.1f);
        Invoke(nameof(SnapScrollbar), 0.25f);
    }
}
[System.Serializable]
public class ScrollPanelData
{
    public RectTransform Panel;
    public Image BG;
    public Image HighlightImg;
    public TMP_Text TitleTxt;
    public TMP_Text InfoTxt;
}
