using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ChoicePath : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] exits;
    NavMeshPath[] paths;
    public Dictionary<int,float> distances;
    float distance;
    public float minDistance;
    public Transform exitPoint;

    // Start is called before the first frame update
    void Start()
    {
        SetDefault();
        CheckDistanceExit();
    }

    void SetDefault()
    {
        paths = new NavMeshPath[exits.Length];
        distances = new Dictionary<int, float>();
        distance = 0;
        minDistance = 0;
    }
    void CheckDistanceExit()
    {
        for (int i = 0; i < exits.Length; i++)
        {
            paths[i] = new NavMeshPath();
            agent.CalculatePath(exits[i].position, paths[i]);
        }
        for (int i = 0; i < paths.Length; i++)
        {
            for (int j = 0; j < paths[i].corners.Length - 1; j++)
            {
                distance += Vector3.Distance(paths[i].corners[j], paths[i].corners[j + 1]);
            }
            distances.Add(i, distance);
            distance = 0;
        }
        distances = distances.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        minDistance = distances.First().Value;
        exitPoint = exits[distances.First().Key];
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            agent.SetDestination(exitPoint.position);
        }
    }
}
