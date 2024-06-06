using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotIterations : MonoBehaviour
{

    private LineRenderer line;
    public GameObject simulationManager;

    public float sizeX = 2f;
    public float sizeY = 1f;

    public float iterationsMax = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    public void UpdateIterationPlot()
    {
        line.positionCount = simulationManager.GetComponent<RunCode>().iterations.Length;
        iterationsMax = 0f;

        for (int i = 0; i < line.positionCount; i++)
        {
            if (simulationManager.GetComponent<RunCode>().iterations[i] > iterationsMax)
            {
                iterationsMax = simulationManager.GetComponent<RunCode>().iterations[i];
            }
        }

        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, new Vector3(i / (line.positionCount - 1) * sizeX, simulationManager.GetComponent<RunCode>().iterations[i] / (iterationsMax) * sizeX, 0f));
        }
        
    }
}
