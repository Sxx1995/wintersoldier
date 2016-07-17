using UnityEngine;
using System.Collections;

public class have_a_try : MonoBehaviour {
    bool flag2=false, flag1=false;
    Color orgColor;
    private Vector3 Rota;

    //旋转速度
    private float SpeedX = 24;
    private float SpeedY = 12;

    //角度限制
    private float MinLimitY = -180;
    private float MaxLimitY = 180;

    //旋转角度
    private float mX = 0.0F;
    private float mY = 0.0F;


    // Use this for initialization
    void Start () {
        orgColor = GetComponent<Renderer>().material.color;
	}
    private float CompareAngle(float angle, float min, float max)
    {
        if (angle > max) return max;
        if (angle < min) return min;
        return angle;
    }
    // Update is called once per frame
    void Update () {
        if (flag2)
            transform.Rotate(Vector3.up * Time.deltaTime * 200);
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Time.deltaTime*3, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Time.deltaTime*3, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(0, 0, Time.deltaTime*3);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0, 0, -Time.deltaTime*3);
        }
    }

    void OnMouseOver() {
        GetComponent<Renderer>().material.color = Color.yellow;
        flag1 = true;
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
