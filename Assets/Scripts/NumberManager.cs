using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    public static int number;
    Text text;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();

        number = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = number.ToString();
    }
}
