using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using ViveHandTracking;


    // This script is used to set emission color when right hand pushes the box
    class GestureTest : MonoBehaviour
    {
        private static Color color = new Color(0.3f, 0, 0, 1);

        private Material material = null;

        public ModelRenderer model;
        private Transform[] Nodes = new Transform[21];
        private Vector3[] pos = new Vector3[21];
        private float[] feature = new float[15];


        private Vector3[] Edge_a = new Vector3[5];
        private Vector3[] Edge_b = new Vector3[5];
        private Vector3[] Edge_c = new Vector3[5];
        private Vector3[] Edge_d = new Vector3[5];
        private float[] angle_ab = new float[5];
        private float[] angle_bc = new float[5];
        private float[] angle_cd = new float[5];


        private bool isTrue = false;



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
            if(Input.GetMouseButtonDown(1))
            {
                GetFeature();
                Judge_thumb();
                if(isTrue)
                {
                    UnityEngine.Debug.Log("左手‘拇指按弦’识别成功");
                }   
                else
                {
                    UnityEngine.Debug.Log("左手‘拇指按弦’识别失败");
                }
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
                
                feature[3 * j] = angle_ab[j];
                feature[3 * j + 1] = angle_bc[j];
                feature[3 * j + 2] = angle_cd[j];

            }


            


        }

        void Judge_thumb()
        {
            if(feature[0] >= 0.25f && feature[0] <= 0.7f)
            {
                if(feature[1] >= 0.5f && feature[0] <= 2.0f)
                {
                    if(feature[2] >= 2.0f && feature[0] <= 2.8f)
                    {
                        if(feature[3] >= 1.1f && feature[0] <= 1.9f)
                        {
                            if(feature[4] >= 0.4f && feature[0] <= 1.65f)
                            {
                                if(feature[5] >= 2.5f && feature[0] <= 3.0f)
                                {
                                    if(feature[6] >= 1.5f && feature[0] <= 1.9f)
                                    {
                                        if(feature[7] >= 2.0f && feature[0] <= 2.3f)
                                        {
                                            if(feature[8] >= 2.3f && feature[0] <= 2.8f)
                                            {
                                                isTrue =  true;
                                            }
                                            else isTrue = false;
                                        }
                                        else isTrue = false;
                                    }
                                    else isTrue = false;
                                }
                                else isTrue = false;
                            }
                            else isTrue = false;
                        }
                        else isTrue = false;
                    }
                    else isTrue = false;
                }
                else isTrue = false;
            }
            else
            {
                isTrue = false;
            }
        }




    }


