using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordPrefab : MonoBehaviour
{
    public TMP_Text nameR;
    public TMP_Text nameL;

    public TMP_Text date;
    public TMP_Text scoreR;
    public TMP_Text scoreL;
    public TMP_Text stadium;
    // Start is called before the first frame update

    public void SetRecordData(GameRecord record)
    {
        nameR.text = record.teamR;
        nameL.text = record.teamL;

        date.text = record.date;

        scoreR.text = record.scoreR;
        scoreL.text = record.scoreL;

        stadium.text = record.stadiumName;
    }
}
