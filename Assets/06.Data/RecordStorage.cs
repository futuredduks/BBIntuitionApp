using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordStorage : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "record.json");

    public static void SaveRecords(GameRecordList recordList)
    {
        string json = JsonUtility.ToJson(recordList, true);
        File.WriteAllText(SavePath, json);

    }

    public static GameRecordList LoadRecords()
    {
        if (!File.Exists(SavePath))
        {
            return new GameRecordList();
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameRecordList>(json);
    }
}
