using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserSettingManager : MonoBehaviour
{
    private static string path = Application.persistentDataPath + "/user_setting.json";
    // Start is called before the first frame update

    public static void Save(UserSettings settings)
    {
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(path, json);
    }

    public static UserSettings Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<UserSettings>(json);
        }
        else
        {
            return new UserSettings();
        }
    }
}
