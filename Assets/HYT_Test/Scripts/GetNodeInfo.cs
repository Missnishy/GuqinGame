using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  //操作文件夹时需引用该命名空间
using System.Text;

namespace ViveHandTracking.Sample {

  // This script is used to set emission color when right hand pushes the box
  class GetNodeInfo : MonoBehaviour {
    private static Color color = new Color(0.3f, 0, 0, 1);

    private Material material = null;

    public ModelRenderer model;
    private Transform[] Nodes = new Transform[21];
    private string[] text_ori = new string[21];
    private string[] text_angle = new string[5];
    private string[] text_length = new string[4];
    private string[] text_data = new string[9];
    private Vector3[] pos = new Vector3[21];

    private int count = 1;
    private int count_data = 1;

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


    IEnumerator Start() {
      while (true) {
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

      // for(int i=0; i<21; i++)
      // {
      //   Nodes[i] = model.Nodes[i];
      // }
      // if (Input.GetKeyDown(key))
      // {
      //   //AddTxtText("adding tex is active");
      //   Debug.Log("test is ok");
      // }
      if(Input.GetMouseButtonDown(1))
      { 
        // Transform[] Nodes = new Transform[21];
        // string[] text = new string[21];
        // Vector3[] pos = new Vector3[21];

          for(int i = 0; i < 21; i++)
          {
            Nodes[i] = model.Nodes[i];
            pos[i] = (Nodes[i].position - Nodes[0].position) * 100;
            //Debug.Log(Nodes[i].name + "\t" + pos[i] + "\n");
            //text[i] = i.ToString();
            text_ori[i] = pos[i].ToString() + "\t";

          }

            for (int j = 0; j < 5; j++)
            {
                    Edge_a[j] = pos[2 * (j + 1)] - pos[1 * (j + 1)];
                    Edge_b[j] = pos[3 * (j + 1)] - pos[2 * (j + 1)];
                    Edge_c[j] = pos[4 * (j + 1)] - pos[3 * (j + 1)];
                    Edge_d[j] = pos[1 * (j + 1)] - pos[4 * (j + 1)];
                    // Edge_a[j] -= Vector3.Project(Edge_a[j], Vector3.up);
                    // Edge_b[j] -= Vector3.Project(Edge_b[j], Vector3.up);
                    // Edge_c[j] -= Vector3.Project(Edge_c[j], Vector3.up);
                    // Edge_d[j] -= Vector3.Project(Edge_d[j], Vector3.up);
                    // angle_ab[j] = 180 - Vector3.Angle(Edge_a[j], Edge_b[j]);
                    // angle_bc[j] = 180 - Vector3.Angle(Edge_b[j], Edge_c[j]);
                    // angle_cd[j] = Vector3.Angle(Edge_c[j], Edge_d[j]);
                    angle_ab[j] = Mathf.Atan2(Vector3.Dot(Vector3.Normalize(Vector3.Cross(Edge_a[j], Edge_b[j])), Vector3.Cross(Vector3.Normalize(Edge_a[j]), Vector3.Normalize(Edge_b[j]))), Vector3.Dot(Vector3.Normalize(Edge_a[j]), Vector3.Normalize(Edge_b[j])));
                    angle_bc[j] = Mathf.Atan2(Vector3.Dot(Vector3.Normalize(Vector3.Cross(Edge_b[j], Edge_c[j])), Vector3.Cross(Vector3.Normalize(Edge_b[j]), Vector3.Normalize(Edge_c[j]))), Vector3.Dot(Vector3.Normalize(Edge_b[j]), Vector3.Normalize(Edge_c[j])));
                    angle_cd[j] = Mathf.Atan2(Vector3.Dot(Vector3.Normalize(Vector3.Cross(Edge_c[j], Edge_d[j])), Vector3.Cross(Vector3.Normalize(Edge_c[j]), Vector3.Normalize(Edge_d[j]))), Vector3.Dot(Vector3.Normalize(Edge_c[j]), Vector3.Normalize(Edge_d[j])));
                    text_angle[j] = angle_ab[j].ToString() + "\t" + angle_bc[j].ToString() + "\t" + angle_cd[j].ToString() + "\t";
                    
            }

                for (int m = 0; m < 4; m++)
                {
                    length_MCP[m] = (pos[1 + m] - pos[5 + m]).sqrMagnitude;
                    length_PIP[m] = (pos[5 + m] - pos[9 + m]).sqrMagnitude;
                    length_DIP[m] = (pos[9 + m] - pos[13 + m]).sqrMagnitude;
                    length_TIP[m] = (pos[13 + m] - pos[17 + m]).sqrMagnitude;
                    text_length[m] = length_MCP[m].ToString() + "\t" + length_PIP[m].ToString() + "\t" + length_DIP[m].ToString() + "\t" + length_TIP[m].ToString() + "\t";
                }

                Array.Copy(text_angle, 0, text_data, 0, text_angle.Length);
                Array.Copy(text_length, 0, text_data, text_angle.Length, text_length.Length);


                AddTxtText_ori(text_ori);
                AddTxtText_data(text_data);


                // text = null;
                // pos = null;
                // Nodes = null;
            }



    }


      void AddTxtText_ori(string[] txtText)//Transform[] Nodes
    {
        string path = "D:/Unity/Thumb_left_info.txt";
        StreamWriter sw;
        FileInfo fi = new FileInfo(path);

        if (!File.Exists(path))
        {
            sw = fi.CreateText();
        }
        else {
            sw = fi.AppendText();   //在原文件后面追加内容      
        }

        foreach (var child in txtText)
          sw.Write(child);
        sw.WriteLine();

        Debug.Log("文本ori成功输出第" + count++ +"次");       
        sw.Close();
        sw.Dispose();
    }

          void AddTxtText_data(string[] txtText)//Transform[] Nodes
    {
        string path = "D:/Unity/Thumb_left_info_feature.txt";
        StreamWriter sw;
        FileInfo fi = new FileInfo(path);

        if (!File.Exists(path))
        {
            sw = fi.CreateText();
        }
        else {
            sw = fi.AppendText();   //在原文件后面追加内容      
        }

        foreach (var child in txtText)
          sw.Write(child);
        sw.WriteLine();

        Debug.Log("文本data成功输出第" + count_data++ +"次");       
        sw.Close();
        sw.Dispose();
    }

    // void OnTriggerEnter(Collider other) {
    //   if (other.gameObject.name.StartsWith("Cube"))
    //     material = other.transform.GetComponent<Renderer>().material;
    //   other.transform.GetComponent<Renderer>().material.color = Color.red;

      
    // }

    // void OnTriggerStay(Collider other) {
    //   if (other.gameObject.name.StartsWith("Cube"))
    //    {
    //       material = other.transform.GetComponent<Renderer>().material;
    //       other.transform.GetComponent<Renderer>().material.color = Color.blue;
    //   }
    //   for(int i=0; i<21; i++)
    //   {
    //     Debug.Log(i + " " + pos[i]+"\t");
    //   }
    // }

    // void OnTriggerExit(Collider other) {
    //   if (other.gameObject.name.StartsWith("Cube")) 
    //     material = null;
    //   other.transform.GetComponent<Renderer>().material.color = Color.white;
    // }
  }

}
