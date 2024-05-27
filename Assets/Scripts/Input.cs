using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Input : ScriptableObject
{

    [Header("Grid")]
    public Vector2Int n = new Vector2Int(50, 51);
    public float lx = 1.0f;
    public float ly = 1.0f;

    [Header("Time")]
    public float tf = 10.0f;
    public int frame = 10;

    [Header("Physical Variables")]
    public float re = 100;
    public float density = 1.0f;
    public float cfl = 1.0f;
    public float fo = 0.1f;
    public float characteristicLength = 1.0f;
    public float characteristicSpeed = 1.0f;
    public string scheme = "CD4";

    [Header("Initial Condition")]
    public bool custom = false;
    public float[] uSides = { 0.0f, 0.0f, 0.0f, 1.0f };
    public float[] vSides = { 0.0f, 0.0f, 0.0f, 0.0f };
    public float uIni = 0.0f;
    public float vIni = 0.0f;

}
