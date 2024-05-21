using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerManager : MonoBehaviour
{
    private Data data;
    private Plot plot;


    // Start is called before the first frame update
    void Start()
    {
        data = this.gameObject.GetComponent<Data>();
        plot = this.gameObject.GetComponent<Plot>();
        StartCoroutine(RunAnimatedPlot(0, 1, -1f, 1f));
        //UpdatePlot(data, 432, 1, -1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator RunAnimatedPlot(int iterationIni, int plotedValue, double min, double max)
    {
        for (int i = iterationIni; i < 432; i++)
        {
            UpdatePlot(data, i, plotedValue, min, max);
            Debug.Log(i);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Fini!");
    }

    public void UpdatePlot(Data data, int iteration, int plotedValue, double min, double max)
    {
        data.ReadDataTechplot(iteration);

        switch (plotedValue)
        {
            case > 0 when plotedValue == 1:
                plot.Update2DMeshPlotShader(data, data.u, min, max);
                break;
            case > 0 when plotedValue == 2:
                plot.Update2DMeshPlotShader(data, data.v, min, max);
                break;
            case > 0 when plotedValue == 2:
                plot.Update2DMeshPlotShader(data, data.mag, min, max);
                break;
            case > 0 when plotedValue == 3:
                plot.Update2DMeshPlotShader(data, data.p, min, max);
                break;
        }
    }
}
