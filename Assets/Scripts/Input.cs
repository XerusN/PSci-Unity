using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Input : ScriptableObject
{

    [Header("Grid")]
    public Vector2Int n = new Vector2Int(50, 51);
    public double lx = 1.0d;
    public double ly = 1.0d;

    [Header("Time")]
    public double tf = 10.0d;
    public int frame = 10;

    [Header("Physical Variables")]
    public double re = 100;
    public double density = 1.0d;
    public double cfl = 1.0d;
    public double fo = 0.1d;
    public double characteristicLength = 1.0;
    public double characteristicSpeed = 1.0;
    public string scheme = "CD4";

    [Header("Initial Condition")]
    public bool custom = false;
    public double[] uSides = { 0.0d, 0.0d, 0.0d, 1.0d };
    public double[] vSides = { 0.0d, 0.0d, 0.0d, 0.0d };
    public double uIni = 0.0d;
    public double vIni = 0.0d;

}
