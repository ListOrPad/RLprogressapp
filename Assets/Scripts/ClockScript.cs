using UnityEngine;
using UnityEngine.UI;
using System;

public class ClockScript : MonoBehaviour
{
    public Sprite[] sprites; //array of images for the digit sprites 

    void Update()
    {
        // Get the current system time
        DateTime now = DateTime.Now;

        // Update the second images
        int second = now.Second;
        int secondTens = second / 10;
        int secondOnes = second % 10;

        int minute = now.Minute;
        int minuteTens = minute / 10;
        int minuteOnes = minute % 10;

        int hour = now.Hour;
        int hourTens = hour / 10;
        int hourOnes = hour % 10;

        int day = now.Day;
        int dayTens = day / 10;
        int dayOnes = day % 10;

        int month = now.Month;
        int monthTens = month / 10;
        int monthOnes = month % 10;

        int year = now.Year;
        int yearThousands = year / 1000;
        int yearHundreds = (year % 1000) / 100;
        int yearTens = (year % 100) / 10;
        int yearOnes = year % 10;

        GameObject secondDivergence1 = GameObject.Find("Second Divergence 1");
        if (secondDivergence1 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = secondDivergence1.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(secondTens);
        }
        GameObject secondDivergence2 = GameObject.Find("Second Divergence 2");
        if (secondDivergence2 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = secondDivergence2.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(secondOnes);
        }

        GameObject minuteDivergence1 = GameObject.Find("Minute Divergence 1");
        if (minuteDivergence1 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = minuteDivergence1.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(minuteTens);
        }
        GameObject minuteDivergence2 = GameObject.Find("Minute Divergence 2");
        if (minuteDivergence2 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = minuteDivergence2.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(minuteOnes);
        }

        GameObject hourDivergence1 = GameObject.Find("Hour Divergence 1");
        if (hourDivergence1 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = hourDivergence1.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(hourTens);
        }
        GameObject hourDivergence2 = GameObject.Find("Hour Divergence 2");
        if (hourDivergence2 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = hourDivergence2.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(hourOnes);
        }

        // Update the date images

        GameObject dayDivergence1 = GameObject.Find("Day Divergence 1");
        if (dayDivergence1 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = dayDivergence1.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(dayTens);
        }
        GameObject dayDivergence2 = GameObject.Find("Day Divergence 2");
        if (dayDivergence2 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = dayDivergence2.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(dayOnes);
        }

        GameObject monthDivergence1 = GameObject.Find("Month Divergence 1");
        if (monthDivergence1 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = monthDivergence1.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(monthTens);
        }
        GameObject monthDivergence2 = GameObject.Find("Month Divergence 2");
        if (monthDivergence2 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = monthDivergence2.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(monthOnes);
        }

        GameObject yearDivergence1 = GameObject.Find("Year Divergence 1");
        if (yearDivergence1 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = yearDivergence1.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(yearThousands);
        }
        GameObject yearDivergence2 = GameObject.Find("Year Divergence 2");
        if (yearDivergence2 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = yearDivergence2.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(yearHundreds);
        }
        GameObject yearDivergence3 = GameObject.Find("Year Divergence 3");
        if (yearDivergence3 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = yearDivergence3.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(yearTens);
        }
        GameObject yearDivergence4 = GameObject.Find("Year Divergence 4");
        if (yearDivergence4 != null)
        {
            // Get the Image component and set the sprite
            Image digitImg = yearDivergence4.GetComponent<Image>();
            digitImg.sprite = GetDigitSprite(yearOnes);
        }
    }
    Sprite GetDigitSprite(int digit)
    {
        switch (digit)
        {
            case 0:
                return sprites[0];
            case 1:
                return sprites[1];
            case 2:
                return sprites[2];
            case 3:
                return sprites[3];
            case 4:
                return sprites[4];
            case 5:
                return sprites[5];
            case 6:
                return sprites[6];
            case 7:
                return sprites[7];
            case 8:
                return sprites[8];
            case 9:
                return sprites[9];
        }
        return null;
    }
}