using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour {
	public GameObject grav;
	GameObject g;
	public float factor;
	public int self;
	// Use this for initialization
	void Start () {
		g = Instantiate (grav);
		g.GetComponent<BoxCollider> ().size = gameObject.transform.localScale;
		Vector3 pos = gameObject.transform.position;
		g.transform.position = new Vector3(pos.x*0.5f, pos.y, pos.z);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = g.GetComponent<Rigidbody>().transform.position;
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.name.Length >= 6 && coll.gameObject.name.Substring(0,6) == "Player") {
			PlayerScript p = coll.gameObject.GetComponent<PlayerScript> ();
			if (self == 0) {
				p.modifySpeed (factor);
			} else if (self == 1) {
				p.modifyJump (factor);
			} else if (self == 2) {
				p.modifyGravity (factor);
			}
			Destroy (g);
			Destroy (gameObject);
		}
	}
}
