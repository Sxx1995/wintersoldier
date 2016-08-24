using UnityEngine;
using System.Collections;

public class have_a_try : MonoBehaviour {
    bool flag2=false, flag1=false;
    Color orgColor;
    private Vector3 Rota;

    //旋转速度
    private float SpeedX = 6;
    private float SpeedY = 12;

    //角度限制
    private float MinLimitY = -180;
    private float MaxLimitY = 180;

    //旋转角度
    private float mX = 0.0F;
    private float mY = 0.0F;

    private UIanother s;
    public GameObject plan;

    // Use this for initialization
    void Start () {
        orgColor = GetComponent<Renderer>().material.color;
        plan = GameObject.Find("Button");
        s = plan.GetComponent<UIanother>();
    }
    private float CompareAngle(float angle, float min, float max)
    {
        if (angle > max) return max;
        if (angle < min) return min;
        return angle;
    }
    // Update is called once per frame
    void Update () {

        if (!s.Stop)
        {
            if (flag2)
                transform.Rotate(Vector3.up * Time.deltaTime * 200);
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, Time.deltaTime * 15);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, -Time.deltaTime * 15);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-Time.deltaTime * 15, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Time.deltaTime * 15, 0, 0);
            }
            if (!Input.GetMouseButton(1))
            {
                mX = Input.GetAxis("Mouse X") * SpeedX;
                transform.Rotate(0, mX, 0, Space.Self);
            }
        }
    }


    void OnMouseExit() {
        GetComponent<Renderer>().material.color = orgColor;
        flag2 = false;
    }

    void OnMouseDown() {
        if (flag1)
            flag2 = true;
    }
}
