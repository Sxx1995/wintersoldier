using UnityEngine;
using System.Collections;
using System;
public class temperature : MonoBehaviour {
    public float centertemperature;
    public Vector3 posi;
    System.Random R = new System.Random();
    int max = 150000;
    int min = 50000;
	// Use this for initialization
	void Start () {
        centertemperature = R.Next(min, max);
        transform.tag= "flame";
        posi = transform.position;
        GetComponent<Renderer>().material.color = Color.red;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
