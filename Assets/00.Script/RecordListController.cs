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


    public TMP_Text allGameCount;
    public TMP_Text outCome;

    int allCount = 0;
    int winCount = 0;
    int loseCount = 0;
    int drawCount = 0;


    // Start is called before the first frame update

    public void CreateList()
    {
        List<GameRecord> list = RecordInputUI.Instance.GameRecordList.records;
        Debug.Log("list Count" + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GameObject recordPrefab = Instantiate(scoreRecordPrefab, scoreContent);

            if (recordPrefab.TryGetComponent<RecordPrefab>(out RecordPrefab recordSc))
            {
                recordSc.SetRecordData(list[i]);
            }
        }

        RecordCalculation();
        InputText();
    }

    void RecordCalculation()
    {
        List<GameRecord> record = RecordInputUI.Instance.GameRecordList.records;
        //allCount = record.Count;

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


}
