using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ship obstacle that the players will have to avoid.
/// </summary>
public class Ship : MonoBehaviour {
	private float maxSpeed;
	private Rigidbody rBody;
	private Vector3 prevVel;
	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
		maxSpeed = 15.0f;
		rBody = gameObject.GetComponent<Rigidbody> ();
	}

	/// <summary>
	/// Update is called once per frame.
	/// The logic of the ships. (They move left until they crash.)
	/// </summary>
	void Update () {
		if (rBody.velocity.x < -maxSpeed) {
			rBody.velocity = new Vector3 (-maxSpeed, rBody.velocity.y);
		} else {
			rBody.AddForce (new Vector3 (-8*rBody.mass, 0, 0));
		}
		prevVel = rBody.velocity;
	}

	/// <summary>
	/// Called when another object starts to collide into it.
	/// If it starts to collide with the Kill Wall, it will just pass through.
	/// </summary>
	/// <param name="coll">Collider of the other object.</param>
	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.name.Equals("KillWall")) {
			Physics.IgnoreCollision (gameObject.GetComponent<Collider> (), coll.gameObject.GetComponent<Collider>());
			rBody.velocity = prevVel;
		}
	}
}
