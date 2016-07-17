using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour
{

    private GameObject cube;
    public Vector3 posi = new Vector3();

    //旋转速度
    private float SpeedX = 24;
    private float SpeedY = 12;

    //角度限制
    private float MinLimitY = -180;
    private float MaxLimitY = 180;

    //旋转角度
    private float mX = 0.0F;
    private float mY = 0.0F;

    //鼠标缩放距离最值
    private float MaxDistance = 10;
    private float MinDistance = 1.5F;
    //鼠标缩放速率
    private float ZoomSpeed = 2F;

    //是否启用差值
    public bool isNeedDamping = true;
    //速度
    public float Damping = 10F;

    //存储角度的四元数

    private Vector3 Rota;
    Transform follow;
    // Use this for initialization
    void Start()
    {
        mX = transform.eulerAngles.x;
        mY = transform.eulerAngles.y;
        follow = GameObject.FindWithTag("Cube").transform;
        posi = follow.position + Vector3.up * 0 + Vector3.forward * 5;
        transform.SetParent(follow);
    }
    private float CompareAngle(float angle, float min, float max)
    {
        if (angle > max) return max;
        if (angle < min) return min;
        return angle;
    }
    // Update is called once per frame
    void Update() {
        //获取鼠标输入
        mX = Input.GetAxis("Mouse X") * SpeedX;
        mY = Input.GetAxis("Mouse Y") * SpeedY;
        //范围限制
        mY = CompareAngle(mY, MinLimitY, MaxLimitY);

        Rota.x = mY;
        Rota.y = mX;
        Rota.z = 0;

        transform.RotateAround(follow.position, Rota, Time.deltaTime*50);

        if (transform.eulerAngles.z != 0) {
            transform.Rotate(0,0,-transform.eulerAngles.z,Space.Self);
        }
    }
}
