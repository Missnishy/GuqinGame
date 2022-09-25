using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SingleNoteControl : MonoBehaviour
{
    public Transform successEffect;
    public Transform failEffect;

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "FailCollider")
        {
            Destroy(gameObject);
            Instantiate(failEffect, transform.position, failEffect.rotation);
        }
        if(other.gameObject.name == "SuccessCollider")
        {
            Destroy(gameObject);
            Instantiate(successEffect, transform.position, successEffect.rotation);
        }
    }
}
