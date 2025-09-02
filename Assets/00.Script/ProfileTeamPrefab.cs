using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ProfileTeamPrefab : MonoBehaviour
{
    public Image logo;
    public TMP_Text teamName;
    public TeamData data;

    public Image background;
    // Start is called before the first frame update

    public void Set(TeamData td)
    {
        data = td;
        logo.sprite = td.teamLogo;
        teamName.text = td.teamName;
        background.color = td.teamColor;
    }

    public void Click()
    {
        UserSettings.Instance.SetInfoTeamData(data);
        ProfileManager.Instance.CheckTeam();

        
    }
}
