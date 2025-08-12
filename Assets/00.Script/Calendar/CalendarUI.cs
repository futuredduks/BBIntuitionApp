using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CalendarUI : MonoBehaviour
{
    public TMP_Text monthText;
    public Button prevMonthBtn;
    public Button nextMonthBtn;
    public Transform dayGridParent;
    public GameObject dayCellPrefab;

    public Action<DateTime> onDateSelected;

    private int year, month;
    private Dictionary<string, CalendarDayCell> dayCells = new Dictionary<string, CalendarDayCell>();

    public void Init(DateTime? initialDate = null)
    {
        DateTime date = initialDate ?? DateTime.Today;
        year = date.Year;
        month = date.Month;


        monthText.text = date.ToString("yyyy-MM");
        DrawCalendar();

        prevMonthBtn.onClick.AddListener(() => { ChangeMonth(-1); });
        nextMonthBtn.onClick.AddListener(() => { ChangeMonth(1); });
    }

    void ChangeMonth(int delta) {
        month += delta;
        if (month > 12) { month = 1; year++; }
        if (month < 1) { month = 12; year--; }
        DrawCalendar();
    }


    void DrawCalendar()
    {
        foreach (Transform child in dayGridParent)
            Destroy(child.gameObject);

        dayCells.Clear();

        DateTime firstDay = new DateTime(year, month, 1);
        int startDay = (int)firstDay.DayOfWeek;
        int totalDays = DateTime.DaysInMonth(year, month);

        monthText.text = $"{year}년 {month}월";

        for (int i = 0; i < startDay; i++)
            Instantiate(dayCellPrefab, dayGridParent);

        for (int d = 1; d <= totalDays; d++)
        {
            DateTime date = new DateTime(year, month, d);
            GameObject cellObj = Instantiate(dayCellPrefab, dayGridParent);
            CalendarDayCell cell = cellObj.GetComponent<CalendarDayCell>();
            cell.Setup(date, onDateSelected);
            dayCells[date.ToString("yyyy-MM-dd")] = cell;
        }
        
    }

    public void ApplyResults(List<CalendarResult> results)
    {
        foreach (var r in results)
        {
            if (dayCells.TryGetValue(r.date, out var cell))
                cell.SetResult(r.result);
        }
    }
    
}
