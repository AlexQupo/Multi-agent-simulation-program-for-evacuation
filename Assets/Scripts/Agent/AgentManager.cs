using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    public InfoScript infoScript;
    public static bool isGamePaused = true;
    public int numOfAgents = 0;
    private NavMeshAgent[] agents;
    private bool isCalculated;

    void Start()
    {
        agents = GameObject.FindObjectsOfType<NavMeshAgent>();
        isCalculated = false;
        numOfAgents = agents.Length;
        infoScript.SetNumOfAllAgents(numOfAgents);
    }

    void Update()
    {
        if (!isCalculated)
        {
            bool pathPending = false;
            for (int i = 0; i < agents.Length; i++)
            {
                if (agents[i].pathPending)
                {
                    pathPending = true;
                    break;
                }
            }
            if (!pathPending)
            {
                StartAgents();
                isCalculated = true;
            }
        }


        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     if (isGamePaused)
        //     {
        //         Resume();
        //         infoScript.isGamePaused = false;
        //     }
        //     else
        //     {
        //         Pause();
        //         infoScript.isGamePaused = true;
        //     }
        // }
    }
    private void StartAgents()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i].isStopped = false;
        }
    }
    // private void Pause()
    // {
    //     agents = GameObject.FindObjectsOfType<NavMeshAgent>();
    //     for (int i = 0; i < agents.Length; i++)
    //     {
    //         agents[i].isStopped = true;
    //     }
    //     isGamePaused = true;
    // }
    // private void Resume()
    // {
    //     agents = GameObject.FindObjectsOfType<NavMeshAgent>();
    //     for (int i = 0; i < agents.Length; i++)
    //     {
    //         agents[i].isStopped = false;
    //     }
    //     isGamePaused = false;
    // }
}
