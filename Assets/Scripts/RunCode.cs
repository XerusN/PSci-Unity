using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;



public class RunCode : MonoBehaviour
{
    MainManager mainManager;

    void Awake()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Process process = null;
    StreamWriter messageStream;
    
    public void StartProcess()
    {
        string oldDirectory = System.IO.Directory.GetCurrentDirectory();

        System.IO.Directory.SetCurrentDirectory(mainManager.cfdCodePath);

        try
        {
            

            process = new Process();
            process.EnableRaisingEvents = false;
            //process.StartInfo.FileName = Application.dataPath + "/home/xaviern/Documents/VSCode/Test";
            process.StartInfo.FileName = "a.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += new DataReceivedEventHandler( DataReceived );
            process.ErrorDataReceived += new DataReceivedEventHandler( ErrorReceived );
            process.Start();
            //process.BeginOutputReadLine();
            //messageStream = process.StandardInput;
            using (StreamReader reader = process.StandardOutput)
            using (StreamReader error = process.StandardError)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    UnityEngine.Debug.Log(line);
                }
                while ((line = error.ReadLine()) != null)
                {
                    UnityEngine.Debug.Log("Error : " + line.ToString());
                }
            }
            process.WaitForExit();
            //UnityEngine.Debug.Log(messageStream.ToString());
            UnityEngine.Debug.Log( "Successfully launched app" );
        }
        catch( Exception e )
        {
            UnityEngine.Debug.LogError( "Unable to launch app: " + e.Message );
        }

        System.IO.Directory.SetCurrentDirectory(oldDirectory);
    }


    void DataReceived( object sender, DataReceivedEventArgs eventArgs )
    {
        // Handle it
    }

    void ErrorReceived( object sender, DataReceivedEventArgs eventArgs )
    {
        UnityEngine.Debug.LogError( eventArgs.Data );
    }


    void OnApplicationQuit()
    {
        if( process != null & !process.HasExited )
        {
            process.Kill();
        }
    }
}

