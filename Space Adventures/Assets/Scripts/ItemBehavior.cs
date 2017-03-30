using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour {
	public GameObject grav;
	public GameObject g;
	public float factor;
	public int self;
	public bool beingCarried;
	public Vector3 prevPosition;
	// Use this for initialization
	void Start () {
		g = Instantiate (grav);
		g.GetComponent<Rigidbody> ().velocity = gameObject.GetComponent<Rigidbody> ().velocity;
		g.GetComponent<BoxCollider> ().size = gameObject.transform.localScale;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			Physics.IgnoreCollision (g.GetComponent<Collider> (), players [i].GetComponent<Collider> ());
		}
		g.transform.position = gameObject.transform.position;
		beingCarried = false;
		prevPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!beingCarried) {
			gameObject.transform.position = g.GetComponent<Rigidbody> ().transform.position;
		} else {
			g.GetComponent<Rigidbody> ().transform.position = gameObject.transform.position;
			g.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.name.Length >= 6 && coll.gameObject.name.Substring(0,6) == "Player") {
			PlayerScript p = coll.gameObject.GetComponent<PlayerScript> ();
			if (!p.getControlPress() || (p.itemCarrying) != null ) {
				if (self == 0) {
					p.modifySpeed (factor);
				} else if (self == 1) {
					p.modifyJump (factor);
				} else if (self == 2) {
					p.modifyGravity (factor);
				}
				Destroy (g);
				Destroy (gameObject);
			} else {
				p.itemCarrying = gameObject;
				Physics.IgnoreCollision (gameObject.GetComponent<Collider> (), coll.gameObject.GetComponent<Collider> ());
				beingCarried = true;
			}
		} 
	}
}
