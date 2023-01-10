using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;




    public class UIController : MonoBehaviour
    {
        public List<UIHandler> openedPages;
        public UIHandler Loginscreen, RegisterScreen,ContestPanel, MainMenuScreen, WinnerLeaderBoard ,SelectMatchTeam,captainscreen;
        public static UIController Instance;





        // Start is called before the first frame update
        private void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && openedPages.Count > 0 && openedPages.Count != 0)
                openedPages[openedPages.Count - 1].OnBack();
        }


        public void AddToOpenPages(UIHandler handler)
        {
            if (openedPages.Contains(handler))
            {
                openedPages.Remove(handler);
            }
            DebugHelper.Log(handler.name + "Page Opened");
            openedPages.Add(handler);
        }


        public void RemoveFromOpenPages(UIHandler handler)
        {
            DebugHelper.Log(handler.name + "Page Closed");
            if (openedPages.Contains(handler))
                openedPages.Remove(handler);
        }
    }


