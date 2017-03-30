using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public GameObject[] items;
	public float minSpeedX, maxSpeedX;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3 (Random.Range (minSpeedX, maxSpeedX), Random.Range (-5, 0), 0);
		gameObject.GetComponent<Rigidbody> ().drag = Random.Range (0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
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
		Destroy (gameObject);
	}
}
