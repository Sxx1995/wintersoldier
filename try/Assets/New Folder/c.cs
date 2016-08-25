using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class c : MonoBehaviour {
    private temperature s;
    public GameObject flame;
    public Vector3 selfpos, flamepos;
    public Transform self;
    private float tmp_center;
    private float[] tmp=new float[10];
    float a = 1f;
    const int WM_COPYDATA = 0x004A;
    public double ans;
    // Use this for initialization

    [DllImport("User32.dll", EntryPoint = "SendMessage")]
    private static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    [DllImport("User32.dll", EntryPoint = "FindWindow")]
    private static extern int FindWindow(string lpClassName, string lpWindowName);

    void Start () {
        //System.Diagnostics.Process.Start("D:\\Projects\\ConsoleApplication22\\ConsoleApplication22\\bin\\Debug\\ConsoleApplication22.exe");
    }
    // Update is called once per frame
    private void Send()
    {
        int hWnd = FindWindow(null, @"ConsoleApplication22");
        if (hWnd != 0)
        {
            SendMessage((IntPtr)hWnd, WM_COPYDATA, 0, 1);
        }
    }
    void Calculate(int num)
    {
        float Lens;
        Lens = (selfpos.x - flamepos.x) * (selfpos.x - flamepos.x) +
            (selfpos.y - flamepos.y) * (selfpos.y - flamepos.y) +
            (selfpos.z - flamepos.z) * (selfpos.z - flamepos.z);
        Lens = Lens / 100;
        tmp[num] = Mathf.Exp(-Lens/(4*a* a))/
            ((2*a*Mathf.Sqrt((float)3.1415))* 
            (2 * a * Mathf.Sqrt((float)3.1415))* 
            (2 * a * Mathf.Sqrt((float)3.1415)))*tmp_center;
        int i;
        ans = 0;
        for (i = 1; i <= 3; i++)
            ans = ans + tmp[i];
        //Send();
    }
    void Update () {
        string ss = "flame";
        string sss;
        int i;
        for ( i = 1; i <= 3; i++)
        {
            sss = ss + i.ToString();
            flame = GameObject.Find(sss);
            s = flame.GetComponent<temperature>();
            tmp_center = s.centertemperature;
            flamepos = s.posi;
            selfpos = self.transform.position;
            Calculate(i);
        }
    }
    void OnGUI() {
        //GUI.Label(new Rect(200, 200, 100,100), ans.ToString());
    }
}
