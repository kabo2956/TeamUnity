﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	//float dX, dY;
	bool onGround,leftWallCheck,rightWallCheck;
	int spacePress;
	float jumpForce, walkVelocity, runVelocity, maxVelocity, refinedJump;
	Rigidbody rBody;
	// Use this for initialization
	void Start () {
		//dX = 0;
		//dY = 0;
		Physics.gravity = new Vector2 (0, -gloVar.gravity);
		onGround = false;
		spacePress = 0;
		rBody = GetComponent<Rigidbody> ();
		jumpForce = 250;
		walkVelocity = 6;
		runVelocity = walkVelocity * 1.5f;
		maxVelocity = walkVelocity;
		refinedJump = -1;
		leftWallCheck = false;
		rightWallCheck = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Moving left and right
		bool leftPress = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
		bool rightPress = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);
		bool upPress = Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);
		bool downPress = Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S);
		Vector3 vel = rBody.velocity;
		//Determine if you're running first.
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			maxVelocity = runVelocity;
		} else {
			maxVelocity = walkVelocity;
		}
		//float dX = vel.x;
		//print (vel.x);
		if (leftPress && !rightPress && !leftWallCheck) {
			if (vel.x > -maxVelocity) {
				if (onGround)
					rBody.AddForce (new Vector3 (-10, 0, 0));
				else
					rBody.AddForce (new Vector3 (-4, 0, 0));
			} else if (onGround) {
				if (vel.x * 13 / 14 > -maxVelocity) {
					rBody.velocity = new Vector3 (vel.x * 13 / 14, vel.y, 0);
				} else {
					rBody.velocity = new Vector3 (-maxVelocity, vel.y, 0);
				}
			}
		} else if (rightPress && !leftPress && !rightWallCheck) {
			if (vel.x < maxVelocity) {
				if (onGround)
					rBody.AddForce (new Vector3 (10, 0, 0));
				else
					rBody.AddForce (new Vector3 (4, 0, 0));
			} else if (onGround) {
				if (vel.x * 13 / 14 < maxVelocity) {
					rBody.velocity = new Vector3 (vel.x * 13 / 14, vel.y, 0);
				} else {
					rBody.velocity = new Vector3 (maxVelocity, vel.y, 0);
				}
			}
		} else if (onGround){
			rBody.velocity = new Vector3 (vel.x*13/14, vel.y, 0);
		}
		//There's a possibility that the person may hit space when the player is close to the ground. Let's account for that.
		if (Input.GetKey (KeyCode.Space)) {
			spacePress += 1;
        } else {
			spacePress = 0;
		}
		//Wall slide
		if (!onGround) {
			if (leftWallCheck && leftPress) {
				rBody.AddForce (new Vector3 (-1, -Physics2D.gravity.y * 1 / 3, 0));
				if (spacePress > 0 && spacePress <= gloVar.isPressed) {
					spacePress = gloVar.isPressed + 1;
					refinedJump = 0;
					if (upPress)
						rBody.AddForce (new Vector3 (jumpForce * 0.4f, jumpForce * 1.3f, 0));
					else if (downPress)
						rBody.AddForce (new Vector3 (jumpForce * 0.8f, -jumpForce * 0.5f, 0));
					else
						rBody.AddForce (new Vector3 (jumpForce * 1.0f, jumpForce * 0.8f, 0));
				}
			} else if (rightWallCheck && rightPress) {
				rBody.AddForce (new Vector3 (1, -Physics2D.gravity.y * 1 / 3));
				if (spacePress > 0 && spacePress <= gloVar.isPressed) {
					spacePress = gloVar.isPressed + 1;
					refinedJump = 0;
					if (upPress)
						rBody.AddForce (new Vector3 (-jumpForce * 0.4f, jumpForce * 1.3f, 0));
					else if (downPress)
						rBody.AddForce (new Vector3 (-jumpForce * 0.8f, -jumpForce * 0.5f, 0));
					else
						rBody.AddForce (new Vector3 (-jumpForce * 1.0f, jumpForce * 0.8f, 0));
				}
			} else {
				leftWallCheck = false;
				rightWallCheck = false;
			}
		}
		if (onGround && spacePress > 0 && spacePress <= gloVar.isPressed) {
			rBody.AddForce (new Vector3 (0, jumpForce, 0));
			onGround = false;
			spacePress = gloVar.isPressed + 1;
			refinedJump = 0;
		} else if (refinedJump >= 0 && !onGround && vel.y > 0) {
			refinedJump += Time.deltaTime;
			if (refinedJump > 0.25)
				refinedJump = -1;
			if (spacePress > 0)
				rBody.AddForce (new Vector3 (0, -Physics2D.gravity.y * 1.0f, 0));
			else
				rBody.AddForce (new Vector3 (0, Physics2D.gravity.y * 0.4f, 0));
		}
	}


	void OnCollisionStay(Collision collision){
			if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (0, 1, 0)) > 1/2) {
				onGround = true;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (1, 0, 0)) > Mathf.Sqrt(3)/2) {
				leftWallCheck = true;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (-1, 0, 0)) > Mathf.Sqrt(3)/2) {
				rightWallCheck = true;
			}
	}


	void OnCollisionExit(Collision collision){
		if (collision.contacts.Length > 0) {
			if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (0, 1, 0)) > 1 / 2) {
				onGround = false;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (1, 0, 0)) > Mathf.Sqrt (3) / 2) {
				leftWallCheck = false;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (-1, 0, 0)) > Mathf.Sqrt (3) / 2) {
				rightWallCheck = false;
			}
		} else {
			onGround = false;
			leftWallCheck = false;
			rightWallCheck = false;
		}
	}
}
