using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;



public class RunCode : MonoBehaviour
{
    MainManager mainManager;

    string oldDirectory;

    public float t;
    public float dt;
    public GameObject runUI;

    void Awake()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Process process = null;
    private Thread outputThread;
    public Boolean cfdCodeRunning = false;



    public void StartProcess()
    {
        runUI.SetActive(true);
        cfdCodeRunning = true;

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
            else if (line.Substring(0, 5) == "temps")
            {
                process.Kill();
                cfdCodeRunning = false;
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

