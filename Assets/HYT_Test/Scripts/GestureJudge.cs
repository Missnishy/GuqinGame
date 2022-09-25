using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ViveHandTracking.Sample
{
    // This script is used to set emission color when right hand pushes the box
    class GestureJudge : MonoBehaviour
    {
        private static Color color = new Color(0.3f, 0, 0, 1);

        private Material material = null;

        public ModelRenderer model;
        private Transform[] Nodes = new Transform[21];
        private Vector3[] pos = new Vector3[21];
        private int count = 1;
        private float[] feature = new float[31];


        private Vector3[] Edge_a = new Vector3[5];
        private Vector3[] Edge_b = new Vector3[5];
        private Vector3[] Edge_c = new Vector3[5];
        private Vector3[] Edge_d = new Vector3[5];
        private float[] angle_ab = new float[5];
        private float[] angle_bc = new float[5];
        private float[] angle_cd = new float[5];


        private float[] length_MCP = new float[4];
        private float[] length_PIP = new float[4];
        private float[] length_DIP = new float[4];
        private float[] length_TIP = new float[4];



        IEnumerator Start()
        {
            while (true)
            {
                while (material == null) yield return null;
                var currentMat = material;
                currentMat.EnableKeyword("_EMISSION");
                currentMat.SetColor("_EmissionColor", color);
                while (material != null) yield return null;
                yield return new WaitForSeconds(0.3f);
                currentMat.DisableKeyword("_EMISSION");
            }

        }

        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.Debug.Log("已启动第" + count + "次识别");
                //UnityEngine.Debug.Log("已启动识别");
                GetFeature();
                Judge(count);
                
                //RunPythonScript(feature);
                count++;
                // for (int n = 0; n < 31; n++)
                // {
                //     UnityEngine.Debug.Log(feature[n]);
                // }
                // string[] arr = { "0.05880171", "0.06094998", "3.09039", "2.131242", "2.106575", "2.196531", "0.8855863", "2.178755", "2.313469", "1.455543", "1.270748", "2.52929", "0.4025039", "1.120593", "2.389663", "21.29013", "4.062404", "3.037387", "3.374247", "22.12964", "7.010205", "3.573874", "5.792621", "16.11626", "16.66375", "4.011983", "9.461636", "21.66237", "34.80245", "4.731051", "18.86982" };
                // RunPythonScript(arr);

            }

        }

        void GetFeature()
        {

            for (int i = 0; i < 21; i++)
            {
                Nodes[i] = model.Nodes[i];
                pos[i] = (Nodes[i].position - Nodes[0].position) * 100;
            }

            for (int j = 0; j < 5; j++)
            {
                Edge_a[j] = pos[2 * (j + 1)] - pos[1 * (j + 1)];
                Edge_b[j] = pos[3 * (j + 1)] - pos[2 * (j + 1)];
                Edge_c[j] = pos[4 * (j + 1)] - pos[3 * (j + 1)];
                Edge_d[j] = pos[1 * (j + 1)] - pos[4 * (j + 1)];
                angle_ab[j] = Mathf.Atan2(Vector3.Dot(Vector3.Normalize(Vector3.Cross(Edge_a[j], Edge_b[j])), Vector3.Cross(Vector3.Normalize(Edge_a[j]), Vector3.Normalize(Edge_b[j]))), Vector3.Dot(Vector3.Normalize(Edge_a[j]), Vector3.Normalize(Edge_b[j])));
                angle_bc[j] = Mathf.Atan2(Vector3.Dot(Vector3.Normalize(Vector3.Cross(Edge_b[j], Edge_c[j])), Vector3.Cross(Vector3.Normalize(Edge_b[j]), Vector3.Normalize(Edge_c[j]))), Vector3.Dot(Vector3.Normalize(Edge_b[j]), Vector3.Normalize(Edge_c[j])));
                angle_cd[j] = Mathf.Atan2(Vector3.Dot(Vector3.Normalize(Vector3.Cross(Edge_c[j], Edge_d[j])), Vector3.Cross(Vector3.Normalize(Edge_c[j]), Vector3.Normalize(Edge_d[j]))), Vector3.Dot(Vector3.Normalize(Edge_c[j]), Vector3.Normalize(Edge_d[j])));
                //text_angle[j] = angle_ab[j].ToString() + "\t" + angle_bc[j].ToString() + "\t" + angle_cd[j].ToString() + "\t";
                feature[3 * j] = angle_ab[j];
                feature[3 * j + 1] = angle_bc[j];
                feature[3 * j + 2] = angle_cd[j];

            }

            for (int m = 0; m < 4; m++)
            {
                length_MCP[m] = (pos[1 + m] - pos[5 + m]).sqrMagnitude;
                length_PIP[m] = (pos[5 + m] - pos[9 + m]).sqrMagnitude;
                length_DIP[m] = (pos[9 + m] - pos[13 + m]).sqrMagnitude;
                length_TIP[m] = (pos[13 + m] - pos[17 + m]).sqrMagnitude;
                //text_length[m] = length_MCP[m].ToString() + "\t" + length_PIP[m].ToString() + "\t" + length_DIP[m].ToString() + "\t" + length_TIP[m].ToString() + "\t";
                feature[15 + 4 * m] = length_MCP[m];
                feature[16 + 4 * m] = length_PIP[m];
                feature[17 + 4 * m] = length_DIP[m];
                feature[18 + 4 * m] = length_TIP[m];
            }

            


        }

        /*
        void RunPythonScript(string[] argvs)
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

        void Get_data(object sender, DataReceivedEventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(eventArgs.Data))
            {
                //UnityEngine.Debug.Log(eventArgs.Data);
                string res = eventArgs.Data;
                Judge(res);
                
            }
        }

        
        void Judge(string num)
        {
            switch (num)
            {
                case "1": UnityEngine.Debug.Log("识别结果为：挑"); break;
                case "2": UnityEngine.Debug.Log("识别结果为：抹"); break;
                case "3": UnityEngine.Debug.Log("识别结果为：打"); break;
                case "4": UnityEngine.Debug.Log("识别结果为：摘"); break;
                case "5": UnityEngine.Debug.Log("识别结果为：剔"); break;
                case "6": UnityEngine.Debug.Log("识别结果为：勾"); break;
                case "7": UnityEngine.Debug.Log("识别结果为：擘"); break;
                case "8": UnityEngine.Debug.Log("识别结果为：托"); break;
                default: UnityEngine.Debug.Log("None"); break;
            }
        }
        */


        void Judge(int num)
        {
            switch (num)
            {
                case 1: 
                {
                    if(Judge_tiao())
                    {
                        UnityEngine.Debug.Log("指法‘挑’识别成功");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("指法‘挑’识别失败");
                    }
                    break;
                }

                case 2: 
                {
                    if(Judge_mo())
                    {
                        UnityEngine.Debug.Log("指法‘抹’识别成功");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("指法‘抹’识别失败");
                    }
                    break;
                }
                case 3: 
                {
                    if(Judge_da())
                    {
                        UnityEngine.Debug.Log("指法‘打’识别成功");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("指法‘打’识别失败");
                    }
                    break;
                }
                case 4:
                {
                    if(Judge_zhai())
                    {
                        UnityEngine.Debug.Log("指法‘摘’识别成功");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("指法‘摘’识别失败");
                    }
                    break;
                }
                case 5: 
                {
                    if(Judge_ti())
                    {
                        UnityEngine.Debug.Log("指法‘剔’识别成功");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("指法‘剔’识别失败");
                    }
                    break;
                }
                case 6: 
                {
                    if(Judge_gou())
                    {
                        UnityEngine.Debug.Log("指法‘勾’识别成功");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("指法‘勾’识别失败");
                    }
                    break;
                }
                case 7: UnityEngine.Debug.Log("识别结果为：擘"); break;
                case 8: UnityEngine.Debug.Log("识别结果为：托"); break;
                default: UnityEngine.Debug.Log("None"); break;
            }
        }

        bool Judge_tiao()
        {
            if(feature[0] >= 0.0f && feature[0] <= 0.5f)
            {
                if(feature[1] >= 0.0f && feature[0] <= 0.5f)
                {
                    if(feature[2] >= 2.7f && feature[0] <= 3.25f)
                    {
                        if(feature[3] >= 1.5f && feature[0] <= 2.35f)
                        {
                            if(feature[4] >= 2.25f && feature[0] <= 3.0f)
                            {
                                if(feature[5] >= 1.5f && feature[0] <= 2.5f)
                                {
                                    if(feature[6] >= 0.8f && feature[0] <= 1.6f)
                                    {
                                        if(feature[7] >= 1.9f && feature[0] <= 2.5f)
                                        {
                                            if(feature[8] >= 2.3f && feature[0] <= 2.75f)
                                            {
                                                return true;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        bool Judge_mo()
        {
            if(feature[0] >= 0.0f && feature[0] <= 0.4f)
            {
                if(feature[1] >= 0.0f && feature[0] <= 0.35f)
                {
                    if(feature[2] >= 2.8f && feature[0] <= 3.3f)
                    {
                        if(feature[3] >= 1.8f && feature[0] <= 2.4f)
                        {
                            if(feature[4] >= 1.3f && feature[0] <= 2.3f)
                            {
                                if(feature[5] >= 1.9f && feature[0] <= 2.5f)
                                {
                                    if(feature[6] >= 0.8f && feature[0] <= 1.4f)
                                    {
                                        if(feature[7] >= 1.8f && feature[0] <= 2.3f)
                                        {
                                            if(feature[8] >= 2.3f && feature[0] <= 2.6f)
                                            {
                                                return true;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        bool Judge_da()
        {
            if(feature[0] >= 0.0f && feature[0] <= 0.5f)
            {
                if(feature[1] >= 0.0f && feature[0] <= 0.4f)
                {
                    if(feature[2] >= 2.8f && feature[0] <= 3.2f)
                    {
                        if(feature[3] >= 1.5f && feature[0] <= 2.25f)
                        {
                            if(feature[4] >= 1.35f && feature[0] <= 2.0f)
                            {
                                if(feature[5] >= 2.2f && feature[0] <= 2.8f)
                                {
                                    if(feature[6] >= 1.2f && feature[0] <= 1.7f)
                                    {
                                        if(feature[7] >= 2.0f && feature[0] <= 2.3f)
                                        {
                                            if(feature[8] >= 2.2f && feature[0] <= 2.7f)
                                            {
                                                return true;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        bool Judge_zhai()
        {
            if(feature[0] >= 0.0f && feature[0] <= 0.3f)
            {
                if(feature[1] >= 0.0f && feature[0] <= 0.3f)
                {
                    if(feature[2] >= 2.9f && feature[0] <= 3.15f)
                    {
                        if(feature[3] >= 1.75f && feature[0] <= 2.25f)
                        {
                            if(feature[4] >= 1.2f && feature[0] <= 2.3f)
                            {
                                if(feature[5] >= 2.1f && feature[0] <= 2.8f)
                                {
                                    if(feature[6] >= 1.0f && feature[0] <= 1.5f)
                                    {
                                        if(feature[7] >= 2.0f && feature[0] <= 2.4f)
                                        {
                                            if(feature[8] >= 2.2f && feature[0] <= 2.65f)
                                            {
                                                return true;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        bool Judge_ti()
        {
            if(feature[0] >= 0.0f && feature[0] <= 0.5f)
            {
                if(feature[1] >= 0.0f && feature[0] <= 0.6f)
                {
                    if(feature[2] >= 2.5f && feature[0] <= 3.2f)
                    {
                        if(feature[3] >= 1.6f && feature[0] <= 2.2f)
                        {
                            if(feature[4] >= 1.4f && feature[0] <= 2.4f)
                            {
                                if(feature[5] >= 2.2f && feature[0] <= 2.7f)
                                {
                                    if(feature[6] >= 1.2f && feature[0] <= 2.0f)
                                    {
                                        if(feature[7] >= 1.3f && feature[0] <= 2.3f)
                                        {
                                            if(feature[8] >= 1.0f && feature[0] <= 2.0f)
                                            {
                                                if(feature[9] >= 2.5f && feature[0] <= 3.0f)
                                                    return true;
                                                else return false;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        bool Judge_gou()
        {
            if(feature[0] >= 0.0f && feature[0] <= 0.3f)
            {
                if(feature[1] >= 0.0f && feature[0] <= 0.3f)
                {
                    if(feature[2] >= 2.9f && feature[0] <= 3.2f)
                    {
                        if(feature[3] >= 1.6f && feature[0] <= 2.2f)
                        {
                            if(feature[4] >= 1.4f && feature[0] <= 2.2f)
                            {
                                if(feature[5] >= 2.5f && feature[0] <= 2.8f)
                                {
                                    if(feature[6] >= 1.3f && feature[0] <= 1.55f)
                                    {
                                        if(feature[7] >= 1.5f && feature[0] <= 2.3f)
                                        {
                                            if(feature[8] >= 1.6f && feature[0] <= 2.9f)
                                            {
                                                if(feature[9] >= 1.5f && feature[0] <= 2.5f)
                                                    return true;
                                                else return false;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

    }
}

