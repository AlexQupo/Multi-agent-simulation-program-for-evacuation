using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnalyzer
{
    private Collider[] colliders;
    private bool[] isVisible;

    private int botCounter;
    private bool isVisibleFire;

    AgentAnalyzer(Collider[] coldrs, bool[] isVsble)
    {
        colliders = coldrs;
        isVisible = isVsble;
    }

    enum LayerName
    {
        Bot,
        Fire
    }

    public void Analyze()
    {
        botCounter = 0;
        if (colliders.Length != 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (isVisible[i])
                {
                    if (isBot(colliders[i].gameObject.layer))
                    {
                        //Видит бота
                        botCounter++;
                        Debug.Log("Видит бота");
                    }
                    else if (isFire(colliders[i].gameObject.layer))
                    {
                        //Видит огонь
                        isVisibleFire = true;
                        Debug.Log("Видит огонь");
                    }
                    else
                        isVisibleFire = false;
                }
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
}
