using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AreaScript : MonoBehaviour
{
    public GameObject[] respawns;
    public LayerMask layerMask;
    public Vector3 exitPoint;
    void Start()
    {
        Collider[] respawnColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, layerMask);
        for (int i = 0; i < respawnColliders.Length; i++)
        {
            respawnColliders[i].gameObject.GetComponent<RespawnScript>().exitPoint = exitPoint;
        }
    }
}
