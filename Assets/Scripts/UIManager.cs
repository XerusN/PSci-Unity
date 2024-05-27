using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    MainManager mainManager;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    public void UpdatePath(string path)
    {
        mainManager.cfdCodePath = path;
    }
}
