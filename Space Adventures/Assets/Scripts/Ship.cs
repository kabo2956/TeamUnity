using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
	private float maxSpeed;
	private Rigidbody rBody;
	private Vector3 prevVel;
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
			rBody.AddForce (new Vector3 (-8*rBody.mass, 0, 0));
		}
		prevVel = rBody.velocity;
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.name.Equals("KillWall")) {
			Physics.IgnoreCollision (gameObject.GetComponent<Collider> (), coll.gameObject.GetComponent<Collider>());
			rBody.velocity = prevVel;
		}
	}
}
