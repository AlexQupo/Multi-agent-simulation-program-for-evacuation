using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public InfoScript infoScript;
    public ExitScript ExitMaksi;
    public ExitScript ExitMiddle;
    public ExitScript ExitFiolent;

    public int numAgentsOfEvacMaksi;
    public int numAgentsOfEvacMiddle;
    public int numAgentsOfFiolent;

    public int totalAgentOfEvac = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        numAgentsOfEvacMaksi = ExitMaksi.counter;
        numAgentsOfEvacMiddle = ExitMiddle.counter;
        numAgentsOfFiolent = ExitFiolent.counter;
        totalAgentOfEvac = numAgentsOfEvacMaksi + numAgentsOfEvacMiddle + numAgentsOfFiolent;
        infoScript.SetNumOfAgentsEvac(totalAgentOfEvac);
    }
}
