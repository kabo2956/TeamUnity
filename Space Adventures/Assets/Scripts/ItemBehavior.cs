using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The behavior of the items in the game.
/// </summary>
public class ItemBehavior : MonoBehaviour {
	/// <summary>
	/// The object that has gravity and collision.
	/// </summary>
	public GameObject grav;
	/// <summary>
	/// The instantiated grav.
	/// </summary>
	public GameObject g;
	/// <summary>
	/// How powerful the item is.
	/// </summary>
	public float factor;
	/// <summary>
	/// The type of object this is.
	/// 0: Speed Modifier
	/// 1: Jump Modifier
	/// 2: Gravity Modifier
	/// </summary>
	public int self;
	/// <summary>
	/// How many solid objects the item is touching when it is being carried to prevent movement.
	/// </summary>
	public int notAllowedToGoHere;
	/// <summary>
	/// If the item is being carried.
	/// </summary>
	public bool beingCarried;
	/// <summary>
	/// The previous position of the item.
	/// </summary>
	public Vector3 prevPosition;
	/// <summary>
	/// How the minimum amount of time that the item affects the player.
	/// </summary>
	public float minExpired;
	/// <summary>
	/// How the maximum amount of time that the item affects the player.
	/// </summary>
	public float maxExpired;
	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
		g = Instantiate (grav);
		g.GetComponent<Rigidbody> ().velocity = gameObject.GetComponent<Rigidbody> ().velocity;
		g.GetComponent<BoxCollider> ().size = gameObject.transform.localScale;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			Physics.IgnoreCollision (g.GetComponent<Collider> (), players [i].GetComponent<Collider> ());
		}
		g.transform.position = gameObject.transform.position;
		beingCarried = false;
		prevPosition = gameObject.transform.position;
	}

	/// <summary>
	/// Update positions for both the Dummy Object and the other object.
	/// </summary>
	void Update () {
		if (!beingCarried) {
			gameObject.transform.position = g.GetComponent<Rigidbody> ().transform.position;
			if (transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height - 1 ||
			    transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize - 1) {
				Destroy (g);
				Destroy (gameObject);
			}
		} else {
			g.GetComponent<Rigidbody> ().transform.position = gameObject.transform.position;
			g.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		}
	}

	/// <summary>
	/// When colliding with a player, decide on if it affects the player or if it is being carried instead.
	/// Otherwise, if it is being carried, make sure it can't go into Solid objects.
	/// </summary>
	/// <param name="coll">Collider for the other object.</param>
	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.name.Length >= 6 && coll.gameObject.tag == "Player") {
			PlayerScript p = coll.gameObject.GetComponent<PlayerScript> ();
			if (!p.getControlPress () || (p.itemCarrying) != null) {
				if (self == 0) {
					p.modifySpeed (factor, minExpired, maxExpired);
				} else if (self == 1) {
					p.modifyJump (factor, minExpired, maxExpired);
				} else if (self == 2) {
					p.modifyGravity (factor, minExpired, maxExpired);
				}
				Destroy (g);
				Destroy (gameObject);
			} else {
				p.itemCarrying = gameObject;
				Physics.IgnoreCollision (gameObject.GetComponent<Collider> (), coll.gameObject.GetComponent<Collider> ());
				beingCarried = true;
			}
		} else if (beingCarried && coll.gameObject.tag == "Solid") {
			notAllowedToGoHere++;
		}
	}

	/// <summary>
	/// If you are exiting a Solid object, then you can let go of the item. Otherwise, do nothing.
	/// </summary>
	/// <param name="coll">Collider for the other object.</param>
	void OnTriggerExit(Collider coll){
		if (beingCarried && coll.gameObject.tag == "Solid" && notAllowedToGoHere > 0) {
			notAllowedToGoHere--;
			if (notAllowedToGoHere < 0)
				notAllowedToGoHere = 0;
		}
	}
}
