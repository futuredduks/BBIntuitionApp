using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class UserSettingManager : MonoBehaviour
{
    public TMP_InputField tmp_nicknameField;
    private static string path;
    // Start is called before the first frame update
    private void Awake()
    {
        path = Application.persistentDataPath + "/settings.json";
    }
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

    public void SaveUserInfo()
    {
        Save(UserSettings.Instance);
        UIManager.Instance.setProfilePanel?.SetActive(false);
    }

    public void InputNickName()
    {
        UserSettings.Instance.SetInfoNickName(tmp_nicknameField.text);
    }
}
