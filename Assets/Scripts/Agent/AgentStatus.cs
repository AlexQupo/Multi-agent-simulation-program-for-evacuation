//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AgentStatus : MonoBehaviour
{

    
    public Vector3[] exits;
    public Vector3 initialExit;
    public Vector3 curExit;

    public string currentStatus;
    [Space]
    [Header("Характеристики (в процентах)")]
    [Tooltip("Растерянный")]
    [Range(0, 100)]
    public int purposeful; //целеустремленный
    [Tooltip("Пугливый")]
    [Range(0, 100)]
    public int brave;//храбрый
    [Tooltip("Неуверенный в себе")]
    [Range(0, 100)]
    public int selfConfident;//самоуверенный
    [Space]

    public NavMeshAgent agent;
    public AgentVision vision;

    private Manager brainManager;
    private double[] characters;
    private bool isSeeFire;
    private FireSize fireSize;
    private int numberOfFireSize;
    private bool isSeeCrowd;
    private int numberOfCrowd;
    private bool isSeeCongestion;
    private int numberOfCongestion;

    private bool isResult;
    private bool isNothinghappened;
    private double resultOfMax;
    private double resultOfCentroid;
    private TypeOfSituations currentSituation;
    private bool isBusy;

    public float delay;
    public float currentDelay;
    public float timerOfAction;
    

    private float speed;
    private float delayOfAction;

    private Vector3 oldExit;
    private Vector3 newExit;


    void Start()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(initialExit, path))
        {
            agent.isStopped = true;
            agent.path = path;
        }
        curExit = initialExit;
        currentDelay = 0;
        delayOfAction = 0;
        isNothinghappened = true;
        isBusy = false;

        purposeful = Random.Range(0, 100);
        brave = Random.Range(0, 100);
        selfConfident = Random.Range(0, 100);
        characters = new double[3] { purposeful, brave, selfConfident };

        brainManager = new Manager();
        brainManager.MakeRules();

        speed = agent.speed;

        oldExit = new Vector3();
        newExit = new Vector3();
    }

    void Update()
    {
        characters = new double[3] { purposeful, brave, selfConfident };
        if (!isResult)
        {
            if (currentDelay <= 0)
            {
                VariableInitialization();
                if (!isSeeFire && !isSeeCrowd && !isSeeCongestion)
                {
                    isNothinghappened = true;
                }
                else
                {
                    isNothinghappened = false;
                    StateSelectionAndCalculate();
                    currentDelay = delay;
                }
            }
            else
            {
                currentDelay -= Time.deltaTime;
            }

            if (!isNothinghappened)
            {
                if (brainManager.CountMax() != -1)
                {
                    isResult = true;
                    resultOfMax = brainManager.CountMax();
                    resultOfCentroid = brainManager.CountCentroid();
                }
                else
                {
                    isResult = false;
                    currentStatus = "Агент продолжает движение!";
                }
                isNothinghappened = true;
            }

        }
        else
        {
            if (currentSituation == TypeOfSituations.Fire)
            {
                ActionForFire(resultOfCentroid, 10);
            }
            else if (currentSituation == TypeOfSituations.Crowd)
            {
                ActionForCrowd(resultOfCentroid);
            }
            else if (currentSituation == TypeOfSituations.Congestion)
            {
                ActionForCongestion(resultOfCentroid);
            }
        }
    }

    void ActionForFire(double x, float time)
    {
        if (x >= 0 && x < 15)
        {
            agent.speed = speed;
            currentStatus = "Агент проходит мимо огня.";
            isResult = false;
        }
        else if (x >= 15 && x < 35)
        {
            currentStatus = "Агент замедлился в два раза.";
            SlowDown(2, time);
        }
        else if (x >= 35 && x < 65)
        {
            currentStatus = "Агент остановился на секунду.";
            Stop(1);
            
        }
        else if (x >= 65 && x < 85)
        {
            currentStatus = "Агент остановился на " + time + " секунд.";
            Stop(time);
            
        }
        else if (x >= 85 && x < 100)
        {
            currentStatus = "Агент встал на месте.";
            Stop(30);

        }

    }

    void ActionForCrowd(double x)
    {
        if (x >= 0 && x < 35)
        {
            currentStatus = "Агент ускорился в 1.5 раза.";
            SpeedUp(1.5f, 15);
        }
        else if (x >= 35 && x < 65)
        {
            currentStatus = "Агент ускорился в 2 раза.";
            SpeedUp(2, 10);
        }
        else if (x >= 65 && x < 100)
        {
            currentStatus = "Агент ускорился в 4 раза.";
            SpeedUp(4, 10);
        }
    }

    void ActionForCongestion(double x)
    {
        if (x >= 0 && x < 35)
        {
            currentStatus = "Агент остается на этом выходе.";
            isResult = false;
        }
        else if (x >= 35 && x < 65)
        {
            currentStatus = "Агент мешкается.";
            if (!isBusy)
            {
                oldExit = curExit;
                newExit = new Vector3();
                for (int i = 0; i < exits.Length; i++)
                {
                    if (exits[i].x != curExit.x && exits[i].z != curExit.z)
                    {
                        newExit = exits[i];
                        break;
                    }
                }
                isBusy = true;
            }
            if (timerOfAction > 0)
            {
                Lags(oldExit, newExit);
                timerOfAction -= Time.deltaTime;
            }
            else
            {
                isResult = false;
                timerOfAction = 10f;
            }
        }
        else if (x >= 65 && x < 100)
        {
            currentStatus = "Идет на другой выход.";
            ChangeDirection();
            isResult = false;
        }
    }

    void SpeedUp(float multiplier, float timeInSec)
    {
        if (timerOfAction > 0)
        {
            agent.speed = speed * multiplier;
            timerOfAction -= Time.deltaTime;
        }
        else
        {
            agent.speed = speed;
            isResult = false;
            timerOfAction = timeInSec;
        }

    }

    void Stop(float timeInSec)
    {            
        if (timerOfAction > 0)
        {
            agent.isStopped = true;
            timerOfAction -= Time.deltaTime;
        }
        else
        {
            agent.isStopped = false;
            isResult = false;
            timerOfAction = timeInSec;
        }
    }

    void SlowDown(float multiplier, float timeInSec)
    {
        if (timerOfAction > 0)
        {
            agent.speed = speed / multiplier;
            timerOfAction -= Time.deltaTime;
        }
        else
        {
            agent.speed = speed;
            isResult = false;
            timerOfAction = timeInSec;
        }
    }

    void Lags(Vector3 oldExit, Vector3 newExit)
    {

        if (delayOfAction >= 0 && delayOfAction < 5)
        {
            agent.SetDestination(newExit);
            curExit = newExit;
            delayOfAction += Time.deltaTime;
        }
        else if (delayOfAction >= 5)
        {
            curExit = newExit;
            agent.SetDestination(oldExit);
            isBusy = false;
            delayOfAction = 0;
        }
    }

    void ChangeDirection()
    {
        for (int i = 0; i < exits.Length; i++)
        {
            if (exits[i].x != curExit.x && exits[i].z != curExit.z)
            {
                agent.SetDestination(exits[i]);
                curExit = exits[i];
                currentStatus = "Идет на другой выход";
                break;
            }
            else
            {
                currentStatus = "Остался на этом выходе";
            }
        }
    }

    void VariableInitialization()
    {

        isSeeFire = vision.isVisibleFire;
        fireSize = vision.flameRating;
        numberOfCrowd = vision.botNearCounter;
        numberOfCongestion = vision.botCounter;

        if (numberOfCrowd > 0)
        {
            isSeeCrowd = true;
            if (numberOfCrowd > 10)
                numberOfCrowd = 10;
        }
        else
            isSeeCrowd = false;

        if (numberOfCongestion > 0)
        {
            isSeeCongestion = true;
            if (numberOfCongestion > 25)
                numberOfCongestion = 25;
        }
        else
            isSeeCongestion = false;

        if (isSeeFire)
        {
            switch (fireSize)
            {
                case FireSize.Weak:
                    numberOfFireSize = Random.Range(0, 30);
                    break;
                case FireSize.Middle:
                    numberOfFireSize = Random.Range(25, 75);
                    break;
                case FireSize.Strong:
                    numberOfFireSize = Random.Range(60, 100);
                    break;
            }
        }
        else numberOfFireSize = 0;
    }
    void StateSelectionAndCalculate()
    {
        if (isSeeFire && isSeeCrowd && isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfFireSize, TypeOfSituations.Fire, (double)numberOfCrowd, TypeOfSituations.Crowd, (double)numberOfCongestion, TypeOfSituations.Congestion);
        }
        if (!isSeeFire && isSeeCrowd && isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfCrowd, TypeOfSituations.Crowd, (double)numberOfCongestion, TypeOfSituations.Congestion);
        }
        if (isSeeFire && !isSeeCrowd && isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfFireSize, TypeOfSituations.Fire, (double)numberOfCongestion, TypeOfSituations.Congestion);
        }
        if (isSeeFire && isSeeCrowd && !isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfFireSize, TypeOfSituations.Fire, (double)numberOfCrowd, TypeOfSituations.Crowd);
        }
        if (isSeeFire && !isSeeCrowd && !isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfFireSize, TypeOfSituations.Fire);
        }
        if (!isSeeFire && isSeeCrowd && !isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfCrowd, TypeOfSituations.Crowd);
        }
        if (!isSeeFire && !isSeeCrowd && isSeeCongestion)
        {
            brainManager.CalculateResult(characters, (double)numberOfCongestion, TypeOfSituations.Congestion);
        }
        currentSituation = brainManager.GetTypeOfSituation();
    }
}
