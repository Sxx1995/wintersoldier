using UnityEngine;
using System.Collections;
using NPC_Crowd;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
public class desision_making : MonoBehaviour {

    // Use this for initialization
    c s;
    GameObject tmp;
    double temp, last_temp;
    double Thread_flame = 400;
    double Thread_time = 20;
    double time_now, time_last;
    double sum;
    public Transform target;
    public Transform sel;
    public Transform another;
    public NPC_Parameter self;
    public bool Flag,Flag_last;
    public bool crowd_flag;
    vec velocity_tmp;
    vec velocity= new vec();
    NavMeshAgent man,man2;
    belief B = new belief();
    Vector3 last_cernor;
    vec mid= new vec();
    vec[] buffer=new vec[20];
    int total;
    int position_in_buffer;
    int T;
    public class belief
    {
        public double belief_of_nav;
        public double belief_of_flame;
        public double belief_of_mate;
    }
    double findway()
    {
        NavMeshPath path = new NavMeshPath();
        man.CalculatePath(GameObject.Find("NPC1").transform.position, path);
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = sel.transform.position;
        allWayPoints[allWayPoints.Length - 1] = target.transform.position;
        for (int j = 0; j < path.corners.Length; j++)
        {
            allWayPoints[j + 1] = path.corners[j];
        }
        float pathLength = 0f;
        for (int j = 0; j < allWayPoints.Length - 2; j++)
        {
            //print(sel.name + ' ' + allWayPoints[j]);
            pathLength += Vector3.Distance(allWayPoints[j], allWayPoints[j + 1]);
        }
        //print(pathLength);
        return pathLength;
    }
    void Start () {
        s = sel.GetComponent<c>();
        temp = s.ans;
        self = calculate.initital(sel.position.x, sel.position.z,0);
        man = gameObject.GetComponent<NavMeshAgent>();
        man2 = gameObject.GetComponent<NavMeshAgent>();
        time_now = 0;
        time_last = 0;
        temp = s.ans;
        last_temp = temp;
        B.belief_of_nav = 0.74;
        B.belief_of_mate =0.25;
        B.belief_of_flame = 0.01;
        last_cernor = Vector3.zero;
        Flag_last = false;
        total = 0;
        position_in_buffer = 0;
        T = 0;
        mid.x = 0;
        mid.y = 0;
        crowd_flag = false;
        for (int i = 0; i < 20; i++)
            buffer[i] = new vec();
        if (sel.name == "NPC1")
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
    vec get_infor_from_device (vec velocity)
    {
        vec s=sel.GetComponent<new_device>().information;
        //print(s.s + ' ' + s.x + ' ' + s.y);
        if (findway()< 50)
        {
            if (s.s == "normal")
            {
                velocity.x = 0;
                velocity.y = 0;                
            }
            else
            {
                double para = sel.GetComponent<new_device>().para;
                //print("para=" + para + "velocity.x" + velocity.x);
                if (para < 0)
                {
                    para *= -1;
                    s.x *= -1;
                    s.y *= -1;
                }
                velocity.x = velocity.x * (1 - para) + s.x * para;
                velocity.y = velocity.y * (1 - para) + s.y * para;
            }
        }
        //else
        //{
        //    man.Stop();
        //}
        return (velocity);
    }
	// Update is called once per frame
	void Update () {
        string sss;
        string ss = "NPC";
        double v;
        temp = s.ans;
        //self.posi.x = sel.transform.position.x;
        //self.posi.y = sel.transform.position.z;
        //calculate belif_of_nav---------------------------------------------------------------------------------------------------------------
        if (temp < Thread_flame)
            B.belief_of_nav *= Math.Pow(3.414f, -(temp - last_temp) / Thread_flame/10);
        else
        {
            B.belief_of_nav = 0.01;
            //print(sel.GetComponent<new_device>().alert.x);
            if (sel.name == "NPC1") {
                sel.GetComponent<new_device>().alert.x = sel.position.x;
                sel.GetComponent<new_device>().alert.y = sel.position.z;
            }
        }

        T = (T + 1) % 10;
        if (T == 0)
        {
            total += 1;
            total = Math.Min(21, total);
            position_in_buffer = (position_in_buffer + 1) % 20;
            if (total < 21 || Math.Pow(Math.Pow(mid.x - sel.transform.position.x, 2) + Math.Pow(mid.y - sel.transform.position.z, 2), 0.5) > Time.deltaTime * 15f)
            {
                mid.x = mid.x * (total - 1);
                mid.x -= buffer[position_in_buffer].x;
                mid.x += sel.transform.position.x;
                mid.x /= Math.Min(20, total);
                // - buffer[position_in_buffer].x + self.posi.x) / total;
                mid.y = (mid.y * (total - 1) - buffer[position_in_buffer].y + sel.transform.position.z) / Math.Min(20, total);
                buffer[position_in_buffer].x = sel.transform.position.x;
                buffer[position_in_buffer].y = sel.transform.position.z;
                if (total > 10 && Math.Pow(Math.Pow(mid.x - self.posi.x, 2) + Math.Pow(mid.y - self.posi.y, 2), 0.5) < Time.deltaTime * 10f)
                    crowd_flag = true;
            }
            else
            {
                //print("!!!!!!");
                crowd_flag = false;
                for (int i = 0; i < 20; i++)
                {
                    buffer[i].x = 0;
                    buffer[i].y = 0;
                }
                total = 1;
                position_in_buffer = 0;
                buffer[position_in_buffer].x = sel.transform.position.x;
                buffer[position_in_buffer].y = sel.transform.position.z;
                mid.x = sel.transform.position.x;
                mid.y = sel.transform.position.z;
                B.belief_of_nav = 0.74;
                B.belief_of_mate = 0.25;
                B.belief_of_flame = 0.01;
            }
            //print(sel.name + ' ' + buffer[position_in_buffer].x + ' ' + mid.x + ' ' + buffer[position_in_buffer].y + ' ' + mid.y);
            //total = Math.Min(total, 20);
            //print(sel.name + ' ' + mid.x + ' ' + total);
        }
        //print(sel.name + ' ' + mid.x + ' ' + mid.y + ' ');

        //calculate belif_of_flame-------------------------------------------------------------------------------------------------------------
        if (temp < Thread_flame)
        {
            B.belief_of_flame *= Math.Pow(3.414f, (temp - last_temp) / Thread_flame) * Math.Pow(3.414f, -(time_now - time_last) / Thread_time);
            B.belief_of_flame = Math.Max(0.01, B.belief_of_flame);
        } 
        else
        {
            time_last = time_now;
            B.belief_of_flame = 1;
        }
        //calculate belif_of_mate-------------------------------------------------------------------------------------------------------------
        Flag = false;
        for (int i = 1; i <= 8; i++)
        {
            sss = ss + i.ToString();
            tmp = GameObject.Find(sss);
            Vector3 other = tmp.GetComponent<Transform>().position;
            if (sel.name != sss)
            {
                if (Vector3.Distance(sel.transform.position, other) < 20) Flag = true;
            }
        }
        if (Flag == false) B.belief_of_mate = 0.01;
        if (Flag == true && Flag_last == false) B.belief_of_mate = 0.25;
        Flag_last = Flag;
        //normalize the belief-----------------------------------------------------------------------------------------------------------------
        sum = B.belief_of_flame + B.belief_of_mate + B.belief_of_nav;
        B.belief_of_flame /= sum;
        B.belief_of_mate /= sum;
        B.belief_of_nav /= sum;
        //get the vector from navgation -------------------------------------------------------------------------------------------------------
        NavMeshPath path = new NavMeshPath();
        man.CalculatePath(target.transform.position, path);
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = sel.transform.position;
        allWayPoints[allWayPoints.Length - 1] = target.transform.position;
        for (int j = 0; j < path.corners.Length; j++)
        {
            allWayPoints[j + 1] = path.corners[j];
            //print(sel.name + ' ' + allWayPoints[j + 1]);
        }
        float pathLength = 0f;
        for (int j = 0; j < allWayPoints.Length - 1; j++)
        {
            pathLength += Vector3.Distance(allWayPoints[j], allWayPoints[j + 1]);
         }
        //print(sel.name + ' ' + allWayPoints[2] + ' ' + allWayPoints[0] + ' ' + Math.Pow(Math.Pow(allWayPoints[2].x - allWayPoints[0].x, 2) + Math.Pow(allWayPoints[2].z - allWayPoints[0].z, 2), 0.5));
        if (Math.Pow(Math.Pow(allWayPoints[2].x - allWayPoints[0].x,2) + Math.Pow(allWayPoints[2].z - allWayPoints[0].z,2),0.5) > 2 && allWayPoints[2]!=last_cernor)
        {
            velocity.x = (allWayPoints[2].x - allWayPoints[0].x) / (Math.Pow(Math.Pow(allWayPoints[2].x - allWayPoints[0].x, 2) + Math.Pow(allWayPoints[2].z - allWayPoints[0].z, 2), 0.5)) * B.belief_of_nav;
            velocity.y = (allWayPoints[2].z - allWayPoints[0].z) / (Math.Pow(Math.Pow(allWayPoints[2].x - allWayPoints[0].x, 2) + Math.Pow(allWayPoints[2].z - allWayPoints[0].z, 2), 0.5)) * B.belief_of_nav;
        }
        else
        {
            last_cernor = allWayPoints[2];
            //print(sel.name+"!!!");
            if (allWayPoints.Length>2)
            {
                velocity.x = (allWayPoints[3].x - allWayPoints[0].x) / (Math.Pow(Math.Pow(allWayPoints[3].x - allWayPoints[0].x, 2) + Math.Pow(allWayPoints[3].z - allWayPoints[0].z, 2), 0.5)) * B.belief_of_nav;
                velocity.y = (allWayPoints[3].z - allWayPoints[0].z) / (Math.Pow(Math.Pow(allWayPoints[3].x - allWayPoints[0].x, 2) + Math.Pow(allWayPoints[3].z - allWayPoints[0].z, 2), 0.5)) * B.belief_of_nav;
            }
            else
            {
                velocity.x = 0;
                velocity.y = 0;

            }

        }
        //print(sel.name + ' ' + velocity.x + ' ' + velocity.y + "first");
        //get the information vector from mates------------------------------------------------------------------------------------------------
            for (int i = 1; i <= 8; i++)
         {
             sss = ss + i.ToString();
             tmp = GameObject.Find(sss);
             NPC_Parameter other=tmp.GetComponent<desision_making>().self;
             if (sel.name != sss)
             {
                 velocity_tmp = calculate.v(self, other);
                /*if (sel.name == "NPC8")
                {
                    print(sel.name + ' ' + self.posi.x + ' ' + self.posi.y + "!!!");
                    print(sss + ' ' + other.posi.x + ' ' + other.posi.y + "!!!");
                }*/
                    
                 velocity.x -= velocity_tmp.x / velocity_tmp.len() * B.belief_of_mate /Math.Pow(8,2);
                 velocity.y -= velocity_tmp.y / velocity_tmp.len() * B.belief_of_mate / Math.Pow(8,2);
             }
          }
        //print(sel.name + '(' + velocity.x.ToString() + ',' + velocity.y.ToString() + ')');
        //get the treat vector from flames-----------------------------------------------------------------------------------------------------
        ss = "flame";
            vec posi1 = new vec();
            vec posi2 = new vec();
            posi1.x = sel.position.x;
            posi1.y = sel.position.z;

            for (int i = 1; i <= 3; i++)
            {
                sss = ss + i.ToString();
                tmp = GameObject.Find(sss);
                posi2.x = tmp.transform.position.x;
                posi2.y = tmp.transform.position.z;
                double lens = Math.Pow((posi1.x- posi2.x) * (posi1.x - posi2.x) + (posi1.y - posi2.y) * (posi1.y - posi2.y), 0.5);
                velocity_tmp = calculate.obstacle(posi1, posi2, Math.Pow(tmp.GetComponent<temperature>().centertemperature/100,lens/ pathLength));
                double v_l = Math.Pow((Math.Pow(velocity_tmp.x, 2) + Math.Pow(velocity_tmp.y, 2)), 0.5);
                velocity.x += velocity_tmp.x / v_l *1* B.belief_of_flame;
                velocity.y += velocity_tmp.y / v_l *1* B.belief_of_flame;
            }
        if (sel.name == "NPC8") {
            velocity = get_infor_from_device(velocity);
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (velocity.x * velocity.x + velocity.y * velocity.y == 0)
        {
            v = 0;
            man.SetDestination(another.position);
        }
        else
            v = Math.Pow(velocity.x * velocity.x + velocity.y * velocity.y, 0.5);
        //if (sel.name == "NPC8")
        //    print(sel.name + ' ' + v + ' '+ velocity.x + ' ' + velocity.y);
        //put the vector to a movement---------------------------------------------------------------------------------------------------------
        //print(an);
        if (velocity.x * velocity.x + velocity.y * velocity.y != 0)
            {
                velocity.x /= v;
                velocity.y /= v;
                sel.transform.Translate((float)(velocity.x * Time.deltaTime * 10f), 0, (float)(velocity.y * Time.deltaTime * 10f));
                self.dis_angle = Math.Atan(velocity.x / velocity.y);
        }
        //print(sel.name + ' ' + velocity.x + ' ' + velocity.y);
        //memorization update
        time_now += 1;
        last_temp = temp;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100,100), temp.ToString());
    }
}
