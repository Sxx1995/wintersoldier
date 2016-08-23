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
    double temp;
    public Transform target;
    public Transform sel;
    public NPC_Parameter self;
    vec velocity_tmp;
    vec velocity= new vec();
    Vector3 last_posi, now_posi;
    double time_now, time_last;
    NavMeshAgent man;
    void Start () {
        s = sel.GetComponent<c>();
        temp = s.ans;
        self = calculate.initital(sel.position.x, sel.position.y,0);
        last_posi = sel.transform.position;
        now_posi = last_posi;
        man = gameObject.GetComponent<NavMeshAgent>();

    }
    
	// Update is called once per frame
	void Update () {
        string sss;
        string ss = "NPC";
        double v;
        temp = s.ans;
        if (temp<300)
        {
            man.SetDestination(target.position);   
        } 
        else
        {
            velocity.x = now_posi.x - last_posi.x*0.25;
            velocity.y = now_posi.z - last_posi.y*0.25;
            for (int i = 1; i <= 3; i++)
            {
                sss = ss + i.ToString();
                tmp = GameObject.Find(sss);
                NPC_Parameter other=tmp.GetComponent<desision_making>().self;
                if (sel.name != sss)
                {
                    velocity_tmp = calculate.v(self, other);
                    velocity.x += velocity_tmp.x;
                    velocity.y += velocity_tmp.y;
                }
            }

            ss = "flame";
            vec posi1 = new vec();
            vec posi2 = new vec();
            posi1.x = sel.position.x;
            posi1.y = sel.position.y;

            for (int i = 1; i <= 3; i++)
            {
                sss = ss + i.ToString();
                tmp = GameObject.Find(sss);
                posi2.x = tmp.transform.position.x;
                posi2.y = tmp.transform.position.y;
                NavMeshPath path = new NavMeshPath();
                man.CalculatePath(tmp.transform.position, path);
                Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
                allWayPoints[0] = transform.position;
                allWayPoints[allWayPoints.Length - 1] = tmp.transform.position;
                for (int j = 0; j < path.corners.Length; j++)
                {
                    allWayPoints[j + 1] = path.corners[j];
                }
                float pathLength = 0f;
                for (int j = 0; j < allWayPoints.Length - 1; j++)
                {
                    pathLength += Vector3.Distance(allWayPoints[j], allWayPoints[j + 1]);
                }
                double lens = Math.Pow((posi1.x- posi2.x) * (posi1.x - posi2.x) + (posi1.y - posi2.y) * (posi1.y - posi2.y), 0.5);
                velocity_tmp = calculate.obstacle(posi1, posi2, Math.Pow(tmp.GetComponent<temperature>().centertemperature,lens/ pathLength));
                velocity.x += velocity_tmp.x;
                velocity.y += velocity_tmp.y;
            }
            v = Math.Pow(velocity.x * velocity.x + velocity.y * velocity.y, 0.5);
            double an=Math.Atan(velocity.x / velocity.y);
            print(an);
            if (v != 0)
            {
                sel.transform.Translate((float)(Math.Sin(an) * Time.deltaTime * 7f), 0, (float)(Math.Cos(an) * Time.deltaTime * 7f));
            }
        }
        last_posi = now_posi;
        now_posi = sel.position;
        self.dis_angle = Math.Atan((last_posi.x - now_posi.x) / (last_posi.z - now_posi.z));

    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100,100), temp.ToString());
    }
}
