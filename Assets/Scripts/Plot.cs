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
    public Gradient gradient;

    // Start is called before the first frame update
    void Awake()
    {

        mesh = new Mesh();
        vertices = new Vector3[meshResolution.x * meshResolution.y];
        triangles = new int[(meshResolution.x - 1) * (meshResolution.y - 1) * 2 * 3];
        this.GetComponent<MeshFilter>().mesh = this.mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateMeshPlot(Data data, double[,] plotedValue, double min, double max)
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
        colors = new Color[vertices.Length];
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
                    if ((data.y[0, l] <= vertices[j * meshResolution.y + i].y) & (data.y[0, l + 1] >= vertices[j * meshResolution.x + i].y)) { break; }
                    l++;
                }
                double t = 0d;
                if (Mathd.Abs(data.y[k, l + 1] - data.y[k, l + 1]) > 0)
                {
                    t = ((double)vertices[j * meshResolution.y + i].x - data.x[k, l]) / (data.x[k + 1, l] - data.x[k, l]);
                }

                float value1 = (float)Mathd.Lerp(plotedValue[k, l], plotedValue[k + 1, l], t);

                float value2 = (float)Mathd.Lerp(plotedValue[k, l + 1], plotedValue[k + 1, l + 1], t);

                if (Mathd.Abs(data.y[k, l + 1] - data.y[k, l + 1]) > 0)
                {
                    t = ((double)vertices[j * meshResolution.y + i].y - data.y[k, l]) / (data.y[k, l + 1] - data.y[k, l + 1]);
                }
                else
                {
                    t = 0d;
                }

                float value = (float)Mathd.Lerp(value1, value2, t);

                vertices[j * meshResolution.y + i].z = - value;

                colors[j * meshResolution.y + i] = gradient.Evaluate(Mathf.InverseLerp((float)min, (float)max, value));
            }
        }
    }

}
