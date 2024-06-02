using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    MainManager mainManager;
    VisualizerManager visualizerManager;
    public GameObject mainMenu;
    public GameObject visualizerUI;
    public GameObject inputMenu;

    public GameObject minInput;
    public GameObject maxInput;
    public GameObject iterationInput;
    public GameObject timeText;

    public GameObject simulationManager;
    public CustomInput input;

    public GameObject runUI;
    public GameObject runTime;
    public GameObject runDtime;
    public GameObject runSlider;

    private Boolean isSelected;

    // Start is called before the first frame update
    void Awake()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        visualizerManager = GameObject.Find("Visualizer").GetComponent<VisualizerManager>();
    }

    private void Update()
    {
        if (visualizerUI.activeSelf)
        {
            if (visualizerManager == null) {
                Debug.Log("no visualizer found");
            }
            if (!isSelected)
            {
                minInput.GetComponent<TMP_InputField>().text = visualizerManager.valueMin.ToString();
                maxInput.GetComponent<TMP_InputField>().text = visualizerManager.valueMax.ToString();
                iterationInput.GetComponent<TMP_InputField>().text = visualizerManager.currentIteration.ToString();
            }
            if (visualizerManager.data.data.Length == 0) {
                Debug.Log("no data");
            }
            timeText.GetComponent<TMP_Text>().text = "Time = " + visualizerManager.data.data[visualizerManager.currentIteration].time.ToString() + " s";
        }
        if (runUI.activeSelf && simulationManager.GetComponent<RunCode>().cfdCodeRunning)
        {
            runTime.GetComponent<TMP_Text>().text = "t = " + simulationManager.GetComponent<RunCode>().t.ToString() + " s";
            runDtime.GetComponent<TMP_Text>().text = "dt = " + simulationManager.GetComponent<RunCode>().dt.ToString() + " s";
            runSlider.GetComponent<Slider>().value = simulationManager.GetComponent<RunCode>().t / input.tf;
        }
    }

    public void UpdatePath(string path)
    {
        mainManager.cfdCodePath = path.Trim();
    }

    public void StartPlot()
    {
        mainMenu.SetActive(false);
        visualizerManager.InitVisualizer();
        visualizerUI.SetActive(true);
    }

    public void UpdateIteration(String valueStr)
    {
        visualizerManager.StopAllCoroutines();
        int value;
        if (int.TryParse(iterationInput.GetComponent<TMP_InputField>().text, out value))
        {
            if (value >= 0 && value < visualizerManager.data.data.Length)
            {
                visualizerManager.currentIteration = value;
                visualizerManager.UpdatePlot(visualizerManager.currentIteration, visualizerManager.currentPlotedValue, visualizerManager.valueMin, visualizerManager.valueMax);
            }
        }
    }

    public void UpdateMax(String valueStr)
    {
        visualizerManager.StopAllCoroutines();
        float value;
        if (float.TryParse(maxInput.GetComponent<TMP_InputField>().text, out value))
        {
            if (value > visualizerManager.valueMin)
            {
                visualizerManager.valueMax = value;
                visualizerManager.UpdatePlot(visualizerManager.currentIteration, visualizerManager.currentPlotedValue, visualizerManager.valueMin, visualizerManager.valueMax);
            }
        }
    }

    public void UpdateMin(String valueStr)
    {
        visualizerManager.StopAllCoroutines();
        float value;
        if (float.TryParse(minInput.GetComponent<TMP_InputField>().text, out value))
        {
            if (value < visualizerManager.valueMax)
            {
                visualizerManager.valueMin = value;
                visualizerManager.UpdatePlot(visualizerManager.currentIteration, visualizerManager.currentPlotedValue, visualizerManager.valueMin, visualizerManager.valueMax);
            }
        }
    }

    public void InputSelected()
    {
        isSelected = true;
    }

    public void InputNotSelected()
    {
        isSelected = false;
    }

    public void InputButton()
    {
        if (!runUI.activeSelf && simulationManager.GetComponent<RunCode>().cfdCodeRunning)
        {
            mainMenu.SetActive(false);
            inputMenu.SetActive(true);
        }
    }

    public void BackInputButton()
    {
        mainMenu.SetActive(true);
        inputMenu.SetActive(false);
        simulationManager.GetComponent<CreateInput>().WriteInputFile();
    }

    public void BackVisualizerButton()
    {
        visualizerManager.StopAllCoroutines();
        visualizerUI.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
