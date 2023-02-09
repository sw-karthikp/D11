using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;



namespace D11
{


    public class AlertPopupHandler : UIHandler
    {

        [SerializeField] private Tween Slidetween;

        public RectTransform Popup;
        public override void HideMe()
        {
            UIController.Instance.RemoveFromOpenPages(this);
            if (Slidetween != null)
            {
                Slidetween.Kill();
            }

            Slidetween = Popup.DOScaleY(0, 0.5f).OnComplete(() =>
            {
                Slidetween = null;
                gameObject.SetActive(false);
            });
        }

        public override void OnBack()
        {
            HideMe();
        }

        public override void ShowMe()
        {
            this.gameObject.SetActive(true);
            UIController.Instance.AddToOpenPages(this);
            if (Slidetween != null)
                Slidetween.Kill();

            Slidetween = Popup.DOScaleY(1f, 0.5f).OnComplete(() => Slidetween = null);
        }
    }
}
