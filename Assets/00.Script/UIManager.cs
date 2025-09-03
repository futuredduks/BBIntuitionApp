using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public CalendarUI calendarUI;
    public GameObject calendarPanel;
    public GameObject addRecordPanel;
    public GameObject setProfilePanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowCalendar(Action<DateTime> onDateSelected)
    {
        calendarPanel.SetActive(true);
        calendarUI.Init();
        calendarUI.onDateSelected = (date) =>
        {
            onDateSelected?.Invoke(date);
            calendarPanel.SetActive(false); // 선택 후 자동 닫기
        };
    }

    public void ShowCalendarWithResults(List<CalendarResult> results)
    {
        calendarPanel.SetActive(true);
        calendarUI.Init();
        calendarUI.ApplyResults(results);
    }

    public void HideCalendar()
    {
        calendarPanel.SetActive(false);
    }

    public void ShowAddRecordPanel()
    {
        if (!addRecordPanel.activeSelf)
            RecordInputUI.Instance.InitRecordPanel();
        addRecordPanel.SetActive(true);

    }

    public void HideAddRecordPanel()
    {
        if (addRecordPanel.activeSelf)
            addRecordPanel.SetActive(false);
    }

    public void ShowProfilePanel()
    {
        setProfilePanel!?.SetActive(true);
    }

}
