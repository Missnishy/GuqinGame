using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AudioManager : MonoBehaviour
{
    //public static AudioManager instance { get; private set; }
 
    public AudioSource audioCom;
    private static bool isPlay;
    
 
    // Start is called before the first frame update
    void Start()
    {
        //instance = this;
        isPlay = false;
    }
 
    // Update is called once per frame
    void Update()
    {
        //Open Music
        if(Input.GetMouseButtonDown(0))
        {
            audioCom.Play();
            isPlay = true;
        }

        //Close Music
        if(Input.GetMouseButtonDown(1))
        {
            audioCom.Stop();
            audioCom.time = 0f;
            isPlay = false;
        }
    }

    public static bool getIsPlayValue()
    {
        return isPlay;
    }
 

}