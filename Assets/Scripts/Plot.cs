using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    float[] xFlat;
    float[] yFlat;
    float[] valueFlat;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        
        this.GetComponent<MeshFilter>().mesh = this.mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Update2DMeshPlot(Data data, int iteration, float[,] plotedValue, float min, float max)
    {

        float dx = (float)data.data[iteration].xMax / ((float)meshResolution.x - 1);
        float dy = (float)data.data[iteration].yMax / ((float)meshResolution.y - 1);

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
        ColorMesh(data, iteration, plotedValue, min, max);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();

    }

    private void ColorMesh(Data data, int iteration, float[,] plotedValue, float min, float max)
    {
        int k = 0;
        int l = 0;
        for (int i = 0; i < meshResolution.x; i++)
        {
            for (int j = 0; j < meshResolution.y; j++)
            {
                k = 0;
                l = 0;
                while ((k + 1 < data.data[iteration].n.x))
                {
                    if ((data.data[iteration].x[k, 0] <= vertices[j * meshResolution.x + i].x) & (data.data[iteration].x[k + 1, 0] >= vertices[j * meshResolution.x + i].x)) { break; }
                    k++;
                }
                while ((l + 1 < data.data[iteration].n.y))
                {
                    if ((data.data[iteration].y[0, l] <= vertices[j * meshResolution.x + i].y) & (data.data[iteration].y[0, l + 1] >= vertices[j * meshResolution.x + i].y)) { break; }
                    l++;
                }
                float t = 0f;
                if (Mathd.Abs(data.data[iteration].x[k + 1, l] - data.data[iteration].x[k, l]) > 0)
                {
                    t = ((float)vertices[j * meshResolution.x + i].x - data.data[iteration].x[k, l]) / (data.data[iteration].x[k + 1, l] - data.data[iteration].x[k, l]);
                }

                float value1 = (float)Mathd.Lerp(plotedValue[k, l], plotedValue[k + 1, l], t);

                float value2 = (float)Mathd.Lerp(plotedValue[k, l + 1], plotedValue[k + 1, l + 1], t);

                if (Mathd.Abs(data.data[iteration].y[k, l + 1] - data.data[iteration].y[k, l]) > 0)
                {
                    t = ((float)vertices[j * meshResolution.x + i].y - data.data[iteration].y[k, l]) / (data.data[iteration].y[k, l + 1] - data.data[iteration].y[k, l]);
                }
                else
                {
                    t = 0f;
                }

                float value = (float)Mathd.Lerp(value1, value2, t);

                vertices[j * meshResolution.x + i].z = -value;

                colors[j * meshResolution.x + i] = gradient.Evaluate(Mathf.InverseLerp((float)min, (float)max, value));
            }
        }
    }

    public void Update2DMeshPlotShader(Data data, int iteration, float[,] plotedValue, float min, float max)
    {
        if (vertexBuffer == null)
        {
            AllocateBuffer(data, iteration);
            //Debug.Log("Allocated mesh");
        }

        if ((data.data[iteration].n.x*data.data[iteration].n.y != valueFlat.Length) | meshResolution.x*meshResolution.y != vertices.Length)
        {
            ReleaseBuffer();
            AllocateBuffer(data, iteration);
            //Debug.Log("Resized mesh");
        }

        for (int i = 0; i < data.data[iteration].n.x; i++)
        {
            for (int j = 0; j < data.data[iteration].n.y; j++)
            {
                xFlat[i + data.data[iteration].n.x * j] = data.data[iteration].x[i, j];
                yFlat[i + data.data[iteration].n.x * j] = data.data[iteration].y[i, j];
                valueFlat[i + data.data[iteration].n.x * j] = plotedValue[i, j];
            }
        }

        vertexBuffer.SetData(vertices);
        computeShader.SetBuffer(0, "vertices", vertexBuffer);

        colorBuffer.SetData(colorsFloat);
        computeShader.SetBuffer(0, "colors", colorBuffer);

        xBuffer.SetData(xFlat);
        computeShader.SetBuffer(0, "x", xBuffer);

        yBuffer.SetData(yFlat);
        computeShader.SetBuffer(0, "y", yBuffer);

        valueBuffer.SetData(valueFlat);
        computeShader.SetBuffer(0, "values", valueBuffer);

        //triangleBuffer.SetData(triangles);
        //computeShader.SetBuffer(0, "triangles", triangleBuffer);

        computeShader.SetInt("meshResolutionX", meshResolution.x);
        computeShader.SetInt("meshResolutionY", meshResolution.y);
        computeShader.SetInt("dataMeshSizeX", data.data[iteration].n.x);
        computeShader.SetInt("dataMeshSizeY", data.data[iteration].n.y);
        computeShader.SetFloat("dataLengthX", (float) data.data[iteration].xMax);
        computeShader.SetFloat("dataLengthY", (float) data.data[iteration].yMax);
        computeShader.SetFloat("valueMin", (float) min);
        computeShader.SetFloat("valueMax", (float) max);

        computeShader.Dispatch(0, meshResolution.x / 8 + 1, meshResolution.y / 8 + 1, 1);

        vertexBuffer.GetData(vertices);
        colorBuffer.GetData(colorsFloat);
        //triangleBuffer.GetData(triangles);

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = gradient.Evaluate(Mathf.InverseLerp((float)min, (float)max, colorsFloat[i]));
        }

        mesh.Clear();
        //ColorMesh(data, plotedValue, min, max);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    void AllocateBuffer(Data data, int iteration)
    {
        vertices = new Vector3[meshResolution.x * meshResolution.y];
        triangles = new int[(meshResolution.x - 1) * (meshResolution.y - 1) * 2 * 3];
        colors = new Color[vertices.Length];
        colorsFloat = new float[vertices.Length];

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

        xFlat = new float[data.data[iteration].n.x * data.data[iteration].n.y];
        yFlat = new float[data.data[iteration].n.x * data.data[iteration].n.y];
        valueFlat = new float[data.data[iteration].n.x * data.data[iteration].n.y];

        vertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);

        colorBuffer = new ComputeBuffer(colorsFloat.Length, sizeof(float));

        xBuffer = new ComputeBuffer(xFlat.Length, sizeof(float));
        
        yBuffer = new ComputeBuffer(yFlat.Length, sizeof(float));

        valueBuffer = new ComputeBuffer(valueFlat.Length, sizeof(float));

        //triangleBuffer = new ComputeBuffer(triangles.Length, sizeof(int));
    }

    void ReleaseBuffer()
    {
        vertexBuffer.Release();
        colorBuffer.Release();
        xBuffer.Release();
        yBuffer.Release();
        valueBuffer.Release();
        //triangleBuffer.Release();
    }

    private void OnDestroy()
    {
        if (vertexBuffer != null)
        {
            ReleaseBuffer();
        }
    }

}
