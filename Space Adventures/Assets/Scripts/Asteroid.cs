using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public GameObject[] items;
	public float minSpeedX, maxSpeedX;
	public float momentsUntilDestruction;
	//public GameObject particleEmitt;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3 (Random.Range (minSpeedX, maxSpeedX), Random.Range (-5, 0), 0);
		gameObject.GetComponent<Rigidbody> ().drag = Random.Range (0, 1);
		momentsUntilDestruction = -1;
	}
	
	// Update is called once per frame
	void Update () {
		//particleEmitt.transform.position = gameObject.transform.position;
		if (momentsUntilDestruction > 0) {
			momentsUntilDestruction -= Time.deltaTime;
			if (momentsUntilDestruction <= 0) {
				Destroy (gameObject);
			}
		}
	}

	void OnCollisionEnter (Collision coll){
		int itemsBlastedOut = Random.Range (1, 6);
		for (int i = 0; i < itemsBlastedOut; i++) {
			int value = Random.Range(0,items.Length);
			GameObject g = Instantiate (items [value]);
			g.transform.SetParent (null);
			float offset = Random.Range (-1.0f, 1.0f);
			g.transform.position = new Vector3(gameObject.transform.position.x+offset, gameObject.transform.position.y+Random.Range(-1.0f, 1.0f),0);
			if (offset >= 0) {
				g.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range(1,5), Random.Range (1, 5), 0);
			} else {
				g.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range(-1,-5), Random.Range (1, 5), 0);
			}
		}
		gameObject.transform.position = new Vector3 (0, 100, 0);
		momentsUntilDestruction = 3;
	}
}
