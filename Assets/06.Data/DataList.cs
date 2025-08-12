using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataList : MonoBehaviour
{
    public static DataList Instance { get; private set; }
    public List<TeamData> teams;
    public List<StadiumData> stadiums;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }



}
