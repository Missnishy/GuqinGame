using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo;
using ViveHandTracking;

/// <summary>
/// 
/// </summary>
public class TextTest : MonoBehaviour
{
    public string eventID;
    private Text text;
    private bool isGetPlayOnce = true;
    private KoreographyEvent currentEvent;

    void Start() 
    {
        text = GetComponent<Text>();
    }

    void Update()
    {

        if(AudioManager.getIsPlayValue() == true && isGetPlayOnce == true)
        {
            Koreographer.Instance.RegisterForEventsWithTime(eventID, UpdateText);
            isGetPlayOnce = false;
        }
        if(AudioManager.getIsPlayValue() == false)
        {
            isGetPlayOnce = true;
        }
            
    }

    private void UpdateText(KoreographyEvent koreographyEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        //判断当前事件是否有文本负荷
        if(koreographyEvent.HasTextPayload())
        {
            //更新文本
            //    本事件为起始事件   || （          本事件 != 上一个事件    &&                本事件开始时间 > 上一个时间开始时间       ）
            if(currentEvent == null || (koreographyEvent != currentEvent && koreographyEvent.StartSample > currentEvent.StartSample))
            {
                
                text.text = koreographyEvent.GetTextValue();
                currentEvent = koreographyEvent;
            }

            //事件结束清空文本
            //   上一个事件时间结束时间 < 指针时间
            if(currentEvent.EndSample < sampleTime)
            {
                text.text = string.Empty;
                currentEvent = null;
            }
        }
    }

}
