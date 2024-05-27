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


    public void StartProcess()
    {


        try
        {
            oldDirectory = System.IO.Directory.GetCurrentDirectory();

            System.IO.Directory.SetCurrentDirectory(mainManager.cfdCodePath);

            process = new Process();
            process.EnableRaisingEvents = false;
            //process.StartInfo.FileName = Application.dataPath + "/home/xaviern/Documents/VSCode/Test";
#if Windows
            process.StartInfo.FileName = "a.exe";
#elif UNITY_EDITOR_WIN
            process.StartInfo.FileName = "a.exe";
#elif Linux
            process.StartInfo.FileName = "a.out";
#elif UNITY_EDITOR_LINUX
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
            UnityEngine.Debug.Log(reader.ReadLine());
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
}

