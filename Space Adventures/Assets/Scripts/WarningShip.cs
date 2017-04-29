using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningShip : MonoBehaviour {
	private float warningTime;
	private float animationTime, nextFrame;
	private int animationFrame;
	private bool active;
	public Sprite[] animationCycle;
	public GameObject ship;
	private GameObject s;
	/// <summary>
	/// Creates a warning for the ship.
	/// Use this for initialization
	/// </summary>
	void Start () {
		warningTime = Random.Range (0.25f, 1.0f);
		animationTime = gloVar.animationSpeed * 4.5f;
		nextFrame = animationTime;
		active = false;
	}

	/// <summary>
	/// Spawning a Warning for the Ships.
	/// </summary>
	public void Spawn (){
		active = true;
		warningTime = Random.Range (0.25f, 1.0f);
		animationFrame = 0;
		nextFrame = animationTime;
		if (s != null) {
			Destroy (s);
		}
		s = null;
	}

	/// <summary>
	///  Update is called once per frame.
	///  The logic of the warning. Controls the animation and the spawning of the ship.
	/// </summary>
	void Update () {
		if (active) {
			Camera cam = Camera.main;
			transform.position = new Vector3 (cam.transform.position.x + cam.orthographicSize * Screen.width / Screen.height - 0.1f, transform.position.y, transform.position.z);
			warningTime -= Time.deltaTime;
			nextFrame -= Time.deltaTime;
			if (warningTime <= 0) {
				//Spawn a ship!
				Vector3 pos = gameObject.transform.position;
				s = Instantiate (ship, gameObject.transform);
				s.transform.position = new Vector3 (pos.x + 2, pos.y, pos.z + 1);
				s.transform.SetParent (null);
				active = false;
				transform.position = new Vector3 (-999, -999, -999);
				//Destroy (gameObject);
			} 
			if (nextFrame <= 0) {
				nextFrame = animationTime;
				animationFrame = (animationFrame + 1) % 4;
				if (animationFrame != 3) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = animationCycle [animationFrame];
				} else {
					gameObject.GetComponent<SpriteRenderer> ().sprite = animationCycle [1];
				}
			}
		}
	}
}
