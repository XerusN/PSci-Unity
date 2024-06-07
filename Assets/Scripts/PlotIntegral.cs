using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotIntegral : MonoBehaviour
{

    private LineRenderer line;
    public GameObject simulationManager;

    public float sizeX = 1f;
    public float sizeY = 0.5f;
    public float offsetX = 0.2f;
    public float offsetY = 1f;
    public float width = 0.001f;

    public float integralsMax = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    public void UpdateIntegralPlot()
    {
        line.startWidth = width;
        line.endWidth = width;


        line.positionCount = simulationManager.GetComponent<RunCode>().integrals.Count;
        integralsMax = 0f;

        for (int i = 0; i < line.positionCount; i++)
        {
            if (simulationManager.GetComponent<RunCode>().integrals[i] > integralsMax)
            {
                integralsMax = simulationManager.GetComponent<RunCode>().integrals[i];
            }
        }

        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, new Vector3(offsetX + (float) i / (float) (line.positionCount - 1) * sizeX, offsetY + (float) simulationManager.GetComponent<RunCode>().integrals[i] / (float) (integralsMax) * sizeX, 0f));
        }
        
    }
}
