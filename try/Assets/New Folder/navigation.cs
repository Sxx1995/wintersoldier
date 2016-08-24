using UnityEngine;
using System.Collections;

public class navigation : MonoBehaviour {
    private NavMeshAgent man;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 posi;
        posi.x = 300;
        posi.y = 2.5f;
        posi.z = 300;
        NavMeshAgent man = gameObject.GetComponent<NavMeshAgent>();
        man.SetDestination(posi);
	}
}
