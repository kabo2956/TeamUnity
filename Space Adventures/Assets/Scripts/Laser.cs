using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	private bool activated;
	private float timeUntilSwitch, realHeight;
	public float maxTime, minTime;
	public Material[] mat;
	public GameObject trap;
	public int color; //Green: 0, Red: 1, Blue: 2
	public float maxPower, minPower;
	// Use this for initialization
	void Start () {
		activated = true;
		realHeight = gameObject.GetComponent<CapsuleCollider> ().height;
		timeUntilSwitch = Random.Range (minTime, maxTime);
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilSwitch -= Time.deltaTime;
		if (timeUntilSwitch <= 0) {
			timeUntilSwitch = Random.Range (minTime, maxTime);
			activated = !activated;
			if (activated) {
				gameObject.GetComponent<CapsuleCollider> ().height = realHeight;
				gameObject.GetComponent<MeshRenderer> ().material = mat [0];
				/*Vector3 pos = gameObject.transform.position;
				gameObject.transform.position = new Vector3 (pos.x, pos.y, pos.z - 0.2f);
				*/
			} else if (!activated) {
				gameObject.GetComponent<CapsuleCollider> ().height = 0;
				gameObject.GetComponent<MeshRenderer> ().material = mat [1];
				/*
				Vector3 pos = gameObject.transform.position;
				gameObject.transform.position = new Vector3 (pos.x, pos.y, pos.z + 0.2f);
				*/
			}
		}
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Player") {
			if (color == 0) {
				Vector3 pos = gameObject.transform.position;
				GameObject g = Instantiate (trap, gameObject.transform);
				g.transform.position = new Vector3 (pos.x - 4, Camera.main.transform.position.y + 7, 0);
				g.transform.SetParent (null);
			} else if (color == 2) {
				coll.gameObject.GetComponent<PlayerScript> ().stun (Random.Range (minPower, maxPower));
			} else {
				coll.gameObject.GetComponent<Rigidbody> ().AddForce (-Random.Range (minPower, maxPower), -8, 0);
			}
			Destroy (gameObject);
		}
	}
}
