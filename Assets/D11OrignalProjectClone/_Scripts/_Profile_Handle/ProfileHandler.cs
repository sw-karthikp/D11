using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace D11 {
    public class ProfileHandler : MonoBehaviour
    {
       [SerializeField] private TMP_Text dob;
        [SerializeField]private Button Dob;



        public void OnDOBBtnClicked()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
#elif UNITY_EDITOR
            date = "24/09/1998";

#endif
        }
        string date, dateTemp;
        private void OnSelectedAction(string value)
        {
            //LoggerUtils.Log(date + "___________");
            date = CommonFunctions.Instance.ChangeDateFormat(value, new string[4] { "MM/d/yyyy", "MM/dd/yyyy", "M/d/yyyy", "M/dd/yyyy" });
            //LoggerUtils.Log(date + "___________");
            MobileNativeManager.OnSelectedAction -= OnSelectedAction;
        }
        IEnumerator UpdateDate()
        {
            while (true)
            {
                if (date != dateTemp)
                {
                    int totaldaysNeeded = 6570;
                    LoggerUtils.Log(((System.DateTime.Now - System.DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).TotalDays > totaldaysNeeded) + "_____________________");
                    if ((System.DateTime.Now - System.DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).TotalDays > totaldaysNeeded)
                    {
                        dateTemp = date;
                        dob.text = date;

                        Dob.interactable = false;
                    }
                    else
                    {
                        date = dateTemp;

                        StopCoroutine("UpdateDate");
                        Dob.interactable = true;
                    }
                    LoggerUtils.Log("data : " + date);
                    yield return new WaitForSeconds(1);
                }
            }


        }
    }
}
