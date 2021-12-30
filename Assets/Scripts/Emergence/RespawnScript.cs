using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject prefBot;
    public int count;
    GameObject newAgent;

    public Vector3 exitPoint;

    void Start()
    {
        List<Vector3> positions = new List<Vector3>();
        if(prefBot.GetComponent<AgentStatus>() != null)
        {
            prefBot.GetComponent<AgentStatus>().initialExit = exitPoint;
        }
        for (int i = 0; i < count; i++)
        {
            SetBot();
        }
    }

    void SetBot()
    {
        float halfSizeX = transform.localScale.x / 2;
        float halfSizeZ = transform.localScale.z / 2;
        Vector3 pos = new Vector3(
                    Random.Range(-halfSizeX, halfSizeX),
                    0,
                    Random.Range(-halfSizeZ, halfSizeZ)
                    );
        pos += transform.position;

        float radius = prefBot.GetComponent<CapsuleCollider>().radius;
        GameObject newAgent;
        if (Physics.OverlapSphere(pos, radius) != null)
        {
            newAgent = Instantiate(prefBot, pos, Quaternion.identity);
        }
        else
        {
            SetBot();
        }
    }

}
