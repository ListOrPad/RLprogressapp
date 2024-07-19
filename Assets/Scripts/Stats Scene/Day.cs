using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Day : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    
    private DateTime day;
    

    private void Start()
    {
        // Set the date label
        dayText.text = day.ToString("dd MMM yy", CultureInfo.InvariantCulture); // Format the DateTime as "day month year"
    }

    public void SetDay(DateTime day)
    {
        this.day = day;
    }

    public DateTime GetDay()
    {
        return day;
    }
}
