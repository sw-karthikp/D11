using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace D11
{

    public class BottomHandler : UIHandler
    {
        
        public List<BottomToggleDatas> bottomtoggles;
        public int currentIndex = 0;
        public int previousIndex = -1;

        private void Awake()
        {
            Input.multiTouchEnabled = false;
            for(int i=0; i<bottomtoggles.Count; i++)
            {
                int index = i;
                bottomtoggles[i].Btmtoggle.onValueChanged.RemoveAllListeners();
                bottomtoggles[i].Btmtoggle.onValueChanged.AddListener(toggleState =>
                {
                    if (toggleState)
                    {
                        ChangePanel(index);
                    }
                });
            }
        }

        private void OnEnable()
        {
            ChangePanel(0);
        }
        public void ChangeToggle(int index)
        {
            for(int i =0; i< bottomtoggles.Count; i++)
            {
                bottomtoggles[i].Btmtoggle.isOn = i == index; 
            }
        }
        public void ChangePanel(int index)
        {
            if(index != previousIndex && bottomtoggles[index].Btmtoggle.isOn)
            {
                if(index > 0)
                {
                    ShowMe();
                }
                else
                {
                    HideMe();
                }
                if(previousIndex > 0)
                {
                    bottomtoggles[previousIndex].TogglePanel.HideMe();
                }
                bottomtoggles[index].TogglePanel.ShowMe();
                //for(int i=0; i<bottomtoggles.Count; i++)
                //{
                //    bottomtoggles[i].ToggleTxt.SetActive(i == index);
                //}
                currentIndex = index;
                previousIndex = index;
            }
        }
        public override void ShowMe()
        {
            UIController.Instance.AddToOpenPages(this);
        }
        public override void HideMe()
        {
            UIController.Instance.RemoveFromOpenPages(this);
            if(currentIndex > 0)
            {
                bottomtoggles[currentIndex].TogglePanel.HideMe();
                currentIndex = 0;
            }
        }
        public override void OnBack()
        {
            HideMe();
            for(int i =0; i< bottomtoggles.Count; i++)
            {
                bottomtoggles[i].Btmtoggle.isOn = i == 0;
                //bottomtoggles[i].ToggleTxt.SetActive(i == 0);
            }
        }

    }
}
[System.Serializable]
public  class BottomToggleDatas
{
    public Toggle Btmtoggle;
    public UIHandler TogglePanel;
    //public GameObject ToggleTxt;
}
