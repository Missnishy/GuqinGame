using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

/// <summary>
/// 
/// </summary>
public class Tiao_temp : MonoBehaviour
{
    public string eventID;

    private void Start() 
    {
        Koreographer.Instance.RegisterForEventsWithTime(eventID, JudgeTiao);

    }

    private void JudgeTiao(KoreographyEvent koreographyEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if(koreographyEvent.HasIntPayload())
        {
            
        }
    }
}
