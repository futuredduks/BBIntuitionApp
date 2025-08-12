using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStadium", menuName = "Baseball/Stadium")]
public class StadiumData : ScriptableObject
{
    public string stadiumId;
    public string stadiumNameE;
    public string stadiumName;
    public string location;
    public Sprite stadiumImage;
}
