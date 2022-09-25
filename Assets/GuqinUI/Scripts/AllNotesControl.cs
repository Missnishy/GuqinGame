using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class AllNotesControl : MonoBehaviour
{
    public GameObject GradeLevel;
    public Transform noteObj;           //存储调用的Prefab变量
    [HideInInspector]public bool isOver = false;

    float[] whichNote = new float[]{1, 6, 3, 4, 2, 5, 1, 3, 4, 5, 5, 1, 3, 7};
    int noteMark = 0;            //位置跟踪    
    string timerReset = "y";     //计时器重置
    float xPos;                  //通过x-offset控制Note在哪个徵位出现
    

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            isOver = true;
        }

        //if音乐停止
        if(isOver)
        {
            StopAllCoroutines();
            GradeLevel.SetActive(true);
        }
        else
        {
            if(timerReset == "y")
            {
                StartCoroutine(spawnNote());
                timerReset = "n";
            }
        }

    }

    IEnumerator spawnNote()
    {
        //等待一秒（测试随便写的）
        //后续应该是通过音乐插件编辑事件来控制等待时长
        yield return new WaitForSeconds(1);
        //音符生成位置
        switch (whichNote[noteMark])
        {
            case 1 :
                xPos = -0.207f;
                break;
            case 2 :
                xPos = -0.053f;
                break;
            case 3 :
                xPos = 0.097f;
                break;
            case 4 :
                xPos = 0.251f;
                break;
            case 5 :
                xPos = 0.405f;
                break;
            case 6 :
                xPos = 0.555f;
                break;
            case 7 :
                xPos = 0.709f;
                break;
            default:
                timerReset = "n";
                break;
        }
        if(noteMark < whichNote.Length - 1)
        {
            noteMark++;
            timerReset = "y";
            //实例化音符对象
            Instantiate(noteObj, new Vector3(xPos, 1.567f, 8.665f), noteObj.rotation);
        }
        else
        {
            timerReset = "n";
        }
    }

}
