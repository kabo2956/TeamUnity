using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A laser that the player will have to dodge.
/// </summary>
public class Laser : MonoBehaviour {
	private bool activated;
	private float timeUntilSwitch, realHeight;
	/// <summary>
	/// The maximum time (seconds) it takes for the laser to toggle on and off.
	/// </summary>
	public float maxTime;
	/// <summary>
	/// The minimum time (seconds) it takes for the laser to toggle on and off.
	/// </summary>
	public float minTime;
	/// <summary>
	/// The materials of the lasers. (GREEN MATERIAL, RED MATERIAL, BLUE MATERIAL)
	/// </summary>
	public Material[] mat;
	/// <summary>
	/// The trap spawned by the green laser. (A solid object)
	/// </summary>
	public GameObject trap;
	/// <summary>
	/// The color of the laser.
	/// Green = 0
	/// Red = 1
	/// Blue = 2
	/// </summary>
	public int color; //Green: 0, Red: 1, Blue: 2
	/// <summary>
	/// If the laser is red, the max power the laser pushes you back by.
	/// If the laser is blue, the maximum amount of time the laser stuns the player.
	/// </summary>
	public float maxPower;
	/// <summary>
	/// If the laser is red, the min power the laser pushes you back by.
	/// If the laser is blue, the minimum amount of time the laser stuns the player.
	/// </summary>
	public float minPower;

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
