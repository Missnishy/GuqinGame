using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UISwitch : MonoBehaviour
{
    public GameObject previousUI;
    public GameObject nextUI;
    

    public void SwitchUI()
    {
        previousUI.SetActive(false);
        nextUI.SetActive(true);
    }




}
