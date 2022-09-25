using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using SonicBloom.Koreo;
using ViveHandTracking;

/// <summary>
/// 通过比较四个手指中哪个手指弯曲程度最强来判定手势（特指单手指手势）
/// </summary>
public class GestureJudgeMin : MonoBehaviour
{
    //-----------------------------------结构体-----------------------------------

    //Single_Hand结构体：记录模型中各个keypoint的三维坐标
    public struct Hand
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Vector3[] pos;
    }
    
    //Hand_Pos结构体: 四个手指共具有3×4个keypoints，记录三角形中间夹角的两条临边向量ba/bc
    public struct Hand_Vector
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Vector3[] thumb_pos;    //thumb keypoints: 2/3/4

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Vector3[] pinky_pos;    //pinky keypoints: 5/6/8

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Vector3[] middle_pos;   //middle keypoints: 9/10/12
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Vector3[] ring_pos;     //ring keypoints: 13/14/16
    }
    
    //-----------------------------------成员变量-----------------------------------
    
    private static Color color = new Color(0.3f, 0, 0, 1);
    private Material material = null;
    
    public ModelRenderer model;
    private Hand hand;
    private Hand_Vector hand_Feature;
    public string eventID_01;               //Koreography EventID 01 右手-挑 / 左手-拇指按弦
    public string eventID_02;               //Koreography EventID 02 右手-抹 / 左手-无名指按弦

    //测试先用public从外部输入JudgeID，实际应通过游戏UI进行控制选择
    //用不用static存疑，应该还是要用，得从别的脚本抓取JudgeID数值
    public int JudgeID;                    //Gesture JudgeID: 0-右手-挑; 1-右手-抹; 2-左手-拇指按弦; 3-左手-无名指按弦

    private bool isTrue = false;            //识别完成flag
    private bool isGetPlayOnce = true;      //Music Control
    private KoreographyEvent currentEvent;

    //feature[fingerID] = angle
    //fingerID: 0-thumb; 1-pinky; 2-middle; 3-ring;
    private float[] feature = new float[4];


    //-----------------------------------主功能函数-----------------------------------

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

            //结构体内数组初始化
            hand.pos = new Vector3[21];
                
        }
            
    }

    void Update()
    {

        if(AudioManager.getIsPlayValue() == true && isGetPlayOnce == true)
        {
            Koreographer.Instance.RegisterForEvents(GetEventID(), Judge);
            isGetPlayOnce = false;
        }
        if(AudioManager.getIsPlayValue() == false)
        {
            isGetPlayOnce = true;
        }
            
    }

    //-----------------------------------成员函数-----------------------------------

    /// <summary>
    /// 通过JudgeID匹配相应的EventID
    /// </summary>
    /// <returns>可调用Koreography的EventID</returns>
    private string GetEventID()
    {
        if(JudgeID == 0 || JudgeID == 2)
            return eventID_01;
        else if(JudgeID == 1 || JudgeID == 3)
            return eventID_02;
        else
            return null;
    }

    /// <summary>
    /// 获取所有keypoints的三维坐标
    /// </summary>
    private void GetPosition()
    {
        //获取一个手模型21个keypoints的坐标
        for (int i = 0; i < 21; i++)
        {
            hand.pos[i] = model.Nodes[i].position * 100;
        }

        //给Hand_Feature赋值
        hand_Feature.thumb_pos = new Vector3[2]{hand.pos[3] - hand.pos[2], hand.pos[3] - hand.pos[4]};
        hand_Feature.pinky_pos = new Vector3[2]{hand.pos[6] - hand.pos[5], hand.pos[6] - hand.pos[8]};
        hand_Feature.middle_pos = new Vector3[2]{hand.pos[10] - hand.pos[9], hand.pos[10] - hand.pos[12]};
        hand_Feature.ring_pos = new Vector3[2]{hand.pos[14] - hand.pos[13], hand.pos[14] - hand.pos[16]};
    }

    /// <summary>
    /// 三维空间中，求两向量夹角
    /// </summary>
    /// <param name="dirA">向量ba</param>
    /// <param name="dirB">向量bc</param>
    /// <returns>AngleB</returns>
    private float GetAngle(Vector3 dirA, Vector3 dirB)
    {
        //https://www.cnblogs.com/dsh20134584/p/7691101.html

        //使向量处于同一个平面，这里平面为XZ
        //注:Vector3.Project计算向量在指定轴上的投影，向量本身减去此投影向量就为在平面上的向量
        dirA = dirA - Vector3.Project(dirA, Vector3.up);
        dirB = dirB - Vector3.Project(dirB, Vector3.up);

        //计算角度
        float angle = Vector3.Angle(dirA, dirB);

        return angle;
    }

    /// <summary>
    /// 通过比较四个手指的不同夹角值，得出四个手指中弯曲程度最高的那一个
    /// </summary>
    /// <returns>fingerID</returns>
    private int FeatureAngleCompare()
    {
        GetPosition();

        //Angle赋值
        feature[0] = GetAngle(hand_Feature.thumb_pos[0], hand_Feature.thumb_pos[1]);
        feature[1] = GetAngle(hand_Feature.pinky_pos[0], hand_Feature.pinky_pos[1]);
        feature[2] = GetAngle(hand_Feature.middle_pos[0], hand_Feature.middle_pos[1]);
        feature[3] = GetAngle(hand_Feature.ring_pos[0], hand_Feature.ring_pos[1]);

        float minAngle = 360;
        int fingerID = 200;

        //得到Angle最小值的fingerID
        for (int i = 0; i < feature.Length; i++)
        {
            if (minAngle > feature[i])
            {
                minAngle = feature[i];
                fingerID = i;
            }
        }
        return fingerID;
    }

    /// <summary>
    /// 手势判定结果呈现，后期可结合UI优化视觉
    /// </summary>
    private void JudgeResult()
    {
        if(FeatureAngleCompare() == JudgeID)
        {
            UnityEngine.Debug.Log(GetEventID() + "Recognized Successfully");
            isTrue = false;
        }
        else
        {
            UnityEngine.Debug.Log(GetEventID() + "Recognized Failed");
        }
    }

    /// <summary>
    /// 手势主判定，通过插件中手势出现点的关键值与JudgeID的匹配情况来判定识别成功与否
    /// </summary>
    /// <param name="koreographyEvent">音乐插件中自定义的时间点</param>
    private void Judge(KoreographyEvent koreographyEvent)
    //private void JudgeTiao(KoreographyEvent koreographyEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if(koreographyEvent.HasIntPayload() && koreographyEvent.GetIntValue() == JudgeID)
        {   
            FeatureAngleCompare();
            JudgeResult();
        }
    }


}
