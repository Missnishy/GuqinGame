using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过鼠标射线获取碰撞物体信息
/// </summary>
public class ColliderEvent : MonoBehaviour
{
    Ray ray;
    RaycastHit hitInfo;
    public GameObject obj;
    [HideInInspector]public bool isCollided = false;

    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hitInfo))
            {
                //Debug.Log(hitInfo.transform.name);
                //将鼠标射线碰撞到的物体名字与待检测物体的名字进行匹配
                if(obj.name.Equals(hitInfo.transform.name))
                {
                    isCollided = true;
                }
            }
        }
    }
}
