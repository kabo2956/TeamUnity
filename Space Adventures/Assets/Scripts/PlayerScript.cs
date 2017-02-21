using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	//float dX, dY;
	bool onGround;
	int spacePress;
	float jumpForce;
	Rigidbody2D rBody;
	// Use this for initialization
	void Start () {
		//dX = 0;
		//dY = 0;
		Physics2D.gravity = new Vector2 (0, -gloVar.gravity);
		onGround = false;
		spacePress = 0;
		rBody = GetComponent<Rigidbody2D> ();
		jumpForce = 200;
	}
	
	// Update is called once per frame
	void Update () {
		//There's a possibility that the person may hit space when the player is close to the ground. Let's account for that.
		if (Input.GetKey (KeyCode.Space)) {
			spacePress += 1;
		} else {
			spacePress = 0;
		}
		if (onGround && spacePress > 0 && spacePress <= gloVar.isPressed) {
			rBody.AddForce(new Vector2(0, jumpForce));
			onGround = false;
			spacePress = 0;
		}
		//onGround = false;
	}

	void OnCollisionStay2D(Collision2D collision){
		if (collision.enabled) {
			if (collision.contacts [0].point.y > collision.collider.bounds.center.y) {
				string name = collision.gameObject.name;
				if (name == "Solid") {
					onGround = true;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision){
		if (collision.enabled) {
			if (collision.contacts [0].point.y > collision.collider.bounds.center.y) {
				string name = collision.gameObject.name;
				if (name == "Solid") {
					onGround = false;
				}
			}
		}
	}
}
