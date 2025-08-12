using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "NewTeam", menuName = "Baseball/Team")]
public class TeamData : ScriptableObject
{
    public string teamId;
    public string teamNameE;
    public string teamName;
    public Color teamColor;
    public Sprite teamLogo;
    public Sprite hatImage;
    public StadiumData homeStadium; 
}
