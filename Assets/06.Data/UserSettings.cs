using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserSettings : MonoBehaviour
{
    public static UserSettings Instance { get; private set; }
    [SerializeField]
    private string nickName;
    [SerializeField]
    private string myTeamName;
    [SerializeField]
    private TeamData myteamData;

    public string NickName => nickName;
    public string MyTeamName => myTeamName;
    public TeamData MyTeamData => myteamData;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void SetInfoTeamData(TeamData mtd)
    {
        myteamData = mtd;
        myTeamName = mtd.teamName;
    }

    public void SetInfoNickName(string _nickname)
    {
        nickName = _nickname;
    }
    
}
