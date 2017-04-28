using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A laser that the player will have to dodge.
/// </summary>
public class Laser : MonoBehaviour {
	private bool activated;
	private float timeUntilSwitch, realHeight;
	public float maxTime, minTime;
	public Material[] mat;
	public GameObject trap;
	public int color; //Green: 0, Red: 1, Blue: 2
	public float maxPower, minPower;

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
		activated = true;
		realHeight = gameObject.GetComponent<CapsuleCollider> ().height;
		timeUntilSwitch = Random.Range (minTime, maxTime);
	}

	/// <summary>
	/// Update is called once per frame
	/// Controls the logic of the laser, and whether or not it is actually on.
	/// </summary>
	void Update () {
		timeUntilSwitch -= Time.deltaTime;
		if (timeUntilSwitch <= 0) {
			timeUntilSwitch = Random.Range (minTime, maxTime);
			activated = !activated;
			if (activated) {
				gameObject.GetComponent<CapsuleCollider> ().height = realHeight;
				gameObject.GetComponent<MeshRenderer> ().material = mat [0];
			} else if (!activated) {
				gameObject.GetComponent<CapsuleCollider> ().height = 0;
				gameObject.GetComponent<MeshRenderer> ().material = mat [1];
			}
		}
	}

	/// <summary>
	/// If something collides with it, this occurs.
	/// If the player collides with it, and it is on, something happens:
	/// 	Green (Color 0): A solid object is spawned to the left of the screen.
	/// 	Red (Color 1): The player is blasted to the left.
	/// 	Blue (Color 2: The player is stunned, and can't be controlled.
	/// </summary>
	/// <param name="coll">The collider of the other object.</param>
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
