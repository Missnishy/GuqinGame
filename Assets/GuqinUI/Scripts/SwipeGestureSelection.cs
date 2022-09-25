using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class SwipeGestureSelection : MonoBehaviour
{
    public GameObject scrollBar;
    float scrollPos = 0;
    float[] pos;
    int objIndex = 0;

    public void Next()
    {
        if(objIndex < pos.Length - 1 && objIndex >= 0)
        {
            objIndex++;
            scrollPos = pos[objIndex];
        }
    }

    public void Previous()
    {
        if(objIndex < pos.Length && objIndex > 0)
        {
            objIndex--;
            scrollPos = pos[objIndex];
        }
    }

    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        
        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        
        if(Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for(int j = 0; j < pos.Length; j++)
            {
                if(scrollPos < pos[j] + (distance / 2) && scrollPos > pos[j] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[j], 0.1f);
                    objIndex = j;
                }
            }
        }
        
        for(int m = 0; m < pos.Length; m++)
        {
            if(scrollPos < pos[m] + (distance / 2) && scrollPos > pos[m] - (distance / 2))
            {
                transform.GetChild(m).localScale = Vector2.Lerp(transform.GetChild(m).localScale, new Vector2(1f, 1f), 0.1f);
                for(int n = 0; n < pos.Length; n++)
                {
                    if(n != m)
                    {
                        transform.GetChild(n).localScale = Vector2.Lerp(transform.GetChild(n).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
        
    }
}
