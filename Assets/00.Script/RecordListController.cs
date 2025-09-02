using System.Collections;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordListController : MonoBehaviour
{
    //메인화면의 직관 기록 결과 리스트 관리 
    public ScrollRect scoreScroll;
    public Transform scoreContent;
    public GameObject scoreRecordPrefab;

    List<GameObject> contentsList = new List<GameObject>();

    public TMP_Text allGameCount;
    public TMP_Text outCome;

    int allCount = 0;
    int winCount = 0;
    int loseCount = 0;
    int drawCount = 0;

    float winRate = 0;
    float winRateD = 0;//무승부 포함 


    // Start is called before the first frame update

    public void CreateList()
    {
        contentsList?.Clear();

        List<GameRecord> list = RecordInputUI.Instance.GameRecordList.records;
        Debug.Log("list Count" + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GameObject recordPrefab = Instantiate(scoreRecordPrefab, scoreContent);
            contentsList.Add(recordPrefab);
            if (recordPrefab.TryGetComponent<RecordPrefab>(out RecordPrefab recordSc))
            {
                recordSc.SetRecordData(list[i]);
            }
        }

        RecordCalculation();
        CalculateWinRate();
        InputText();
    }

    void RecordCalculation()
    {
        List<GameRecord> record = RecordInputUI.Instance.GameRecordList.records;
        allCount = 0;
        winCount = 0;
        loseCount = 0;
        drawCount = 0;

        for (int i = 0; i < record.Count; i++)
        {
            switch (record[i].result)
            {
                case GameRecordManager.GameResult.Win:
                    winCount++;
                    allCount++;
                    break;
                case GameRecordManager.GameResult.Lose:
                    loseCount++;
                    allCount++;
                    break;
                case GameRecordManager.GameResult.Draw:
                    drawCount++;
                    allCount++;
                    break;
            }
        }
    }

    void InputText()
    {
        allGameCount.text = "총 " + allCount + " 경기";
        outCome.text = winCount + "승 " + loseCount + "패 " + drawCount + " 무";
    }

    void CalculateWinRate()
    {

        if (allCount == 0) // 나눗셈 0 방지
            winRate = 0;
        else
        {
            winRate = ((float)winCount / (winCount + loseCount)) * 100f;
            winRateD = (((float)winCount + (0.5f * drawCount)) / allCount) * 100f;
        }
           

        Debug.Log("winRate: " + winRate+"%" +"winrateD"+winRateD+"%");

        
    }


}
