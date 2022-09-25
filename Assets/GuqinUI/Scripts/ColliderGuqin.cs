/* 
 *  Author : Missnish
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ColliderGuqin : MonoBehaviour
{
    //--------------成员变量 public--------------
    [HideInInspector]public bool isCollided = false;
    public GameObject menu;


    //--------------成员变量 private--------------


    //--------------Unity主控函数--------------
    void Start()
    {
        
    }

    void Update()
    {
        if(menu.gameObject.activeSelf)
        {
            transform.GetComponent<BoxCollider>().enabled = false;
        }
    }

    //--------------自定义成员函数--------------
    private void OnCollisionEnter(Collision other)
    {
        isCollided = true;
        menu.gameObject.SetActive(true);
        //Debug.Log("true");
    }

    private void OnCollisionExit(Collision other)
    {
        isCollided = false;
    }
}
