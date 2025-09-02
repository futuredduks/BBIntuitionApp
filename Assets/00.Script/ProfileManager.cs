using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{

    public static ProfileManager Instance { get; private set; }
    // Start is called before the first frame update
    public TMP_Text nickName;
    public TMP_Text team;


    public Transform teamViewContent;
    public GameObject teamPrefab;

    public List<GameObject> teambtnList=new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Start()
    {
        CreateList();
    }

    void CreateList()
    {
        List<TeamData> td = DataList.Instance.teams;
        for (int i = 0; i < td.Count; i++)
        {
            GameObject tP = Instantiate(teamPrefab, teamViewContent);
            if (tP.TryGetComponent<ProfileTeamPrefab>(out ProfileTeamPrefab pp))
            {
                pp.Set(td[i]);
                teambtnList.Add(tP);
            }
        }
    }



    public void CheckTeam()
    {
        team.text = "나의 최애 팀:"+"\n"+UserSettings.Instance.MyTeamName;
    }
}
