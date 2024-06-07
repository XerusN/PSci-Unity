using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Linq;



public class RunCode : MonoBehaviour
{
    MainManager mainManager;

    string oldDirectory;

    public float t;
    public float dt;
    public GameObject runUI;
    public GameObject stopButton;
    public List<int> iterations;
    public List<float> integrals;

    void Awake()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (process != null)
        {
            if (process.HasExited)
            {
                cfdCodeRunning = false;
            }
        }
    }
    
    Process process = null;
    private Thread outputThread;
    public Boolean cfdCodeRunning = false;
    public CustomInput input;




    public void StartProcess()
    {
        runUI.SetActive(true);
        stopButton.SetActive(true);

        cfdCodeRunning = true;

        iterations = new List<int>();
        integrals = new List<float>();

        try
        {
            oldDirectory = System.IO.Directory.GetCurrentDirectory();

            System.IO.Directory.SetCurrentDirectory(mainManager.cfdCodePath);

            process = new Process();
            process.EnableRaisingEvents = false;
            //process.StartInfo.FileName = Application.dataPath + "/home/xaviern/Documents/VSCode/Test";

#if UNITY_STANDALONE_WIN
            process.StartInfo.FileName = "a.exe";
#elif UNITY_EDITOR_WIN
            process.StartInfo.FileName = "a.exe";
#elif UNITY_EDITOR_LINUX
            process.StartInfo.FileName = "a.out";
#else
            process.StartInfo.FileName = "a.out";
#endif

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            System.IO.Directory.SetCurrentDirectory(oldDirectory);

            outputThread = new Thread(ReadOutput);
            outputThread.Start();

            //StartCoroutine(cfdCodeExit());

            ////process.BeginOutputReadLine();
            ////messageStream = process.StandardInput;
            //using (StreamReader reader = process.StandardOutput)
            //using (StreamReader error = process.StandardError)
            //{
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        UnityEngine.Debug.Log(line);
            //    }
            //    while ((line = error.ReadLine()) != null)
            //    {
            //        UnityEngine.Debug.Log("Error : " + line.ToString());
            //    }
            //}
            //process.WaitForExit();
            ////UnityEngine.Debug.Log(messageStream.ToString());
        }
        catch ( Exception e )
        {
            UnityEngine.Debug.LogError( "Unable to launch app: " + e.Message );
        }

    }

    void ReadOutput()
    {
        StreamReader reader = process.StandardOutput;
        while(!reader.EndOfStream && !process.HasExited)
        {
            string line = reader.ReadLine().Trim();
            //UnityEngine.Debug.Log(line);
            if (line[0] == 't' && line[2] == '=')
            {
                float.TryParse(line[3..], out t);
            }
            else if (line[0] == 'd')
            {
                float.TryParse(line[4..], out dt);
            }
            else if (line.Substring(0, 5) == "temps" | process.HasExited)
            {
                process.Kill();
                cfdCodeRunning = false;
            }
            else if (line[0] != '-')
            {
                int index1 = line.IndexOf(':');
                if (index1 < 0) { continue; }
                int index2 = line.IndexOf("i", index1);
                if (index2 < 0) { continue; }
                int iteration;
                int.TryParse(line[(index1+1)..(index2-1)], out iteration);
                iterations.Add(iteration);

                index1 = line.IndexOf('=');
                if (index1 < 0) { continue; }
                float integral;
                float.TryParse(line[(index1+1)..], out integral);
                integrals.Add(integral);
            }
        }

        StreamReader errorReader = process.StandardError;
        while (!errorReader.EndOfStream && !process.HasExited)
        {
            UnityEngine.Debug.Log(errorReader.ReadLine());
        }

    }

    void OnApplicationQuit()
    {
        if( process != null && !process.HasExited )
        {
            process.Kill();
        }
    }

    public void StopProcess()
    {
        process.Kill();
        cfdCodeRunning = false;
    }

}

