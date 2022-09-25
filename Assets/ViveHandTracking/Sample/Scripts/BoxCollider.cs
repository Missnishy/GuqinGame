using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveHandTracking.Sample {

  // This script is used to set emission color when right hand pushes the box
  class BoxCollider : MonoBehaviour {
    private static Color color = new Color(0.3f, 0, 0, 1);

    private Material material = null;

    /*
    struct Finger
    {
      public Vector3 fingertip_pos;
      public Vector3 fingertip_v;
      public Vector3 fingermid_pos;
    }

    struct Hand
    {
      public Finger[] fingers = new Finger[5];
      public Vector3 hand_center = new Vector3(0.0f, 0.0f, 0.0f);

      public Hand(Finger[] f, Vector3 h)
      {
        this.fingers = f;
        this.hand_center = h;
      }
    }

    private  Finger thumb;
    private  Finger index;
    private  Finger middle;
    private  Finger ring;
    private  Finger pinky;
    private Vector3 hand_center;
    */

    IEnumerator Start() {
      while (true) {
        while (material == null) yield return null;
        var currentMat = material;
        currentMat.EnableKeyword("_EMISSION");
        currentMat.SetColor("_EmissionColor", color);
        while (material != null) yield return null;
        yield return new WaitForSeconds(0.3f);
        currentMat.DisableKeyword("_EMISSION");

        /*
        Transform[] myTransforms = GetComponentsInChildren<Transform>();
        foreach (var child in myTransforms)
        {
          switch(child.name)
          {
            case "R_Wrist" :
                hand_center = child.transform.position;
                break;
            case "r_thumb_2" :
                thumb.fingermid_pos = child.transform.position;
                break;
            case "r_thumb_tip" :
                thumb.fingertip_pos = child.transform.position;
                thumb.fingertip_v = child.GetComponent<Rigidbody>().velocity;
                break;
            case "r_index_2" :
                index.fingermid_pos = child.transform.position;
                break;
            case "r_index_tip" :
                index.fingertip_pos = child.transform.position;
                index.fingertip_v = child.GetComponent<Rigidbody>().velocity;
                break;
            case "r_middle_2" :
                middle.fingermid_pos = child.transform.position;
                break;
            case "r_middle_tip" :
                middle.fingertip_pos = child.transform.position;
                middle.fingertip_v = child.GetComponent<Rigidbody>().velocity;
                break;
            case "r_ring_2" :
                ring.fingermid_pos = child.transform.position;
                break;
            case "r_ring_tip" :
                ring.fingertip_pos = child.transform.position;
                ring.fingertip_v = child.GetComponent<Rigidbody>().velocity;
                break;
            case "r_pinky_2" :
                pinky.fingermid_pos = child.transform.position;
                break;
            case "r_pinky_tip" :
                pinky.fingertip_pos = child.transform.position;
                pinky.fingertip_v = child.GetComponent<Rigidbody>().velocity;
                break;
            default :
                break;
            }
            
        }
        
        Finger[] fingers = new Finger[5]{thumb, index, middle, ring, pinky};
        Hand hand_info = new Hand(fingers, hand_center);
        */
      }
    }

    void OnCollisionEnter(Collision other) {
      if (other.gameObject.name.StartsWith("Cube"))
        material = other.transform.GetComponent<Renderer>().material;
      other.transform.GetComponent<Renderer>().material.color = Color.red;
    }

     void OnCollisionStay(Collision other) {
      if (other.gameObject.name.StartsWith("Cube"))
        material = other.transform.GetComponent<Renderer>().material;
      other.transform.GetComponent<Renderer>().material.color = Color.red;
    }

    void OnCollisionExit(Collision other) {
      if (other.gameObject.name.StartsWith("Cube")) 
        material = null;
      other.transform.GetComponent<Renderer>().material.color = Color.white;
    }
  }

}
