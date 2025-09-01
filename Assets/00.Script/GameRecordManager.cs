using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// 직관 정보 저장 불러오기 코드

public class GameRecordManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "records.json");

    }

    public void SaveRecords(GameRecordList data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

    }

    public GameRecordList LoadRecords()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameRecordList>(json);
        }
        return new GameRecordList();
    }


    public void SaveImage(Texture2D image, string fileName)
    {
        byte[] bytes = image.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, bytes);
    }

    public Texture2D LoadImage(string path)
    {
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            return tex;
        }
        return null;
    }


    public enum GameResult {
        Win,
        Lose,
        Draw,
        Unknown
    }
    //승패 결정 
    public static GameResult CalcualteIsWin(string myTeam, string team_R, string team_L, int score_R, int score_L)
    {
        if ((myTeam != team_R) && (myTeam != team_L))
        {
            Debug.Log("myteam null");
            return GameResult.Unknown;
        }
        bool isTeamR = (team_R == myTeam);
        bool isTeamL = (team_L == myTeam);

        if (score_R == score_L)
            return GameResult.Draw;
            

        if (isTeamR)
            return (score_R > score_L) ? GameResult.Win : GameResult.Lose;
        else
            return (score_L > score_R) ? GameResult.Win : GameResult.Lose;

    }
}
