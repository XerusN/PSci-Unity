using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    MainManager mainManager;
    VisualizerManager visualizerManager;
    GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        visualizerManager = GameObject.Find("Visualizer").GetComponent<VisualizerManager>();
        mainMenu = GameObject.Find("Main Menu");
    }

    public void UpdatePath(string path)
    {
        mainManager.cfdCodePath = path;
    }

    public void StartPlot()
    {
        mainMenu.SetActive(false);
        StartCoroutine(visualizerManager.RunAnimatedPlot(0, 250, 1, -1f, 1f));
    }
}
