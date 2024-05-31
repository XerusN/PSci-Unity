using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerManager : MonoBehaviour
{
    public Data data;
    private Plot plot;

    public Camera mainCamera;

    public int currentIteration = 0;
    public int currentPlotedValue = 1;
    public float valueMin = -1f;
    public float valueMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        data = this.gameObject.GetComponent<Data>();
        plot = this.gameObject.GetComponent<Plot>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitVisualizer()
    {
        data.InitData();
        for (int i = 0; i < data.data.Length; i++)
        {
            data.ReadDataTechplot(i);
        }
        PlaceCamera();

        valueMin = data.data[0].uMin;
        valueMax = data.data[0].uMax;
        currentPlotedValue = 1;
        currentIteration = 0;

        UpdatePlot(1, currentPlotedValue, valueMin, valueMax);
    }

    public IEnumerator RunAnimatedPlot(int iterationIni, int plotedValue)
    {

        float time = 0f;
        float dTime;

        for (int i = iterationIni; i < data.data.Length; i++)
        {
            currentIteration = i;
            UpdatePlot(i, plotedValue, valueMin, valueMax);
            dTime = data.data[i].time - time;
            time = data.data[i].time;
            yield return new WaitForSeconds(dTime);
        }
    }

    public void UpdatePlot(int iteration, int plotedValue, float min, float max)
    {

        switch (plotedValue)
        {
            case > 0 when plotedValue == 1:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].u, min, max);
                break;
            case > 0 when plotedValue == 2:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].v, min, max);
                break;
            case > 0 when plotedValue == 3:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].mag, min, max);
                break;
            case > 0 when plotedValue == 4:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].p, min, max);
                break;
        }
    }

    public void PlaceCamera()
    {
        mainCamera.transform.position = new Vector3(data.data[0].xMax / 2f, data.data[0].yMax / 2f, -5f*data.data[0].xMax);
        mainCamera.orthographicSize = 1f;
    }

    public void PlayButton()
    {
        StopAllCoroutines();
        StartCoroutine(RunAnimatedPlot(currentIteration, currentPlotedValue));
    }

    public void PauseButton()
    {
        StopAllCoroutines();
    }

    public void NextFrameButton()
    {
        StopAllCoroutines();
        if (currentIteration + 1 < data.data.Length)
        {
            currentIteration += 1;
            UpdatePlot(currentIteration, currentPlotedValue, valueMin, valueMax);
        }
    }

    public void PreviousFrameButton()
    {
        StopAllCoroutines();
        if (currentIteration - 1 > 0)
        {
            currentIteration -= 1;
            UpdatePlot(currentIteration, currentPlotedValue, valueMin, valueMax);
        }
    }

    public void FirstFrameButton()
    {
        StopAllCoroutines();
        currentIteration = 0;
        UpdatePlot(currentIteration, currentPlotedValue, valueMin, valueMax);
    }

    public void LastFrameButton()
    {
        StopAllCoroutines();
        currentIteration = data.data.Length - 1;
        UpdatePlot(currentIteration, currentPlotedValue, valueMin, valueMax);
    }

    public void RescaleButton()
    {
        StopAllCoroutines();
        
        valueMin = data.data[currentIteration].uMin;
        valueMax = data.data[currentIteration].uMax;
        switch (currentPlotedValue)
        {
            case > 0 when currentPlotedValue == 1:
                valueMin = data.data[currentIteration].uMin;
                valueMax = data.data[currentIteration].uMax;
                break;
            case > 0 when currentPlotedValue == 2:
                valueMin = data.data[currentIteration].vMin;
                valueMax = data.data[currentIteration].vMax;
                break;
            case > 0 when currentPlotedValue == 3:
                valueMin = data.data[currentIteration].magMin;
                valueMax = data.data[currentIteration].magMax;
                break;
            case > 0 when currentPlotedValue == 4:
                valueMin = data.data[currentIteration].pMin;
                valueMax = data.data[currentIteration].pMax;
                break;
        }
        UpdatePlot(currentIteration, currentPlotedValue, valueMin, valueMax);
    }

    public void UpdatePlotedValue(int plotedValue)
    {
        StopAllCoroutines();
        currentPlotedValue = plotedValue + 1;
        UpdatePlot(currentIteration, currentPlotedValue, valueMin, valueMax);
    }
}
