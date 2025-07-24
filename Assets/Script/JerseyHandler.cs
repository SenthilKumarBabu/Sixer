using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerseyHandler : MonoBehaviour
{
    public static JerseyHandler instance;
    public Texture[] WicketKeeperJersey;
    public Texture[] FielderJersey;
    public Texture[] BatsmanJersey;
    public Material[] battingTeamMat;   //0-uniform     //helmat   
    public Color32[] AccessBaseColor = new Color32[10];

    void Awake()
    {
        instance = this;
    }

    public void UpdateBatsmanMaterials()
    {
        battingTeamMat[0].SetTexture("_MainTex", BatsmanJersey[CONTROLLER.JerseyIDX]);
        battingTeamMat[1].SetColor("_BaseColor", AccessBaseColor[CONTROLLER.JerseyIDX]);
    }
}
