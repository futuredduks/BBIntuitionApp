using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AI;
using System.Security.Cryptography;
using Unity.Collections.LowLevel.Unsafe;

//직관 기록 저장 
public class RecordInputUI : MonoBehaviour
{
    public static RecordInputUI Instance { get; private set; }
    public TMP_Text txt_selectedDate;
    private DateTime selectedDate;

    public int myTeam;
    public int enemyTeam;

    public TMP_Dropdown teamDropdown_R;
    public TMP_Dropdown teamDropdown_L;

    public Toggle isHome;
    public Toggle isAway;
    public Toggle isneither;
    public Image hat_R;
    public Image hat_L;
    public TMP_Dropdown stadiumNameDropdown;
    public TMP_InputField scoreRInput;
    public TMP_InputField scoreLInput;
    public TMP_InputField memoInput;
    public RawImage photoPreview;


    private Texture2D currentPhoto;
    private GameRecordManager recordManager;
    [SerializeField]
    private GameRecordList recordList;
    [SerializeField]
    private List<string> teamNames = new List<string>();
    private List<string> stadiumNames = new List<string>();
    string myTeamName = "삼성라이온즈"; //테스트용 데이터 값 
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        recordManager = FindObjectOfType<GameRecordManager>();
        recordList = recordManager.LoadRecords();
        selectedDate = DateTime.Today;
        txt_selectedDate.text = selectedDate.ToString("yyyy-MM-dd");

        PopTeamDropDown(teamDropdown_R);
        PopTeamDropDown(teamDropdown_L);
        PopStadiumDropDown(stadiumNameDropdown);

        teamDropdown_R.onValueChanged.AddListener(delegate { OnDropdownChanged(teamDropdown_R, teamDropdown_L); });
        teamDropdown_L.onValueChanged.AddListener(delegate { OnDropdownChanged(teamDropdown_L, teamDropdown_R); });

        isHome.onValueChanged.AddListener(delegate { GetSelectedLocation(); });
        isAway.onValueChanged.AddListener(delegate { GetSelectedLocation(); });
        isneither.onValueChanged.AddListener(delegate { GetSelectedLocation(); });
    }

    public void InitRecordPanel()
    {
        selectedDate = DateTime.Today;
        txt_selectedDate.text = selectedDate.ToString("yyyy-MM-dd");

        teamDropdown_L.value = 0;
        teamDropdown_R.value = 0;

        UpdateDropDownColor(teamDropdown_L);
        UpdateDropDownColor(teamDropdown_R);

        scoreLInput.text = string.Empty;
        scoreRInput.text = string.Empty;

        isHome.isOn = true;

        stadiumNameDropdown.value = 0;
        
    }

    /// <summary>
    /// 입력된 값들 저장 하는 버튼 
    /// </summary>
    public void OnSaveButtonClicked()
    {
        if (IsCheckInfo())
        {
            
            string date = txt_selectedDate.text;
            string teamR = teamDropdown_R.captionText.text;
            string teamL = teamDropdown_L.captionText.text;
            string stadiumName = stadiumNameDropdown.captionText.text;

            int scoreR, scoreL = 0;
            int.TryParse(scoreRInput.text, out scoreR);
            int.TryParse(scoreLInput.text, out scoreL);

            string imageFileName = "img_" + System.DateTime.Now.Ticks + ".png";
            // bool? isWinBool = GameRecordManager.CalcualteIsWin()
            GameRecord newRecord = new GameRecord
            {
                //date
                date = date,
                teamR = teamR,
                teamL = teamL,
                scoreL = scoreL.ToString(),
                scoreR = scoreR.ToString(),
                stadiumName = stadiumName,
                result = GameRecordManager.CalcualteIsWin(myTeamName, teamR, teamL, scoreR, scoreL),
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
            UIManager.Instance.HideAddRecordPanel();
        }
        else
        {
            Debug.Log("저장실패");
        }

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

        if (teamNames.Count <= 0)
        {
            teamNames.Add("선택하세요.");
            foreach (var team in DataList.Instance.teams)
            {
                teamNames.Add(team.teamName);
            }
        }

        dd.value = 0;
        SetTeamImage(dd);
        dd.AddOptions(teamNames);
    }

    public bool IsMyTeam() {
        string teamR_Name = DataList.Instance.teams[teamDropdown_R.value-1].teamName;
        string teamL_Name = DataList.Instance.teams[teamDropdown_L.value-1].teamName;

        

        if ((myTeamName != teamR_Name) && (myTeamName != teamL_Name))
        {
            Debug.Log("myteam null");
            myTeam = 0;
            enemyTeam = 0;
            stadiumNameDropdown.value = 0;
            isneither.isOn = true;
            return false;
        }
        else
        {
            if (myTeamName == teamL_Name)
            {
                Debug.Log("내 홈팀 포함" + teamDropdown_L.value);
                myTeam = teamDropdown_L.value;
                enemyTeam = teamDropdown_R.value;
               

            }
            else if (myTeamName == teamR_Name)
            {
                Debug.Log("내 홈팀 포함" + teamDropdown_R.value);
                myTeam = teamDropdown_R.value;
                enemyTeam = teamDropdown_L.value;
                
            }

            GetSelectedLocation();
            stadiumNameDropdown.RefreshShownValue();
        }
        return false;
    }

    public void GetSelectedLocation()
    {
        int num = 0;
        if (isHome.isOn)
        {
            num = myTeam;
        }
        else if (isAway.isOn)
        {
            num = enemyTeam;
        }
        else
            num = 0;

        
        SearchStadium(num);
        stadiumNameDropdown.RefreshShownValue(); 
    }

    public void SearchStadium(int num)
    {
        if (num == 0)
            stadiumNameDropdown.value = 0;
        else
        {
            for (int i = 0; i < stadiumNames.Count; i++)
            {
                 Debug.Log("찾아보자" + stadiumNames[i] + "//" + DataList.Instance.teams[num-1].homeStadium.stadiumNameE);
                if (stadiumNames[i] == DataList.Instance.teams[num - 1].homeStadium.stadiumNameE)
                {
                    stadiumNameDropdown.value = i;
                    break;
                }
            }
        }
       
        stadiumNameDropdown.RefreshShownValue(); 
    }



    /// <summary>
    /// 팀 드롭다운에 따라 이미지 세팅 
    /// </summary>
    /// <param name="teamName"></param>
    public void SetTeamImage(TMP_Dropdown dd)
    {
        var hat = dd == teamDropdown_R ? hat_R : hat_L;
        int num = dd.value;

        Color c = hat.color;
        if (num == 0)
            c.a = 0;//불투명
        else
        {
            c.a = 1;
            hat.sprite = DataList.Instance.teams[num - 1].hatImage;
        }

        hat.color = c;
    }

    public void SetStadiumDropDown()
    {
        
    }
    public void PopStadiumDropDown(TMP_Dropdown dd)
    {
        dd.ClearOptions();

        List<string> stadiumName = new List<string>();
        stadiumName.Add("구장을 선택하세요.");
        stadiumNames.Add("Null");
        foreach (var team in DataList.Instance.stadiums)
        {
            stadiumName.Add(team.stadiumName);
            stadiumNames.Add(team.stadiumNameE);
        }

        dd.AddOptions(stadiumName);
    }

    /// <summary>
    /// 팀선택 안 할 경우 
    /// 스코어 입력안 할 경우
    /// false: 저장 안됨 
    /// </summary>
    public bool IsCheckInfo()
    {
        if (teamDropdown_L.value == 0 || teamDropdown_R.value == 0)
        {
            Debug.Log("팀을 입력하지 않았음");
            return false;
        }

        string scoreR = scoreRInput.text;
        string scoreL = scoreLInput.text;
        if (string.IsNullOrEmpty(scoreR) || string.IsNullOrEmpty(scoreL))
        {
            Debug.Log("점수 값을 입력하지 않았음 저장안됨");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 드롭다운 R L 이 같은 값 선택하면 한쪽을 다시 선택하게 변경 함.
    /// </summary>
    /// <param name="change"></param>
    /// <param name="other"></param>
    public void OnDropdownChanged(TMP_Dropdown change, TMP_Dropdown other)
    {
        UpdateDropDownColor(other, change.value);

        SetTeamImage(change);

        if (change.value != 0)
        {
            if (change.value == other.value)
            {
                other.value = 0;
            }
        }

        if (change.value != 0 && other.value != 0)
        {
            IsMyTeam();
        }
    }

    /// <summary>
    /// 선택하지않은 쪽 드롭다운 색상 변경 (동일 값 회색으로)
    /// </summary>
    /// <param name="dd">드롭다운</param>
    /// <param name="num">선택된 index</param>
    public void UpdateDropDownColor(TMP_Dropdown dd, int num)
    {
        for (int i = 0; i < dd.options.Count; i++)
        {
            if (i == num && i != 0)
            {
                dd.options[i].text = $"<color=grey>{teamNames[i]}</color>";
            }
            else
                dd.options[i].text = teamNames[i];
        }
    }

    public void UpdateDropDownColor(TMP_Dropdown dd)
    {
        for (int i = 0; i < dd.options.Count; i++)
        {
            dd.options[i].text = teamNames[i];
        }

        SetTeamImage(dd);
    }





}
