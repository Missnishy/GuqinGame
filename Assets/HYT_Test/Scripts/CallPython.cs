using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// 
/// </summary>
public class CallPython : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string[] arr = { "0.05880171", "0.06094998", "3.09039", "2.131242", "2.106575", "2.196531", "0.8855863", "2.178755", "2.313469", "1.455543", "1.270748", "2.52929", "0.4025039", "1.120593", "2.389663", "21.29013", "4.062404", "3.037387", "3.374247", "22.12964", "7.010205", "3.573874", "5.792621", "16.11626", "16.66375", "4.011983", "9.461636", "21.66237", "34.80245", "4.731051", "18.86982" };
            RunPythonScript(arr);
        }
    }
    private static void RunPythonScript(string[] argvs)
    {
        Process p = new Process();
        string path = @"F:\Sources\download\GBDT_Simple_Tutorial-master\GBDT\rf_predict.py";
        foreach (string temp in argvs)
        {
            path += " " + temp;
        }
        p.StartInfo.FileName = @"C:\Users\user\Anaconda3\python.exe";

        p.StartInfo.UseShellExecute = false;
        p.StartInfo.Arguments = path;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.CreateNoWindow = true;

        p.Start();
        p.BeginOutputReadLine();
        p.OutputDataReceived += new DataReceivedEventHandler(Get_data);
        p.WaitForExit();
    }
    private static void Get_data(object sender, DataReceivedEventArgs eventArgs)
    {
        if (!string.IsNullOrEmpty(eventArgs.Data))
        {
            //UnityEngine.Debug.Log(eventArgs.Data);
            string res = eventArgs.Data;
            Judge(res);
        }
    }
    private static void Judge(string num)
    {
        switch(num)
        {
            case "1" : print("挑"); break;
            case "2" : print("抹"); break;
            case "3" : print("打"); break;
            case "4" : print("摘"); break;
            case "5" : print("剔"); break;
            case "6" : print("勾"); break;
            case "7" : print("擘"); break;
            case "8" : print("托"); break;
            default : print("None"); break;
        }
    }
}