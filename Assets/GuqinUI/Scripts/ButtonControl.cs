/* 
 *  Author : Missnish
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// 
/// </summary>
public class ButtonControl : MonoBehaviour
{
    //--------------成员变量 public--------------

    //--------------成员变量 private--------------
    Button btn;
    //Outline outline;


    //--------------Unity主控函数--------------
    void Start()
    {
        btn = GetComponent<Button>();
        //outline = GetComponent<Outline>();
    }

    void Update()
    {
        
    }

    //--------------自定义成员函数--------------
    private void OnCollisionEnter(Collision other)
    {
        /*
        btn.onClick.AddListener(delegate() {
				this.OnClick(buttonObj); 
			});
        Debug.Log("true");
        */

        //碰撞发生触发Onclick事件
        if (other.gameObject.CompareTag("Index"))
        {
            ExecuteEvents.Execute(btn.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler); 
            //Debug.Log("true");
            //outline.enabled = true;
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Index"))
        {
            //outline.enabled = false;
        }
        
    }

    /*
    public void OnClick(GameObject btnObj)
    {
        if(btnObj.CompareTag("SceneLoader"))
        {
            GameLoader scene = btnObj.GetComponent<GameLoader>();
            scene.SwtichScene(SceneID);
        }
        
    }
    */

}
