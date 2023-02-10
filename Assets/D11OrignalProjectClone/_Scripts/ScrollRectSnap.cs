using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform panel;
    public Button[] btn;
    public RectTransform centerpos;
    //public ScrollRect scroll;
    private float[] distance;
    private int btnLength;
    private float[] distReposition;
    private bool dragging = false; // it will be true while drag the scroll
    private int btndistance; // it will tell distance between the buttns
    private int minbtnnum; // it tells the number of button , which is smallest distance to centre

    private void Start()
    {

        //scroll.onValueChanged.AddListener(delegate { Onvaluechange()});
         btnLength = btn.Length;
        distance = new float[btn.Length];
        //get distance between buttons
        distReposition= new float[btn.Length];  
        btndistance = (int)Mathf.Abs(btn[1].GetComponent<RectTransform>().anchoredPosition.x - btn[0].GetComponent<RectTransform>().anchoredPosition.x);
        Debug.Log(btndistance);
    }
     public void Update()
    {
        for (int i = 0; i < btn.Length; i++)
        {
            distReposition[i] = centerpos.GetComponent<RectTransform>().position.x - btn[i].GetComponent<RectTransform>().position.x;
            distance[i] = Mathf.Abs(centerpos.transform.position.x - btn[i].transform.position.x);
            if (distReposition[i] > 3000)
            {
                float currentposx = btn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float currentposy = btn[i].GetComponent<RectTransform>().anchoredPosition.y;
                Vector2 newAnchorpos = new Vector2(currentposx + (btnLength * btndistance), currentposy);
                btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchorpos;
            }
            if (distReposition[i] < -3000)
            {
                float currentposx = btn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float currentposy = btn[i].GetComponent<RectTransform>().anchoredPosition.y;
                Vector2 newAnchorpos = new Vector2(currentposx - (btnLength * btndistance), currentposy);
                btn[i].GetComponent<RectTransform>().anchoredPosition = newAnchorpos;
            }
        }
        float minDistane = Mathf.Min(distance); // It will give min distance of the draggging  of buttons
        for (int a = 0; a < btn.Length; a++)
        {
            if (minDistane == distance[a])  // it calculates the value of minimumm distance between the normal position
            {
                minbtnnum = a;
            }
            if (!dragging)  // if dragging is not done
            {
                //LerpTobtn(minbtnnum * -btndistance); 
                LerpTobtn(-btn[minbtnnum].GetComponent<RectTransform>().anchoredPosition.x);
            }
        }
    }
        
     void LerpTobtn(float position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 2.5f); // changeing button position to parent anchored position
        Vector2 newPosition = new Vector2 (newX , panel.anchoredPosition.y ); // declaring the local pos into newposition 
        panel.anchoredPosition = newPosition;   
    }
    public void StartDrag() // this func is used to drag into the event trigerrs in scroll rect
    {
        dragging= true; 
    }

    public void Enddrag()
    {
        dragging = false;
    }

}
