using UnityEngine;
using System.Collections;

public class new_UI : MonoBehaviour
{

    // Use this for initialization

    public Rect window01 = new Rect(20, 20, 150, 100);   //定义窗体初始状态：X、Y位置及长宽



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool WindowShow = false;
    public bool Stop = false;
    bool k=true;
    void OnGUI()
    {
        if (!WindowShow) {
            k = GUI.Button(new Rect(310, 10, 80, 30), "暂停");
        }
        else
        {
            k = GUI.Button(new Rect(310, 10, 80, 30), "开始");
        }
        if (k)
        {
            WindowShow = !WindowShow;
        }

        if (WindowShow)
        {
            GUI.Window(0, window01, DoMyWindow, "暂停窗口");
            Stop = true;
        }
        if (!WindowShow)
        {
            Stop = false;
        }
    }
    void DoMyWindow(int windowID)
    {

        //GUI.DragWindow(new Rect(0,0,10000,20));
        GUI.DragWindow(new Rect(0, 0, 150, 20));             //使用DragWindow设置window窗体为可被鼠标拖动移动，并设置window窗体的鼠标响应范围，四个值分别是窗体中响应区的开始X、Y位置（窗体中的局部坐标），响应区的长宽。


    }
}