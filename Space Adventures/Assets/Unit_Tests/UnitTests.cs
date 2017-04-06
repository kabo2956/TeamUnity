﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitTests : MonoBehaviour {
	public GameObject player;
	private float time;
	private float prevJump, prevWalk, prevGrav;
	private bool[] testedYet;
	// Use this for initialization
	void Start () {
		Assert.AreEqual (true, true);
		player = Instantiate (player);
		testedYet = new bool[100];
		testedYet [0] = false;
	}
	
	// Update is called once per frame
	void Update () {
		Physics.gravity = new Vector3(0,0,0);
		player.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		if (time == 0 && !testedYet[0]) {
			testedYet [0] = true;
			InitialAssertions ();
			testedYet [1] = false;
		} else if (time >= 0.025 && !testedYet [1]) {
			testedYet [1] = true;
			PlayerPowerupNonPermanenceAssertion ();
			PlayerPowerupLimitCheck ();
			testedYet [2] = false;
			time = -Time.deltaTime;
		} else if (time >= 0.025 && !testedYet [2]) {
			testedYet [2] = true;
			PlayerPowerupNonPermanenceAssertion ();
		}
		time += Time.deltaTime;
	}

	void InitialAssertions () {
		print ("Testing that the player initialized correctly.");
		PlayerInitAssertions ();
	}

	void PlayerInitAssertions () {
		Assert.AreEqual (6, (int)(player.GetComponent<PlayerScript> ().getValue ("walkVelocity")));
		print ("Testing that increasing the speed of the player works.");
		PlayerModifySpeedCheckUp ();
		print ("Same for decreasing the speed.");
		PlayerModifySpeedCheckDown ();
		print ("Testing that increasing the jumping power of the player works.");
		PlayerModifyJumpCheckUp ();
		print ("Same for decreasing the jump power.");
		PlayerModifyJumpCheckDown ();
		print ("Testing that increasing the gravity of the player works.");
		PlayerModifyGravityCheckUp ();
		print ("Testing that decreasing the gravity of the player works.");
		PlayerModifyGravityCheckDown ();
	}

	void PlayerModifySpeedCheckUp() {
		float prevWalkVel = player.GetComponent<PlayerScript> ().getValue ("walkVelocity");
		float prevRunVel = player.GetComponent<PlayerScript> ().getValue ("runVelocity");
		float prevAccelFactor = player.GetComponent<PlayerScript> ().getValue ("accelFactor");
		player.GetComponent<PlayerScript> ().modifySpeed (1.1f, 0, 0);
		Assert.AreApproximatelyEqual (prevWalkVel * 1.1f, player.GetComponent<PlayerScript> ().getValue ("walkVelocity"));
		Assert.AreApproximatelyEqual (prevRunVel * 1.1f, player.GetComponent<PlayerScript> ().getValue ("runVelocity"));
		Assert.AreApproximatelyEqual (prevAccelFactor * 1.1f, player.GetComponent<PlayerScript> ().getValue ("accelFactor"));
	}

	void PlayerModifySpeedCheckDown() {
		float prevWalkVel = player.GetComponent<PlayerScript> ().getValue ("walkVelocity");
		prevWalk = prevWalkVel;
		float prevRunVel = player.GetComponent<PlayerScript> ().getValue ("runVelocity");
		float prevAccelFactor = player.GetComponent<PlayerScript> ().getValue ("accelFactor");
		player.GetComponent<PlayerScript> ().modifySpeed (0.8f, 0.01f, 0.015f);
		Assert.AreApproximatelyEqual (prevWalkVel * 0.8f, player.GetComponent<PlayerScript> ().getValue ("walkVelocity"));
		Assert.AreApproximatelyEqual (prevRunVel * 0.8f, player.GetComponent<PlayerScript> ().getValue ("runVelocity"));
		Assert.AreApproximatelyEqual (prevAccelFactor * 0.8f, player.GetComponent<PlayerScript> ().getValue ("accelFactor"));
	}

	void PlayerModifyJumpCheckUp() {
		float prevJumpFor = player.GetComponent<PlayerScript> ().getValue ("jumpForce");
		player.GetComponent<PlayerScript> ().modifyJump (1.1f, 0, 0);
		Assert.AreApproximatelyEqual (prevJumpFor * 1.1f, player.GetComponent<PlayerScript> ().getValue ("jumpForce"));
	}

	void PlayerModifyJumpCheckDown() {
		prevJump = player.GetComponent<PlayerScript> ().getValue ("jumpForce");
		player.GetComponent<PlayerScript> ().modifyJump (0.8f, 0.01f, 0.015f);
		Assert.AreApproximatelyEqual (prevJump * 0.8f, player.GetComponent<PlayerScript> ().getValue ("jumpForce"));
	}

	void PlayerModifyGravityCheckUp() {
		float prevGravi = player.GetComponent<PlayerScript> ().getValue ("personalGravity");
		player.GetComponent<PlayerScript> ().modifyGravity (1.1f, 0, 0);
		Assert.AreApproximatelyEqual (prevGravi * 1.1f, player.GetComponent<PlayerScript> ().getValue ("personalGravity"));
	}

	void PlayerModifyGravityCheckDown() {
		prevGrav = player.GetComponent<PlayerScript> ().getValue ("personalGravity");
		player.GetComponent<PlayerScript> ().modifyGravity (0.8f, 0.01f, 0.015f);
		Assert.AreApproximatelyEqual (prevGrav * 0.8f, player.GetComponent<PlayerScript> ().getValue ("personalGravity"));
	}

	void PlayerPowerupNonPermanenceAssertion() {
		print ("Testing to see if non-permanent speed changes revert back to normal.");
		Assert.AreApproximatelyEqual (prevWalk, player.GetComponent<PlayerScript> ().getValue ("walkVelocity"));
		print ("Testing to see if non-permanent jump changes revert back to normal.");
		Assert.AreApproximatelyEqual (prevJump, player.GetComponent<PlayerScript> ().getValue ("jumpForce"));
		print ("Testing to see if non-permanent gravity changes revert back to normal.");
		Assert.AreApproximatelyEqual (prevGrav, player.GetComponent<PlayerScript> ().getValue ("personalGravity"));
	}

	void PlayerPowerupLimitCheck() {
		print ("Testing the lower limit of speed changes. (Should do nothing)");
		prevWalk = player.GetComponent<PlayerScript> ().getValue ("walkVelocity");
		player.GetComponent<PlayerScript> ().modifySpeed (0, 0, 0);
		Assert.AreApproximatelyEqual (prevWalk, player.GetComponent<PlayerScript> ().getValue ("walkVelocity"));
		print ("Testing the upper limit of speed changes. (Should go to 18)");
		player.GetComponent<PlayerScript> ().modifySpeed (999, 0.01f, 0.015f);
		Assert.AreApproximatelyEqual (18, player.GetComponent<PlayerScript> ().getValue ("walkVelocity"));
		print ("Testing the lower limit of jump changes. (Should do nothing)");
		prevJump = player.GetComponent<PlayerScript> ().getValue ("jumpForce");
		player.GetComponent<PlayerScript> ().modifyJump (0, 0, 0);
		Assert.AreApproximatelyEqual (prevJump, player.GetComponent<PlayerScript> ().getValue ("jumpForce"));
		print ("Testing the upper limit of jump changes. (Should go to 750)");
		player.GetComponent<PlayerScript> ().modifyJump (999, 0.01f, 0.015f);
		Assert.AreApproximatelyEqual (750, player.GetComponent<PlayerScript> ().getValue ("jumpForce"));
		print ("Testing the lower limit of gravity changes. (Should go to 0.2)");
		prevGrav = player.GetComponent<PlayerScript> ().getValue ("personalGravity");
		player.GetComponent<PlayerScript> ().modifyGravity (0, 0.01f, 0.015f);
		Assert.AreApproximatelyEqual (0.2f, player.GetComponent<PlayerScript> ().getValue ("personalGravity"));
	}
}