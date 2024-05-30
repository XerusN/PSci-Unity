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

    private Boolean isSelected;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        visualizerManager = GameObject.Find("Visualizer").GetComponent<VisualizerManager>();
    }

    private void Update()
    {
        if (visualizerUI.activeSelf)
        {
            if (!isSelected)
            {
                minInput.GetComponent<TMP_InputField>().text = visualizerManager.valueMin.ToString();
                maxInput.GetComponent<TMP_InputField>().text = visualizerManager.valueMax.ToString();
                iterationInput.GetComponent<TMP_InputField>().text = visualizerManager.currentIteration.ToString();
            }
            timeText.GetComponent<TMP_Text>().text = "Time = " + visualizerManager.data.data[visualizerManager.currentIteration].time.ToString() + " s";
        }
    }

    public void UpdatePath(string path)
    {
        mainManager.cfdCodePath = path;
    }

    public void StartPlot()
    {
        mainMenu.SetActive(false);
        visualizerUI.SetActive(true);
        visualizerManager.InitVisualizer();
        //StartCoroutine(visualizerManager.RunAnimatedPlot(0, 1, -1f, 0.5f));
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
        mainMenu.SetActive(false);
        inputMenu.SetActive(true);
    }

    public void BackInputButton()
    {
        mainMenu.SetActive(true);
        inputMenu.SetActive(false);
        simulationManager.GetComponent<CreateInput>().WriteInputFile();
    }
}
