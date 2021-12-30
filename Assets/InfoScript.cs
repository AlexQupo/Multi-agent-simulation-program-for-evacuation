using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScript : MonoBehaviour
{
    DateTime stopwatch;
    Stopwatch stopwatch2;
    public bool isGamePaused = true;
    public Text numOfAllAgents;
    public Text numOfRescuedAgents;
    public Text NumOfCurrentAgents;
    public Text Timer;

    int nOfAll = 0;
    int nOfResc = 0;
    private void Start()
    {
        stopwatch = new DateTime();
        stopwatch2 = new Stopwatch();
        stopwatch2.Start();
    }

    public void SetNumOfAllAgents(int number)
    {
        numOfAllAgents.text = number.ToString();
        nOfAll = number;
    }
    public void SetNumOfAgentsEvac(int number)
    {
        numOfRescuedAgents.text = number.ToString();
        nOfResc = number;
    }

    private void Update()
    {
        NumOfCurrentAgents.text = (nOfAll - nOfResc).ToString();
    }
    void FixedUpdate()
    {

        // if (isGamePaused)
        // {
        //     isGamePaused = false;
        //     stopwatch2.Start();
        // }
        // else
        // {
        //     isGamePaused = true;
        //     stopwatch2.Stop();
        // }


        TimeSpan ts = stopwatch2.Elapsed;
        Timer.text = ts.ToString();
        if((nOfAll - nOfResc) <= 0)
        {
            stopwatch2.Stop();
        }
        // if ((nOfAll - nOfResc) >= 0)
        // {

        //     if (!isGamePaused)
        //     {
        //         StartTimer();
        //     }

        // }
    }

    private void StartTimer()
    {
        stopwatch2.Start();
        stopwatch.AddSeconds(1);
        Timer.text = stopwatch.ToLongTimeString();
    }

    private void StopTimer()
    {

    }
}
