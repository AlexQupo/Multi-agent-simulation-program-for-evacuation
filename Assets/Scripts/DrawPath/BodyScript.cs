using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyScript : MonoBehaviour
{
    PointScript pointScript;
    void Awake()
    {
        pointScript = transform.parent.GetComponent<PointScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bot")
            pointScript.CollisionFromChild(other);
    }
}
