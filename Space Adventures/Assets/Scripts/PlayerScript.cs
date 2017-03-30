using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	//float dX, dY;
	public bool isDummy;
	bool onGround,leftWallCheck,rightWallCheck, controlPress, isRight;
	int spacePress;
	float jumpForce, walkVelocity, runVelocity, maxVelocity, refinedJump, accelFactor, personalGravity;
	private float stunTime, itemThrowAngle, holdSpeed, untilDequeue;
	public GameObject itemCarrying;
	private Queue<GameObject> itemsRemoved;
	private Queue<float> escapeFromQueue;
	private LinkedList<float> timeUntilExpired;
	private LinkedList<int> itemUsed;
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
		stunTime = 0;
		accelFactor = 1;
		personalGravity = 1;
		untilDequeue = -1.5f;
		if (isDummy) {
			stunTime = float.MaxValue;
		}
		itemsRemoved = new Queue<GameObject> ();
		escapeFromQueue = new Queue<float> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Moving left and right
		if (stunTime > 0) {
			stunTime -= Time.deltaTime;
			if (stunTime <= 0) {
				stunTime = 0;

			} else {
				return;
			}
		}
		bool leftPress = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
		bool rightPress = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);
		bool upPress = Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);
		bool downPress = Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S);
		controlPress = Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
		Vector3 vel = rBody.velocity;
		//Determine if you're running first.
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			maxVelocity = runVelocity;
		} else {
			maxVelocity = walkVelocity;
		}
		//float dX = vel.x;
		//print (vel.x);
		//Personal Gravity
		rBody.AddForce(0,gloVar.gravity*(1-personalGravity)*rBody.mass,0);
		if (leftPress && !rightPress && !leftWallCheck) {
			isRight = false;
			if (vel.x > -maxVelocity) {
				if (onGround)
					rBody.AddForce (new Vector3 (-10*accelFactor, 0, 0));
				else
					rBody.AddForce (new Vector3 (-4*accelFactor, 0, 0));
			} else if (onGround) {
				if (vel.x * 13 / 14 > -maxVelocity) {
					rBody.velocity = new Vector3 (vel.x * 13 / 14, vel.y, 0);
				} else {
					rBody.velocity = new Vector3 (-maxVelocity, vel.y, 0);
				}
			}
		} else if (rightPress && !leftPress && !rightWallCheck) {
			isRight = true;
			if (vel.x < maxVelocity) {
				if (onGround)
					rBody.AddForce (new Vector3 (10*accelFactor, 0, 0));
				else
					rBody.AddForce (new Vector3 (4*accelFactor, 0, 0));
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
		if (upPress && !downPress) {
			if (vel.y > maxVelocity / 4 && personalGravity * gloVar.gravity < 3) {
				rBody.velocity = new Vector3 (vel.x, vel.y * 13 / 14, 0);
			} else if (personalGravity * gloVar.gravity < 3) {
				rBody.AddForce (0, 1 * accelFactor, 0);
			}
		} else if (downPress && !upPress) {
			if (vel.y < -maxVelocity / 4 && personalGravity * gloVar.gravity < 3) {
				rBody.velocity = new Vector3 (vel.x, vel.y * 13 / 14, 0);
			} else if (personalGravity * gloVar.gravity < 3) {
				rBody.AddForce (0, -1 * accelFactor, 0);
			}
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
		//Item carrying
		if (controlPress && itemCarrying != null) {
			Vector3 newPosition = rBody.position;
			if (upPress && !downPress) {
				if (gloVar.throwingHandler) {
					itemThrowAngle = Mathf.PI / 4; //45 Degrees
				} else {
					itemThrowAngle += Mathf.PI * Time.deltaTime;
					if (itemThrowAngle > Mathf.PI / 2) {
						itemThrowAngle = Mathf.PI / 2;
					}
				}
			} else if (downPress && !upPress) {
				if (gloVar.throwingHandler) {
					itemThrowAngle = -Mathf.PI / 4;
				} else {
					itemThrowAngle -= Mathf.PI * Time.deltaTime;
					if (itemThrowAngle < -Mathf.PI * 5 / 12) {
						itemThrowAngle = -Mathf.PI * 5 / 12;
					}
				}
			} else {
				if (gloVar.throwingHandler) {
					itemThrowAngle = 0;
				}
			}
			Vector3 scale = gameObject.transform.localScale;
			if (isRight) {
				newPosition = new Vector3 (newPosition.x + (scale.x) * Mathf.Cos (itemThrowAngle), newPosition.y + (scale.y) * Mathf.Sin (itemThrowAngle) * 0.8f, newPosition.z); 
			} else {
				newPosition = new Vector3 (newPosition.x - (scale.x) * Mathf.Cos (itemThrowAngle), newPosition.y + (scale.y) * Mathf.Sin (itemThrowAngle) * 0.8f, newPosition.z);
			}
			itemCarrying.GetComponent<Rigidbody>().MovePosition(newPosition);
		} else if (itemCarrying != null) {
			float force = 300.0f;
			if (maxVelocity == runVelocity) {
				force *= 1.5f;
			}
			if (isRight) {
				itemCarrying.GetComponent<ItemBehavior> ().g.GetComponent<Rigidbody> ().AddForce (new Vector3 (Mathf.Cos (itemThrowAngle) * force, Mathf.Sin (itemThrowAngle) * force, 0));
			} else {
				itemCarrying.GetComponent<ItemBehavior> ().g.GetComponent<Rigidbody> ().AddForce (new Vector3 (-Mathf.Cos (itemThrowAngle) * force, Mathf.Sin (itemThrowAngle) * force, 0));
			}
			itemCarrying.GetComponent<ItemBehavior> ().beingCarried = false;
			itemsRemoved.Enqueue (itemCarrying);
			escapeFromQueue.Enqueue (0.1f);
			itemCarrying = null;
		}
		if (escapeFromQueue.ToArray ().Length > 0 && untilDequeue <= -1) {
			untilDequeue = escapeFromQueue.Dequeue ();
		}
		if (untilDequeue > 0) {
			untilDequeue -= Time.deltaTime;
			if (untilDequeue <= 0) {
				GameObject g = itemsRemoved.Dequeue ();
				Physics.IgnoreCollision (g.GetComponent<Collider> (), gameObject.GetComponent<Collider> (), false);
				untilDequeue = -1.5f;
			}
		}
	}


	void OnCollisionStay(Collision collision){
		if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (0, 1, 0)) > 1 / 2) {
			onGround = true;
		}
		if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (1, 0, 0)) > Mathf.Sqrt (3) / 2) {
			leftWallCheck = true;
		}
		if (Vector2.Dot (collision.contacts [0].normal, new Vector3 (-1, 0, 0)) > Mathf.Sqrt (3) / 2) {
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

	public void stun(float time){
		if (stunTime <= 0) {
			stunTime += time;
		}
	}

	public void modifySpeed(float factor, float minExpired, float maxExpired){
		accelFactor *= factor;
		runVelocity *= factor;
		walkVelocity *= factor;
		if (walkVelocity > 18) {
			walkVelocity = 18;
			accelFactor = 3;
			runVelocity = 27;
		}

	}

	public void modifyJump(float factor, float minExpired, float maxExpired){
		jumpForce *= factor;
		if (jumpForce > 750) {
			jumpForce = 750;
		}
	}

	/** Modifies personal gravity. */
	public void modifyGravity(float factor, float minExpired, float maxExpired){
		personalGravity *= factor;
		if (personalGravity < 0.2f) {
			personalGravity = 0.2f;
		}
	}

	public bool getControlPress(){
		return controlPress;
	}
}
