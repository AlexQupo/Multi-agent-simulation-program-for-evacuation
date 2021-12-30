using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FireSize
{
    None,
    Weak,
    Middle,
    Strong,
}

enum LayerName
{
    Bot,
    Fire
}

public class AgentVision : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public float nearRadius;
    public Vector3 referenceFireSize = new Vector3(2, 0.5f, 2);

    public Collider[] objectsInRangeChecks;
    public Collider[] objectsInNearRangeChecks;
    public bool[] isSeeSmth;

    public int botNearCounter;
    public int botCounter;
    public bool isVisibleFire;

    public FireSize flameRating;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    // public int counter;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
            Analyze();
            NearCheck();

        }
    }

    private void FieldOfViewCheck()
    {
        objectsInRangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        isSeeSmth = new bool[objectsInRangeChecks.Length];

        if (objectsInRangeChecks.Length != 0)
        {
            for (int i = 0; i < objectsInRangeChecks.Length; i++)
            {
                Transform target = objectsInRangeChecks[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        isSeeSmth[i] = true;
                    else
                        isSeeSmth[i] = false;
                }
                else
                    isSeeSmth[i] = false;
            }
        }
    }

    private void NearCheck()
    {
        objectsInNearRangeChecks = Physics.OverlapSphere(transform.position, nearRadius, targetMask);
        botNearCounter = 0;
        if (objectsInNearRangeChecks.Length > 1)
        {
            for (int i = 0; i < objectsInNearRangeChecks.Length; i++)
            {
                if (isBot(objectsInNearRangeChecks[i].gameObject.layer))
                {
                    botNearCounter++;
                }
            }
        }
    }

    public void Analyze()
    {
        botCounter = 0;
        if (objectsInRangeChecks.Length != 0)
        {
            for (int i = 0; i < objectsInRangeChecks.Length; i++)
            {
                if (isSeeSmth[i])
                {
                    if (isBot(objectsInRangeChecks[i].gameObject.layer))
                    {
                        //Видит бота
                        botCounter++;
                    }
                    else if (isFire(objectsInRangeChecks[i].gameObject.layer))
                    {
                        //Видит огонь
                        flameRating = FlameRating(objectsInRangeChecks[i]);
                        isVisibleFire = true;
                    }
                    else
                        isVisibleFire = false;
                }
                else if (isFire(objectsInRangeChecks[i].gameObject.layer))
                    isVisibleFire = false;
            }
        }
        else if (isVisibleFire)
            isVisibleFire = false;
    }

    bool isBot(int objectLayer, LayerName layer = LayerName.Bot)
    {
        return objectLayer == LayerMask.NameToLayer(layer.ToString());
    }

    bool isFire(int objectLayer, LayerName layer = LayerName.Fire)
    {
        return objectLayer == LayerMask.NameToLayer(layer.ToString());
    }

    FireSize FlameRating(Collider fireCollider)
    {
        Vector3 size = fireCollider.transform.localScale;

        if (size.x <= referenceFireSize.x * 20 && size.y <= referenceFireSize.y * 20 && size.z <= referenceFireSize.z * 20)
        {
            return FireSize.Weak;
        }
        else if (size.x > 20 * referenceFireSize.x && size.y > 20 * referenceFireSize.y && size.z > 20 * referenceFireSize.z
                && size.x < 70 * referenceFireSize.x && size.y < 70 * referenceFireSize.y && size.z < 70 * referenceFireSize.z)
        {
            return FireSize.Middle;
        }
        else if (size.x >= 70 * referenceFireSize.x && size.y >= 70 * referenceFireSize.y && size.z >= 70 * referenceFireSize.z)
        {
            return FireSize.Strong;
        }
        else 
            return FireSize.None;
    }
}