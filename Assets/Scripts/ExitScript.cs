using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public int counter;

    void Start()
    {
        counter = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        
        Destroy(other.gameObject);
        if(other.CompareTag("Bot"))
        {
            counter++;
        }
        
    }
}
