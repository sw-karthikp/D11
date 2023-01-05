using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BannerLoop : MonoBehaviour
{
    public GameObject Bannars;
    public GameObject _parent;
    public ScrollRect _scroll;
    // Start is called before the first frame update

    private void Awake()
    {
        _scroll.onValueChanged.AddListener(delegate { ScrollLoop(); });
    }

    private void OnEnable()
    {

            Instantiate(Bannars, _parent.transform);
        
 
    }

    public void ScrollLoop()
    {
        if(_scroll.horizontalScrollbar.value  == 1  )
        {
          GameObject obj=   _parent.GetComponentInChildren<Transform>().transform.GetChild(0).gameObject;
            obj.transform.SetSiblingIndex(_parent.transform.childCount - 1);
            _scroll.horizontalNormalizedPosition = 0;
        }
        
        //if(_scroll.horizontalScrollbar.value > 0.7f)
        //{
        //    GameObject obj = _parent.GetComponentInChildren<Transform>().transform.GetChild(_parent.transform.childCount - 1).gameObject;
        //    obj.transform.SetSiblingIndex(0);
        //}
    }
}
