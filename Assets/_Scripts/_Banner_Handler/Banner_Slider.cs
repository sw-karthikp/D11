using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace D11 {
    public class Banner_Slider : MonoBehaviour
    {
        public List<Banner> Bannerslider;
        public bool random;
        public bool Elastic = true;

        public Transform Bannerposition;
        public Button Bannerbtn;
        public Transform DotPosition;
        public Toggle Dots;
        public Horizontal_Slider horizontal_Slider;
        

        public void OnValidate()
        {
        //
        //GetComponent<ScrollRect>().content.GetComponent<GridLayoutGroup>().cellSize = GetComponent<RectTransform>().sizeDelta;
        }

        public IEnumerator Start()
        {
            foreach (Transform child in Bannerposition)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in DotPosition)
            {
                Destroy(child.gameObject);
            }
            foreach (var banner in Bannerslider)
            {
                var instance = Instantiate(Bannerbtn, Bannerposition);
                var button = instance.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                //if(string.IsNullOrEmpty(banner.url))
                instance.GetComponent<Image>().sprite = banner.Bannerimage;
                if (Bannerslider.Count > 1)
                {
                    var dotstoggle = Instantiate(Dots, DotPosition);

                    dotstoggle.group = DotPosition.GetComponent<ToggleGroup>();
                }
             
                
            }
            yield return null;
           
            horizontal_Slider.Begin(random);
            horizontal_Slider.GetComponent<ScrollRect>().movementType = Elastic ? ScrollRect.MovementType.Elastic :ScrollRect.MovementType.Clamped;
            //StartCoroutine(nameof(SlidePanel));

        }


        //public IEnumerator SlidePanel()

        //{

            
        //    Bannerposition.localPosition = new Vector3(0, 0, 0);
        //    yield return new WaitForSeconds(5f);
        //    Bannerposition.localPosition = new Vector3(-933, 0, 0);
        //    yield return new WaitForSeconds(5f);
        //    Bannerposition.localPosition = new Vector3(-1794, 0, 0);
        //    yield return new WaitForSeconds(5f);
        //    Bannerposition.localPosition = new Vector3(0, 0, 0);
        //}
    }


}
