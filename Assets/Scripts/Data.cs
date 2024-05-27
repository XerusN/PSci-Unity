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

    public struct DataValue
    {
        public float time;
        public Vector2Int n;
        public float[,] x;
        public float[,] y;
        public float[,] u;
        public float[,] v;
        public float[,] mag;
        public float[,] p;

        public float xMax;
        public float yMax;

        public float uMin;
        public float uMax;

        public float vMin;
        public float vMax;

        public float magMin;
        public float magMax;

        public float pMin;
        public float pMax;
    }

    public DataValue[] data;

    MainManager mainManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    public void InitData(int size)
    {
        data = new DataValue[size];
    }

    public void ReadDataTechplot(int iteration)
    {
        string fileName = mainManager.cfdCodePath + "/output/resTECPLOT_" + (iteration * input.frame).ToString().Trim() + ".dat";
        StreamReader reader = new StreamReader(fileName);
        string str = "";
        int i;
        str = reader.ReadLine();
        str = reader.ReadLine();
        str = reader.ReadLine();
        float.TryParse(str[10..32], out data[iteration].time);
        Int32.TryParse(str[44..48], out i);
        data[iteration].n.x = i;
        Int32.TryParse(str[52..56], out i);
        data[iteration].n.y = i;

        data[iteration].x = new float[data[iteration].n.x + 2, data[iteration].n.y + 2];
        data[iteration].y = new float[data[iteration].n.x + 2, data[iteration].n.y + 2];
        data[iteration].u = new float[data[iteration].n.x + 2, data[iteration].n.y + 2];
        data[iteration].v = new float[data[iteration].n.x + 2, data[iteration].n.y + 2];
        data[iteration].mag = new float[data[iteration].n.x + 2, data[iteration].n.y + 2];
        data[iteration].p = new float[data[iteration].n.x + 2, data[iteration].n.y + 2];

        for (i = 1; i < data[iteration].n.x+1; i++)
        {
            for (int j = 1; j < data[iteration].n.y+1; j++)
            {
                str = reader.ReadLine();
                float.TryParse(str[0..20], out data[iteration].x[i,j]);
                float.TryParse(str[21..41], out data[iteration].y[i,j]);
                float.TryParse(str[42..62], out data[iteration].u[i, j]);
                float.TryParse(str[63..83], out data[iteration].v[i, j]);
                float.TryParse(str[84..104], out data[iteration].mag[i, j]);
                float.TryParse(str[105..125], out data[iteration].p[i, j]);
            }
        }


        for (int j = 0; j < data[iteration].n.y + 2; j++)
        {
            data[iteration].x[0, j] = data[iteration].x[1, j];
            data[iteration].x[data[iteration].n.x+1, j] = data[iteration].x[data[iteration].n.x, j];
            data[iteration].y[0, j] = data[iteration].y[1, j];
            data[iteration].y[data[iteration].n.x + 1, j] = data[iteration].y[data[iteration].n.x, j];
            data[iteration].u[0, j] = data[iteration].u[1, j];
            data[iteration].u[data[iteration].n.x + 1, j] = data[iteration].u[data[iteration].n.x, j];
            data[iteration].v[0, j] = data[iteration].v[1, j];
            data[iteration].v[data[iteration].n.x + 1, j] = data[iteration].v[data[iteration].n.x, j];
            data[iteration].mag[0, j] = data[iteration].mag[1, j];
            data[iteration].mag[data[iteration].n.x + 1, j] = data[iteration].mag[data[iteration].n.x, j];
            data[iteration].p[0, j] = data[iteration].p[1, j];
            data[iteration].p[data[iteration].n.x + 1, j] = data[iteration].p[data[iteration].n.x, j];
        }
        for (i = 0; i < data[iteration].n.x + 2; i++)
        {
            data[iteration].x[i, 0] = data[iteration].x[i, 1];
            data[iteration].x[i, data[iteration].n.x + 1] = data[iteration].x[i, data[iteration].n.y];
            data[iteration].y[i, 0] = data[iteration].y[i, 1];
            data[iteration].y[i, data[iteration].n.x + 1] = data[iteration].y[i, data[iteration].n.y];
            data[iteration].u[i, 0] = data[iteration].u[i, 1];
            data[iteration].u[i, data[iteration].n.x + 1] = data[iteration].u[i, data[iteration].n.y];
            data[iteration].v[i, 0] = data[iteration].v[i, 1];
            data[iteration].v[i, data[iteration].n.x + 1] = data[iteration].v[i, data[iteration].n.y];
            data[iteration].mag[i, 0] = data[iteration].mag[i, 1];
            data[iteration].mag[i, data[iteration].n.x + 1] = data[iteration].mag[i, data[iteration].n.y];
            data[iteration].p[i, 0] = data[iteration].p[i, 1];
            data[iteration].p[i, data[iteration].n.x + 1] = data[iteration].p[i, data[iteration].n.y];
        }

        data[iteration].xMax = 0f;
        data[iteration].yMax = 0f;

        data[iteration].uMax = 0f;
        data[iteration].vMax = 0f;
        data[iteration].magMax = 0f;
        data[iteration].pMax = 0f;

        data[iteration].uMin = 0f;
        data[iteration].vMin = 0f;
        data[iteration].magMin = 0f;
        data[iteration].pMin = 0f;

        for (i = 1; i < data[iteration].n.x+1; i++)
        {
            for (int j = 1; j < data[iteration].n.y + 1; j++)
            {
                if (data[iteration].x[i, j] > data[iteration].xMax) { data[iteration].xMax = data[iteration].x[i, j]; }
                if (data[iteration].y[i, j] > data[iteration].yMax) { data[iteration].yMax = data[iteration].y[i, j]; }

                if (data[iteration].u[i, j] > data[iteration].uMax) { data[iteration].uMax = data[iteration].u[i, j]; }
                if (data[iteration].v[i, j] > data[iteration].vMax) { data[iteration].vMax = data[iteration].v[i, j]; }
                if (data[iteration].mag[i, j] > data[iteration].magMax) { data[iteration].magMax = data[iteration].mag[i, j]; }
                if (data[iteration].p[i, j] > data[iteration].pMax) { data[iteration].pMax = data[iteration].p[i, j]; }

                if (data[iteration].u[i, j] < data[iteration].uMin) { data[iteration].uMin = data[iteration].u[i, j]; }
                if (data[iteration].v[i, j] < data[iteration].vMin) { data[iteration].vMin = data[iteration].v[i, j]; }
                if (data[iteration].mag[i, j] < data[iteration].magMin) { data[iteration].magMin = data[iteration].mag[i, j]; }
                if (data[iteration].p[i, j] < data[iteration].pMin) { data[iteration].pMin = data[iteration].p[i, j]; }
            }
        }

        reader.Close();
    }
}
