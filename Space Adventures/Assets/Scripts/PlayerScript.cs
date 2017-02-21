using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	float dX, dY;
	// Use this for initialization
	void Start () {
		dX = 0;
		dY = 0;
		Physics2D.gravity = new Vector2 (0, -gloVar.gravity);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider2D other){
		
	}
}
