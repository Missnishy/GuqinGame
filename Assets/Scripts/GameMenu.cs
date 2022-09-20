using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏菜单
/// 点击古琴 → 弹出UI面板
/// </summary>
public class GameMenu : MonoBehaviour
{
    public CanvasGroup menu;
    public Camera targetCamera;
    public AnimationCurve showCurve;    //显示动画曲线
    public AnimationCurve hideCurve;    //隐藏动画曲线
    public float animationSpeed;    //动画播放速率

    IEnumerator showMenu()
    {
        float timer = 0;
        //当面板的不透明度小于1时进入循环
        while(menu.alpha < 1)
        {
            //根据动画曲线动态更新透明度
            menu.alpha = showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;

            yield return null;
        }
    }

    IEnumerator hideMenu()
    {
        float timer = 0;
        //当面板的不透明度大于0时进入循环
        while(menu.alpha > 0)
        {
            //根据动画曲线动态更新透明度
            menu.alpha = hideCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;

            yield return null;
        }
    }

    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        if(targetCamera.GetComponent<ColliderEvent>().isCollided)
        {
            //停止所有协程
            StopAllCoroutines();
            //运行显示Menu协程
            StartCoroutine(showMenu());
            targetCamera.GetComponent<ColliderEvent>().isCollided = false;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            //运行隐藏Menu协程
            StartCoroutine(hideMenu());
        }
    }
}
