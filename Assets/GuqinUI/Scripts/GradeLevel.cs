using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GradeLevel : MonoBehaviour
{
    public GameObject gradeLevel;
    public GameObject MusicControl;


    void Update()
    {
        //后面改 if音乐结束
        if(MusicControl.GetComponent<AllNotesControl>().isOver)
        {
            //得分计算function;
            gradeLevel.gameObject.SetActive(true);
            MusicControl.GetComponent<AllNotesControl>().isOver = false;
        }
        else
        {
            gradeLevel.gameObject.SetActive(false);
        }
    }

}
