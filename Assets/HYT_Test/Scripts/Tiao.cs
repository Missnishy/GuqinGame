using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using SonicBloom.Koreo;
using ViveHandTracking;


    // This script is used to set emission color when right hand pushes the box
    class Tiao : MonoBehaviour
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


        public string eventID;
        private bool isTrue = false;
        private bool isGetPlayOnce = true;
        private KoreographyEvent currentEvent;



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
            //Koreographer.Instance.RegisterForEventsWithTime(eventID, JudgeTiao);
            
        }


        void Update()
        {
            if(AudioManager.getIsPlayValue() == true && isGetPlayOnce == true)
            {
                Koreographer.Instance.RegisterForEvents(eventID, JudgeTiao);
                isGetPlayOnce = false;
            }
            if(AudioManager.getIsPlayValue() == false)
            {
                isGetPlayOnce = true;
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


            


        }


        void Judge_tiao()
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
                                                isTrue = true;
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

        private void JudgeTiao(KoreographyEvent koreographyEvent)
        //private void JudgeTiao(KoreographyEvent koreographyEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
        {
            if(koreographyEvent.HasIntPayload() && koreographyEvent.GetIntValue() == 2)
            {   
                GetFeature();
                Judge_tiao();
                if(isTrue)
                {
                    UnityEngine.Debug.Log("指法‘挑’识别成功");
                }
                else
                {
                    UnityEngine.Debug.Log("指法‘挑’识别失败");
                }
            }
        }


    }


