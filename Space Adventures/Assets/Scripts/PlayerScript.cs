using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	//float dX, dY;
	bool onGround,leftWallCheck,rightWallCheck;
	int spacePress;
	float jumpForce, walkVelocity,refinedJump;
	Rigidbody2D rBody;
	// Use this for initialization
	void Start () {
		//dX = 0;
		//dY = 0;
		Physics2D.gravity = new Vector2 (0, -gloVar.gravity);
		onGround = false;
		spacePress = 0;
		rBody = GetComponent<Rigidbody2D> ();
		jumpForce = 250;
		walkVelocity = 6;
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
		Vector2 vel = rBody.velocity;
		//float dX = vel.x;
		if (leftPress && !rightPress && !leftWallCheck && (!rightWallCheck || onGround)) {
			if (vel.x > -walkVelocity) {
				if (onGround)
					rBody.AddForce (new Vector2 (-10, 0));
				else
					rBody.AddForce (new Vector2 (-4, 0));
			} else {
				
			}
		} else if (rightPress && !leftPress && !rightWallCheck && (!leftWallCheck || onGround)) {
			if (vel.x < walkVelocity) {
				if (onGround)
					rBody.AddForce (new Vector2 (10, 0));
				else
					rBody.AddForce (new Vector2 (4, 0));
			}
		} else if (onGround){
			rBody.velocity = new Vector2 (vel.x*13/14, vel.y);
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
				rBody.AddForce (new Vector2 (-1, -Physics2D.gravity.y * 1 / 3));
				if (spacePress > 0 && spacePress <= gloVar.isPressed) {
					spacePress = gloVar.isPressed + 1;
					refinedJump = 0;
					if (upPress)
						rBody.AddForce (new Vector2 (jumpForce * 0.4f, jumpForce * 1.3f));
					else if (downPress)
						rBody.AddForce (new Vector2 (jumpForce * 0.8f, -jumpForce * 0.5f));
					else if (rightPress)
						rBody.AddForce (new Vector2 (jumpForce * 1.5f, jumpForce * 0.3f));
					else
						rBody.AddForce (new Vector2 (jumpForce * 1.0f, jumpForce * 0.8f));
				}
			} else if (rightWallCheck && rightPress) {
				rBody.AddForce (new Vector2 (1, -Physics2D.gravity.y * 1 / 3));
				if (spacePress > 0 && spacePress <= gloVar.isPressed) {
					spacePress = gloVar.isPressed + 1;
					refinedJump = 0;
					if (upPress)
						rBody.AddForce (new Vector2 (-jumpForce * 0.4f, jumpForce * 1.3f));
					else if (downPress)
						rBody.AddForce (new Vector2 (-jumpForce * 0.8f, -jumpForce * 0.5f));
					else if (leftPress)
						rBody.AddForce (new Vector2 (-jumpForce * 1.5f, jumpForce * 0.3f));
					else
						rBody.AddForce (new Vector2 (-jumpForce * 1.0f, jumpForce * 0.8f));
				}
			}
		}
		if (onGround && spacePress > 0 && spacePress <= gloVar.isPressed) {
			rBody.AddForce (new Vector2 (0, jumpForce));
			onGround = false;
			spacePress = gloVar.isPressed + 1;
			refinedJump = 0;
		} else if (refinedJump >= 0 && !onGround && vel.y > 0) {
			refinedJump += Time.deltaTime;
			if (refinedJump > 0.25)
				refinedJump = -1;
			if (spacePress > 0)
				rBody.AddForce (new Vector2 (0, -Physics2D.gravity.y * 1.0f));
			else
				rBody.AddForce (new Vector2 (0, Physics2D.gravity.y * 0.4f));
		}
	}

	void OnCollisionStay2D(Collision2D collision){
		if (collision.enabled && collision.gameObject.tag == "Solid") {
			if (Vector2.Dot (collision.contacts [0].normal, new Vector2 (0, 1)) > Mathf.Sqrt(2)/2) {
				onGround = true;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector2 (1, 0)) > Mathf.Sqrt(2)/2) {
				leftWallCheck = true;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector2 (-1, 0)) > Mathf.Sqrt(2)/2) {
				rightWallCheck = true;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision){
		if (collision.enabled && collision.gameObject.tag == "Solid") {
			if (Vector2.Dot (collision.contacts [0].normal, new Vector2 (0, 1)) > Mathf.Sqrt(2)/2) {
				onGround = false;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector2 (1, 0)) > Mathf.Sqrt(2)/2) {
				leftWallCheck = false;
			}
			if (Vector2.Dot (collision.contacts [0].normal, new Vector2 (-1, 0)) > Mathf.Sqrt(2)/2) {
				rightWallCheck = false;
			}
		}
	}
}
