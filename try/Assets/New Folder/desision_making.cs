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
    public NPC_Parameter self;
    public bool Flag,Flag_last;
    vec velocity_tmp;
    vec velocity= new vec();
    NavMeshAgent man;
    belief B = new belief();
    Vector3 last_cernor;
    public class belief
    {
        public double belief_of_nav;
        public double belief_of_flame;
        public double belief_of_mate;
    }

    void Start () {
        s = sel.GetComponent<c>();
        temp = s.ans;
        self = calculate.initital(sel.position.x, sel.position.z,0);
        man = gameObject.GetComponent<NavMeshAgent>();
        time_now = 0;
        time_last = 0;
        temp = s.ans;
        last_temp = temp;
        B.belief_of_nav = 0.74;
        B.belief_of_mate =0.25;
        B.belief_of_flame = 0.01;
        last_cernor = Vector3.zero;
        Flag_last = false;
    }
    
	// Update is called once per frame
	void Update () {
        string sss;
        string ss = "NPC";
        double v;
        temp = s.ans;
        //calculate belif_of_nav---------------------------------------------------------------------------------------------------------------
        if (temp < Thread_flame)
            B.belief_of_nav *= Math.Pow(3.414f, -(temp - last_temp) / Thread_flame/10);
        else
            B.belief_of_nav = 0.01;
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
        for (int i = 1; i <= 3; i++)
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
        print(sel.name + "belief_of_nav" + B.belief_of_nav);
        print("belief_of_mate" + B.belief_of_mate);
        print("belief_of_flame" + B.belief_of_flame);
        
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
        for (int i = 1; i <= 3; i++)
         {
             sss = ss + i.ToString();
             tmp = GameObject.Find(sss);
             NPC_Parameter other=tmp.GetComponent<desision_making>().self;
             if (sel.name != sss)
             {
                 velocity_tmp = calculate.v(self, other);
                //print(sel.name + ' ' + self.posi.x + ' ' + self.posi.y + "!!!");
                //print(sss + ' ' + other.posi.x + ' ' + other.posi.y + "!!!");
                velocity.x -= velocity_tmp.x / velocity_tmp.len() * B.belief_of_mate ;
                 velocity.y -= velocity_tmp.y / velocity_tmp.len() * B.belief_of_mate ;
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
            v = Math.Pow(velocity.x * velocity.x + velocity.y * velocity.y, 0.5);
        velocity.x /= v;
        velocity.y /= v;
        //put the vector to a movement---------------------------------------------------------------------------------------------------------
        //print(an);
        if (v != 0)
            {
                sel.transform.Translate((float)(velocity.x * Time.deltaTime * 10f), 0, (float)(velocity.y * Time.deltaTime * 10f));
            }
        //print(sel.name + ' ' + velocity.x + ' ' + velocity.y);
        //memorization update
        self.dis_angle = Math.Atan(velocity.x / velocity.y);
        time_now += 1;
        last_temp = temp;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100,100), temp.ToString());
    }
}
