using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIanother : MonoBehaviour {

    public Rect window01 = new Rect(20, 20, 150, 100);   //定义窗体初始状态：X、Y位置及长宽
    private Text t;
    private Button s;
    
    public bool WindowShow = false;
    public bool Stop = false;
    // Use this for initialization
    void Start () {
        s = GameObject.Find("Canvas/Button").GetComponent<Button>();
        s.onClick.AddListener(OnClick);
        t = s.transform.FindChild("Text").GetComponent<Text>();
        string tt = "begin";
        t.text = tt;
        Stop = true;
    }
    void OnClick()
    {
        if (!WindowShow)
        {
            string tt= "Stop";
            t.text = tt;
        }
        else
        {
            string tt = "begin";
            t.text = tt;
        }
        WindowShow = !WindowShow;
        if (WindowShow)
        {
            Stop = false;
        }
        if (!WindowShow)
        {
            Stop = true;
        }
    }
}
