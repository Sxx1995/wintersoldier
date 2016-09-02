using UnityEngine;
using System.Collections;
using NPC_Crowd;
using System.IO;
using System;
using System.Text;
public class new_device : MonoBehaviour {
    vec[] record_send = new vec[10];
    //vec[] record_recieve = new vec[5];
    int n;
    int thread=10;
    int buff;
    int Thread_flame;
    public Transform self;
    public double para;
    public vec get_infor = new vec();
    char[] sss = new char[50];
    public vec alert= new vec();
    public vec information = new vec();
    // Use this for initialization
    void Start () {
        n = 0;
        buff=0;
        Thread_flame = 350;
        for (int i = 0; i < 10; i++)
        {
            record_send[i] = new vec();
        }
        alert.x = 0;
        alert.y = 0;
    }

    public void Read()
    {
        StreamReader sr = new StreamReader("D:\\Unity\\try\\test\\dist1\\data.txt", Encoding.Default);
        String line;
        string temp = "";
        int num = 0;
        while ((line = sr.ReadLine()) != null)
        {
            //print(line);
            for (int i = 4; i < line.Length; i++)
            {
                if (line[i] != ' ')
                    temp += line[i];
                else
                {
                    num++;
                    if (num == 1) information.x = int.Parse(temp);
                    if (num == 2) information.y = int.Parse(temp);
                    temp = "";
                }
                information.s = temp;
            }
        }
        sr.Close();
    }

    public void Write(int x, int y, string ss)
    {
        print("final_print");
        string path = "D:\\Unity\\try\\test\\dist1\\data.txt";
        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        string s = "011" + ' ' +x.ToString() + ' ' + y.ToString() + ' ' +ss;
        sw.Write(s);
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }
    // Update is called once per frame
    void Update () {
        n = n + 1;
        n = n % thread;
        if (n == 0)
        {
            record_send[buff].x = self.position.x;
            record_send[buff].y = self.position.z;
            if (self.GetComponent<desision_making>().crowd_flag)
                record_send[buff].s = "crowd";
            else
                if (self.GetComponent<c>().ans > Thread_flame)
                    record_send[buff].s = "fire";
                else
                    record_send[buff].s = "normal";
            buff++;
        }
        if (buff==10&&self.name=="NPC1")
        {
            //send;
            if (alert.x != 0 || alert.y != 0)
            {
                get_infor = alert;
                get_infor.s = "!!!!!!";
            }
            else
            {
                get_infor = record_send[buff-1];
            }
            Write((int)get_infor.x,(int)get_infor.y,get_infor.s);
            //System.Threading.Thread.Sleep(200);
            //System.Diagnostics.Process.Start("D:\\Unity\\try\\test\\dist1\\unitysend1.exe");
            buff = 0;
            alert.x = 0;
            alert.y = 0;
        }
        //get_infor=recieve
        if (buff == 10 && self.name == "NPC8")
        {
            //for (int i = 0; i <= 100000; i++)
            //    for (int j = 0; j <= 100; j++)
            //        ;
            Read(); 
            //print(information.x.ToString() + ' ' + information.y.ToString() + ' ' + information.s);
            if (information.s == "fire") para = 0.1;
            if (information.s == "crowd") para = -0.3;
            if (information.s == "normal") para = 0.3;
            if (information.s == "!!!!!!") para = 0;
            buff = 0;
        }


    }
}
