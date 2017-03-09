using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenLaser : MonoBehaviour {
	private bool activated;
	private float timeUntilSwitch;
	public float maxTime, minTime;
	public Material[] mat;
	public GameObject trap;
	// Use this for initialization
	void Start () {
		activated = true;
		timeUntilSwitch = Random.Range (minTime, maxTime);
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilSwitch -= Time.deltaTime;
		if (timeUntilSwitch <= 0) {
			timeUntilSwitch = Random.Range (minTime, maxTime);
			activated = !activated;
			if (activated) {
				Vector3 pos = gameObject.transform.position;
				gameObject.transform.position = new Vector3 (pos.x, pos.y, pos.z - 1);
				gameObject.GetComponent<MeshRenderer> ().material = mat [0];
			} else if (!activated) {
				Vector3 pos = gameObject.transform.position;
				gameObject.transform.position = new Vector3 (pos.x, pos.y, pos.z + 1);
				gameObject.GetComponent<MeshRenderer> ().material = mat [1];
			}
		}
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.name == "Player") {
			Vector3 pos = gameObject.transform.position;
			GameObject g = Instantiate(trap, gameObject.transform);
			g.transform.position = new Vector3 (pos.x - 4, Camera.main.transform.position.y + 7, 0);
			g.transform.SetParent (null);
			Destroy (gameObject);
		}
	}
}
