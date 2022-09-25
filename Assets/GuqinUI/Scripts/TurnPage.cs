using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TurnPage : MonoBehaviour
{
    [Header("Page Objects")]
    public GameObject LeftPage;
    public GameObject RightPage;
    public GameObject OverTurnPage;


    [Header("All Page Textures")]
    public List<Sprite> sprites = new List<Sprite>();

    [Header("Interval")]
    public float time = 1;
    
    int PageNum = 0;    //页码
    int PaperNum = 0;   //页数
    float timer;
    float JianGe = 1.1f;

    void Start()
    {
        //关闭左页面物体
        LeftPage.gameObject.SetActive(false);
        //关闭翻页物体
        OverTurnPage.gameObject.SetActive(false);
    }

        public void Update()
    {
    
        
    }



    //关闭书本
    void CloseBook()
    {
        if(timer > Time.time)
        {
            return;
        }
        timer = Time.time + JianGe;

        //打开翻页物体
        OverTurnPage.gameObject.SetActive(true);
        //设置翻页物体的贴图
        OverTurnPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[0].texture);
        OverTurnPage.GetComponent<MeshRenderer>().material.SetTexture("_SecondTex", sprites[1].texture);

        //隐藏左页
        LeftPage.gameObject.SetActive(false);
        //开始翻页
        int rotateVal = 180;

        DOTween.To(() => rotateVal, x => rotateVal = x, 0, 1).OnUpdate(delegate
        {
            OverTurnPage.GetComponent<MeshRenderer>().material.SetInt("_Angle", rotateVal);
        }).OnComplete(delegate
        {
            //翻页结束
            //将右页替换为当前页数0
            RightPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[0].texture);

            //关闭翻页物体
            OverTurnPage.gameObject.SetActive(false);
            PageNum = 0;
        });
    }

    /// <summary>
    /// 翻转书本--正翻
    /// 从第一页翻到第二页
    /// 在书本打开后，显示第一页，需要翻到第二页时，先将翻页物体的第一张和第二张贴图设置成第一页的后半张及第二页的前半张
    /// 在翻页成功后，将书本的左页设置成第二页的前半张，书本的右页设置成第二页的后半张，之后将翻页物体隐藏
    /// </summary>
    public void OverTurnBookEvent()
    {
        if(PaperNum >= sprites.Count / 2 || timer > Time.time)
        {
            return;
        }
        timer = Time.time + JianGe;
        //打开翻页物体
        OverTurnPage.gameObject.SetActive(true);
        //设置翻页物体的贴图
        OverTurnPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[PageNum].texture);
        OverTurnPage.GetComponent<MeshRenderer>().material.SetTexture("_SecondTex", sprites[PageNum + 1].texture);

        //先将右页设置成当前页数+2
        RightPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[PageNum + 2].texture);
        //开始翻页
        int rotateVal = 0;
        
        DOTween.To(() => rotateVal, x => rotateVal = x, 180, 1).OnUpdate(delegate
        {
            OverTurnPage.GetComponent<MeshRenderer>().material.SetInt("_Angle", rotateVal);
        }).OnComplete(delegate
        {
            //翻页结束
            //激活左页
            LeftPage.gameObject.SetActive(true);
            //将左页替换为当前页数+1
            LeftPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[PageNum + 1].texture);

            //关闭翻页物体
            OverTurnPage.gameObject.SetActive(false);
            PageNum += 2;
        });
        PaperNum++;
        Debug.Log("true");
    }

    /// <summary>
    /// 逆翻书效果
    /// </summary>
    void OverTurnBookBackEvent()
    {
        if (PaperNum <= 1||timer > Time.time)
        {
            return;
        }
        timer = Time.time + JianGe;

        //打开翻页物体
        OverTurnPage.gameObject.SetActive(true);
        //设置翻页物体的贴图
        OverTurnPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[PageNum - 2].texture);
        OverTurnPage.GetComponent<MeshRenderer>().material.SetTexture("_SecondTex", sprites[PageNum - 1].texture);

        //先将左页设置成当前页数-2
        LeftPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[PageNum - 3].texture);
        //开始翻页
        int rotateVal = 180;

        DOTween.To(() => rotateVal, x => rotateVal = x, 0, 1).OnUpdate(delegate
        {
            OverTurnPage.GetComponent<MeshRenderer>().material.SetInt("_Angle", rotateVal);
        }).OnComplete(delegate
        {
            //翻页结束
            //将右页替换为当前页数-1
            RightPage.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sprites[PageNum - 2].texture);

            //关闭翻页物体
            OverTurnPage.gameObject.SetActive(false);
            PageNum -= 2;
        });
        PaperNum--;
    }


    public void BackTurnBookEvent()
    {
        if(PaperNum == 1)
            {
                CloseBook();
            }
            else
            {
                OverTurnBookBackEvent();
            }
    }
}
