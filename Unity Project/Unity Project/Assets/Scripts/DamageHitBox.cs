using UnityEngine;
using System.Collections;

public class DamageHitBox : MonoBehaviour {


	private float timer=0;
	public float flinchTime = 0.5f;

	void OnTriggerStay(Collider other) {
		print ("Hit");
		if(other.tag == "EnemySword"){

		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
