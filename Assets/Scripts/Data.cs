using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;

public class Data : MonoBehaviour
{
    public Input input;

    public double time;
    public Vector2Int n;
    public double[,] x;
    public double[,] y;
    public double[,] u;
    public double[,] v;
    public double[,] mag;
    public double[,] p;

    public double xMax;
    public double yMax;

    public double uMin;
    public double uMax;

    public double vMin;
    public double vMax;

    public double magMin;
    public double magMax;

    public double pMin;
    public double pMax;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void ReadDataTechplot(int iteration)
    {
        MainManager mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        string fileName = mainManager.cfdCodePath + "/output/resTECPLOT_" + (iteration * input.frame).ToString().Trim() + ".dat";
        StreamReader reader = new StreamReader(fileName);
        string str = "";
        int i;
        str = reader.ReadLine();
        str = reader.ReadLine();
        str = reader.ReadLine();
        Double.TryParse(str[10..32], out time);
        Int32.TryParse(str[44..48], out i);
        n.x = i;
        Int32.TryParse(str[52..56], out i);
        n.y = i;

        x = new double[n.x + 2, n.y + 2];
        y = new double[n.x + 2, n.y + 2];
        u = new double[n.x + 2, n.y + 2];
        v = new double[n.x + 2, n.y + 2];
        mag = new double[n.x + 2, n.y + 2];
        p = new double[n.x + 2, n.y + 2];

        for (i = 1; i < n.x+1; i++)
        {
            for (int j = 1; j < n.y+1; j++)
            {
                str = reader.ReadLine();
                Double.TryParse(str[0..20], out x[i,j]);
                Double.TryParse(str[21..41], out y[i,j]);
                Double.TryParse(str[42..62], out u[i, j]);
                Double.TryParse(str[63..83], out v[i, j]);
                Double.TryParse(str[84..104], out mag[i, j]);
                Double.TryParse(str[105..125], out p[i, j]);
            }
        }


        for (int j = 0; j < n.y + 2; j++)
        {
            x[0, j] = x[1, j];
            x[n.x+1, j] = x[n.x, j];
            y[0, j] = y[1, j];
            y[n.x + 1, j] = y[n.x, j];
            u[0, j] = u[1, j];
            u[n.x + 1, j] = u[n.x, j];
            v[0, j] = v[1, j];
            v[n.x + 1, j] = v[n.x, j];
            mag[0, j] = mag[1, j];
            mag[n.x + 1, j] = mag[n.x, j];
            p[0, j] = p[1, j];
            p[n.x + 1, j] = p[n.x, j];
        }
        for (i = 0; i < n.x + 2; i++)
        {
            x[i, 0] = x[i, 1];
            x[i, n.x + 1] = x[i, n.y];
            y[i, 0] = y[i, 1];
            y[i, n.x + 1] = y[i, n.y];
            u[i, 0] = u[i, 1];
            u[i, n.x + 1] = u[i, n.y];
            v[i, 0] = v[i, 1];
            v[i, n.x + 1] = v[i, n.y];
            mag[i, 0] = mag[i, 1];
            mag[i, n.x + 1] = mag[i, n.y];
            p[i, 0] = p[i, 1];
            p[i, n.x + 1] = p[i, n.y];
        }

        xMax = 0f;
        yMax = 0f;

        uMax = 0f;
        vMax = 0f;
        magMax = 0f;
        pMax = 0f;

        uMin = 0f;
        vMin = 0f;
        magMin = 0f;
        pMin = 0f;

        for (i = 1; i < n.x+1; i++)
        {
            for (int j = 1; j < n.x + 1; j++)
            {
                if (x[i, j] > xMax) { xMax = x[i, j]; }
                if (y[i, j] > yMax) { yMax = y[i, j]; }

                if (u[i, j] > uMax) { uMax = u[i, j]; }
                if (v[i, j] > vMax) { vMax = v[i, j]; }
                if (mag[i, j] > magMax) { magMax = mag[i, j]; }
                if (p[i, j] > pMax) { pMax = p[i, j]; }

                if (u[i, j] < uMin) { uMin = u[i, j]; }
                if (v[i, j] < vMin) { vMin = v[i, j]; }
                if (mag[i, j] < magMin) { magMin = mag[i, j]; }
                if (p[i, j] < pMin) { pMin = p[i, j]; }
            }
        }

        reader.Close();
    }
}
