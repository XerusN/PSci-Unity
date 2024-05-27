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
        //StartCoroutine(RunAnimatedPlot(0, 1, -1f, 1f));
        //UpdatePlot(data, 432, 1, -1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator RunAnimatedPlot(int iterationIni, int iterationMax, int plotedValue, float min, float max)
    {
        data.InitData();
        for (int i  = iterationIni; i < data.data.Length; i++)
        {
            data.ReadDataTechplot(i);
        }

        float time = 0f;
        float dTime;

        for (int i = iterationIni; i < data.data.Length; i++)
        {
            UpdatePlot(data, i, plotedValue, min, max);
            Debug.Log(i.ToString() + " " + data.data[i].time.ToString());
            dTime = data.data[i].time - time;
            time = data.data[i].time;
            yield return new WaitForSeconds(dTime);
        }
        Debug.Log("Fini!");
    }

    public void UpdatePlot(Data data, int iteration, int plotedValue, float min, float max)
    {

        switch (plotedValue)
        {
            case > 0 when plotedValue == 1:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].u, min, max);
                break;
            case > 0 when plotedValue == 2:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].v, min, max);
                break;
            case > 0 when plotedValue == 2:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].mag, min, max);
                break;
            case > 0 when plotedValue == 3:
                plot.Update2DMeshPlotShader(data, iteration, data.data[iteration].p, min, max);
                break;
        }
    }
}
