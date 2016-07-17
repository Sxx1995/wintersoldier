using UnityEngine;
using System.Collections;

public class c : MonoBehaviour {
    private temperature s;
    public GameObject flame;
    private Vector3 selfpos, flamepos;
    private float temp,tmp_center;
    float a = 1f;
	// Use this for initialization
	void Start () {
	    
	}
	// Update is called once per frame
    void Calculate()
    {
        float Lens;
        Lens = (selfpos.x - flamepos.x) * (selfpos.x - flamepos.x) +
            (selfpos.y - flamepos.y) * (selfpos.y - flamepos.y) +
            (selfpos.z - flamepos.z) * (selfpos.z - flamepos.z);
        Lens = Lens / 100;
        temp = Mathf.Exp(-Lens/(4*a* a))/
            ((2*a*Mathf.Sqrt((float)3.1415))* 
            (2 * a * Mathf.Sqrt((float)3.1415))* 
            (2 * a * Mathf.Sqrt((float)3.1415)))*tmp_center;
    }
	void Update () {
        flame = GameObject.FindWithTag("flame");
        s = flame.GetComponent<temperature>();
        tmp_center = s.centertemperature;
        flamepos = s.posi;
        selfpos = transform.position;
        Calculate();
    }
    void OnGUI() {
        GUI.Label(new Rect(300, 200, 100, 100), temp.ToString());
    }
}
