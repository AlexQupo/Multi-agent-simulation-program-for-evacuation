using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFireScript : MonoBehaviour
{
    bool fst;
    bool snd;
    bool thd;

    Vector3 size;
    void Start()
    {
        size = new Vector3(2, 0.5f, 2);
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        fst = true;
        snd = true;
        thd = true;

        while (fst)
        {
            transform.localScale = size * 20;
            fst = false;
            yield return new WaitForSeconds(2f);
        }

        while (snd)
        {
            transform.localScale = size * 60;
            snd = false;
            yield return new WaitForSeconds(2f);
        }

        while (thd)
        {
            transform.localScale = size * 100;
            thd = false;
            yield return new WaitForSeconds(2f);
        }
        StartCoroutine(TestCoroutine());
    }
}
