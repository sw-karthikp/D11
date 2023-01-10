using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using Unity.VisualScripting;

namespace D11 
{
    public class CommonFunctions : SingletonMonoBehaviour<CommonFunctions>
    {

    public string ChangeDateFormat(string inputDate, string[] specificFormats)
        {
            string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                        "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "MM/dd/yyyy","yyyy/MM/dd","dd/MM/yyyy","MM-dd-yyyy","yyyyMMdd"};
            DateTime.TryParseExact(inputDate, specificFormats == null ? formats : specificFormats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out DateTime outputDate);
            return outputDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
