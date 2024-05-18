using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;



public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartProcess();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Process process = null;
    StreamWriter messageStream;
    
    void StartProcess()
    {
        try
        {
            process = new Process();
            process.EnableRaisingEvents = false;
            //process.StartInfo.FileName = Application.dataPath + "/home/xaviern/Documents/VSCode/Test";
            process.StartInfo.FileName = "/home/xaviern/Documents/VSCode/Test/a.out";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += new DataReceivedEventHandler( DataReceived );
            process.ErrorDataReceived += new DataReceivedEventHandler( ErrorReceived );
            process.Start();
            process.BeginOutputReadLine();
            messageStream = process.StandardInput;
    
            UnityEngine.Debug.Log( "Successfully launched app" );
        }
        catch( Exception e )
        {
            UnityEngine.Debug.LogError( "Unable to launch app: " + e.Message );
        }
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

