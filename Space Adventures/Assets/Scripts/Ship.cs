using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
	private float maxSpeed;
	private Rigidbody rBody;
	// Use this for initialization
	void Start () {
		maxSpeed = 15.0f;
		rBody = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rBody.velocity.x < -maxSpeed) {
			rBody.velocity = new Vector3 (-maxSpeed, rBody.velocity.y);
		} else {
			rBody.AddForce (new Vector3 (-8, 0, 0));
		}
	}
}
