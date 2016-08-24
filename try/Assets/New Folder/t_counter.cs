using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class t_counter : MonoBehaviour
{

    Button B;
    private float f = 1f;
    private bool flag;//1-red;0-green;
    private bool flag2;//1-continue;0-start;
    // Use this for initialization
    void Start()
    {
        B = GameObject.Find("Button2").GetComponent<Button>();
        flag = true;
        flag2 = false;
        B.GetComponent<Image>().color = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        //B.GetComponent<Image>().color = Color.red;
        //Vector3 v = Input.mousePosition;
    }

    void OnMouseOver()
    {
        if (flag2 == false) { f = 1f; flag2 = true; }
        B.GetComponent<Image>().color = Color.yellow;
        if (f > 0)
            f = f - Time.deltaTime;
        else
            B.GetComponent<Image>().color = Color.green;
    }
    void OnMouseExit()
    {
        B.GetComponent<Image>().color = Color.red;
        flag2 = false;
    }
}

