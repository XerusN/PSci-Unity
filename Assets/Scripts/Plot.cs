using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    public Vector2Int meshResolution = new Vector2Int(11, 11);
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    private float[] colorsFloat;
    public Gradient gradient;

    public ComputeShader computeShader;
    ComputeBuffer vertexBuffer;
    ComputeBuffer colorBuffer;
    ComputeBuffer xBuffer;
    ComputeBuffer yBuffer;
    ComputeBuffer valueBuffer;
    //ComputeBuffer triangleBuffer;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        vertices = new Vector3[meshResolution.x * meshResolution.y];
        triangles = new int[(meshResolution.x - 1) * (meshResolution.y - 1) * 2 * 3];
        colors = new Color[vertices.Length];
        colorsFloat = new float[vertices.Length];
        this.GetComponent<MeshFilter>().mesh = this.mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Update2DMeshPlot(Data data, double[,] plotedValue, double min, double max)
    {

        float dx = (float)data.xMax / ((float)meshResolution.x - 1);
        float dy = (float)data.yMax / ((float)meshResolution.y - 1);

        for (int i = 0; i < meshResolution.x; i++)
        {
            for (int j = 0; j < meshResolution.y; j++)
            {
                vertices[j * meshResolution.x + i] = new Vector2(i * dx, j * dy);
            }
        }

        int triIndex = 0;
        for (int i = 0; i < meshResolution.x - 1; i++)
        {
            for (int j = 0; j < meshResolution.y - 1; j++)
            {
                triangles[triIndex] = i + 1 + j * meshResolution.x;
                triangles[triIndex + 1] = i + j * meshResolution.x;
                triangles[triIndex + 2] = i + (j + 1) * meshResolution.x;
                triangles[triIndex + 3] = i + 1 + j * meshResolution.x;
                triangles[triIndex + 4] = i + (j + 1) * meshResolution.x;
                triangles[triIndex + 5] = i + 1 + (j + 1) * meshResolution.x;

                triIndex += 6;
            }
        }

        mesh.Clear();
        ColorMesh(data, plotedValue, min, max);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();

    }

    private void ColorMesh(Data data, double[,] plotedValue, double min, double max)
    {
        int k = 0;
        int l = 0;
        for (int i = 0; i < meshResolution.x; i++)
        {
            for (int j = 0; j < meshResolution.y; j++)
            {
                k = 0;
                l = 0;
                while ((k + 1 < data.n.x))
                {
                    if ((data.x[k, 0] <= vertices[j * meshResolution.x + i].x) & (data.x[k + 1, 0] >= vertices[j * meshResolution.x + i].x)) { break; }
                    k++;
                }
                while ((l + 1 < data.n.y))
                {
                    if ((data.y[0, l] <= vertices[j * meshResolution.x + i].y) & (data.y[0, l + 1] >= vertices[j * meshResolution.x + i].y)) { break; }
                    l++;
                }
                double t = 0d;
                if (Mathd.Abs(data.x[k + 1, l] - data.x[k, l]) > 0)
                {
                    t = ((double)vertices[j * meshResolution.x + i].x - data.x[k, l]) / (data.x[k + 1, l] - data.x[k, l]);
                }

                float value1 = (float)Mathd.Lerp(plotedValue[k, l], plotedValue[k + 1, l], t);

                float value2 = (float)Mathd.Lerp(plotedValue[k, l + 1], plotedValue[k + 1, l + 1], t);

                if (Mathd.Abs(data.y[k, l + 1] - data.y[k, l]) > 0)
                {
                    t = ((double)vertices[j * meshResolution.x + i].y - data.y[k, l]) / (data.y[k, l + 1] - data.y[k, l]);
                }
                else
                {
                    t = 0d;
                }

                float value = (float)Mathd.Lerp(value1, value2, t);

                vertices[j * meshResolution.x + i].z = -value;

                colors[j * meshResolution.x + i] = gradient.Evaluate(Mathf.InverseLerp((float)min, (float)max, value));
            }
        }
    }

    public void Update2DMeshPlotShader(Data data, double[,] plotedValue, double min, double max)
    {
        double[] xFlat = new double[data.x.Length];
        double[] yFlat = new double[data.x.Length];
        double[] uFlat = new double[data.x.Length];
        for (int i = 0; i < data.n.x; i++)
        {
            for (int j = 0; j < data.n.y; j++)
            {
                xFlat[i + data.n.x * j] = data.x[i, j];
                yFlat[i + data.n.x * j] = data.y[i, j];
                uFlat[i + data.n.x * j] = data.u[i, j];
            }
        }

        vertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float)*3);
        vertexBuffer.SetData(vertices);
        computeShader.SetBuffer(0, "vertices", vertexBuffer);

        colorBuffer = new ComputeBuffer(colorsFloat.Length, sizeof(float));
        colorBuffer.SetData(colorsFloat);
        computeShader.SetBuffer(0, "colors", colorBuffer);

        xBuffer = new ComputeBuffer(xFlat.Length, sizeof(double));
        xBuffer.SetData(xFlat);
        computeShader.SetBuffer(0, "x", xBuffer);

        yBuffer = new ComputeBuffer(yFlat.Length, sizeof(double));
        yBuffer.SetData(yFlat);
        computeShader.SetBuffer(0, "y", yBuffer);

        valueBuffer = new ComputeBuffer(uFlat.Length, sizeof(double));
        valueBuffer.SetData(uFlat);
        computeShader.SetBuffer(0, "values", valueBuffer);

        //triangleBuffer = new ComputeBuffer(triangles.Length, sizeof(int));
        //triangleBuffer.SetData(triangles);
        //computeShader.SetBuffer(0, "triangles", triangleBuffer);

        computeShader.SetInt("meshResolutionX", meshResolution.x);
        computeShader.SetInt("meshResolutionY", meshResolution.y);
        computeShader.SetInt("dataMeshSizeX", data.n.x);
        computeShader.SetInt("dataMeshSizeY", data.n.y);
        computeShader.SetFloat("dataLengthX", (float) data.xMax);
        computeShader.SetFloat("dataLengthY", (float) data.yMax);
        computeShader.SetFloat("valueMin", (float) data.uMin);
        computeShader.SetFloat("valueMax", (float) data.uMax);

        computeShader.Dispatch(0, meshResolution.x / 8 + 1, meshResolution.y / 8 + 1, 1);

        vertexBuffer.GetData(vertices);
        colorBuffer.GetData(colorsFloat);
        //triangleBuffer.GetData(triangles);

        vertexBuffer.Release();
        colorBuffer.Release();
        xBuffer.Release();
        yBuffer.Release();
        valueBuffer.Release();
        //triangleBuffer.Release();

        int triIndex = 0;
        for (int i = 0; i < meshResolution.x - 1; i++)
        {
            for (int j = 0; j < meshResolution.y - 1; j++)
            {
                triangles[triIndex] = i + 1 + j * meshResolution.x;
                triangles[triIndex + 1] = i + j * meshResolution.x;
                triangles[triIndex + 2] = i + (j + 1) * meshResolution.x;
                triangles[triIndex + 3] = i + 1 + j * meshResolution.x;
                triangles[triIndex + 4] = i + (j + 1) * meshResolution.x;
                triangles[triIndex + 5] = i + 1 + (j + 1) * meshResolution.x;

                triIndex += 6;
            }
        }

        for (int i = 0; i < colors.Length; i++)
        {
            if (colorsFloat[i] > 2 | colorsFloat[i] < 0)
            {
                //Debug.Log((i % meshResolution.x).ToString() + " " + (i / meshResolution.x).ToString() + " " + colorsFloat[i].ToString());
            }
            colors[i] = gradient.Evaluate(Mathf.InverseLerp((float)min, (float)max, colorsFloat[i]));
        }

        //for (int i = 0; i < triangles.Length; i++)
        //{
        //    if (triangles[i] == 0)
        //    {
        //        Debug.Log(i.ToString() + " " + triangles[i].ToString());
        //    }
        //}

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].z == 0f & vertices[i].y == 0f & vertices[i].x == 0f)
            {
                Debug.Log((i % meshResolution.x).ToString() + " " + (i / meshResolution.x).ToString() + " " + vertices[i].ToString());
            }
        }

        int[] count = new int[vertices.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            count[triangles[i]] += 1;
        }

        for (int i = 0; i < count.Length; i++)
        {
            if (count[i] != 6)
            {
                if (!((((i % meshResolution.x)%(meshResolution.x - 1) == 0) | ((i / meshResolution.x)%(meshResolution.y - 1) == 0)) & (count[i] == 3)))
                {
                    Debug.Log((i % meshResolution.x).ToString() + " " + (i / meshResolution.x).ToString() + " " + count[i].ToString());
                }
            }
        }

        Debug.Log((triangles.Length - 2).ToString() + " " + triangles[triangles.Length - 2].ToString() + " " + vertices[triangles[triangles.Length - 2]].ToString());
        Debug.Log("Dernier indice triangle " + (triangles.Length - 1).ToString() + " " + triangles[triangles.Length - 1].ToString() + " " + vertices[triangles[triangles.Length - 1]].ToString());
        Debug.Log((vertices.Length - 1).ToString() + " " + vertices[vertices.Length - 1].ToString());

        mesh.Clear();
        //ColorMesh(data, plotedValue, min, max);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }








}
