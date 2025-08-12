using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AI;

//직관 기록 저장 
public class RecordInputUI : MonoBehaviour
{
    public TMP_Text txt_selectedDate;
    private DateTime selectedDate;

    public TMP_Dropdown teamDropdown_R;
    public TMP_Dropdown teamDropdown_L;
    public Image hat_R;
    public Image hat_L;
    public TMP_Dropdown stadiumNameDropdown;
    public TMP_InputField scoreRInput;
    public TMP_InputField scoreLInput;
    public TMP_InputField memoInput;
    public RawImage photoPreview;


    private Texture2D currentPhoto;
    private GameRecordManager recordManager;
    private GameRecordList recordList;

    private void Start()
    {
        recordManager = FindObjectOfType<GameRecordManager>();
        recordList = recordManager.LoadRecords();
        selectedDate = DateTime.Today;
        txt_selectedDate.text = selectedDate.ToString("yyyy-MM-dd");

        PopTeamDropDown(teamDropdown_R);
        PopTeamDropDown(teamDropdown_L);
        PopStadiumDropDown(stadiumNameDropdown);
    }

/// <summary>
/// 입력된 값들 저장 하는 버튼 
/// </summary>
    public void OnSaveButtonClicked()
    {
        string imageFileName = "img_" + System.DateTime.Now.Ticks + ".png";
        //bool? isWinBool = GameRecordManager.CalcualteIsWin()
        GameRecord newRecord = new GameRecord
        {
            //date
            date = txt_selectedDate.text,
            teamR = teamDropdown_R.captionText.text,
            teamL = teamDropdown_L.captionText.text,
            scoreR = scoreRInput.text,
            scoreL = scoreLInput.text,
            stadiumName = stadiumNameDropdown.captionText.text,
            //  isWin = is
            memo = memoInput.text,
            imagePath = Path.Combine(Application.persistentDataPath, imageFileName)
        };

        if (currentPhoto != null)
        {
            recordManager.SaveImage(currentPhoto, imageFileName);
        }

        recordList.records.Add(newRecord);
        recordManager.SaveRecords(recordList);

        Debug.Log("저장 완료");
    }

    public void OnDateTextClicked()
    {
        UIManager.Instance.ShowCalendar((pickedDate) =>
        {
            selectedDate = pickedDate;
            txt_selectedDate.text = pickedDate.ToString("yyyy-MM-dd");
        });
    }

    public void SetPhoto(Texture2D photo)
    {
        currentPhoto = photo;
        photoPreview.texture = photo;
    }


/// <summary>
/// 팀 드롭다운 세팅 
/// </summary>
/// <param name="dd"></param>
    public void PopTeamDropDown(TMP_Dropdown dd)
    {
        dd.ClearOptions();

        List<string> teamNames = new List<string>();

        foreach (var team in DataList.Instance.teams)
        {
            teamNames.Add(team.teamName);
        }

        dd.AddOptions(teamNames);
    }

    /// <summary>
    /// 선택된 드롭다운 값 가져오기 
    /// </summary>
    public void GetTeamDropDownValue()
    {   
        

    }

    /// <summary>
    /// 팀 드롭다운에 따라 이미지 세팅 
    /// </summary>
    /// <param name="teamName"></param>
    public void SetTeamImage(Image hat, TeamData team)
    {
        foreach (var t in DataList.Instance.teams)
        {
            if (team == t)
                hat.sprite = t.hatImage;
        }

    }

    public void PopStadiumDropDown(TMP_Dropdown dd)
    {
        dd.ClearOptions();

        List<string> teamNames = new List<string>();

        foreach (var team in DataList.Instance.stadiums)
        {
            teamNames.Add(team.stadiumName);
        }

        dd.AddOptions(teamNames);
    }




}
