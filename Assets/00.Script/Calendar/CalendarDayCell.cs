using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using System;

public class CalendarDayCell : MonoBehaviour
{

    public TMP_Text dayText;
    public Image background;
    public Image resultIcon;

    private DateTime myDate;

    public void Setup(DateTime date, Action<DateTime> onClick)
    {
        myDate = date;
        dayText.text = date.Day.ToString();

        if (date.Date == DateTime.Today)
            background.color = new Color(1f, 0.9f, 0.6f);

        GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke(date));


    }

    public void SetResult(String result)
    {
        switch (result.ToLower())
        {
            case "win":
                resultIcon.color = Color.blue;
                break;
            case "lose":
                resultIcon.color = Color.red;
                break;
            case "draw":
                resultIcon.color = Color.gray;
                break;
            default:
                resultIcon.enabled = false;
                break;
        }
        resultIcon.enabled=true;
    }
}
