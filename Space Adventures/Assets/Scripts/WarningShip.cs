using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningShip : MonoBehaviour {
	private float warningTime;
	private float animationTime, nextFrame;
	private int animationFrame;
	public Sprite[] animationCycle;
	public GameObject ship;
	// Use this for initialization
	void Start () {
		warningTime = Random.Range (0.25f, 1.0f);
		animationTime = gloVar.animationSpeed * 4.5f;
		nextFrame = animationTime;
	}
	
	// Update is called once per frame
	void Update () {
		Camera cam = Camera.main;
		transform.position = new Vector3 (cam.transform.position.x + 9.2f, transform.position.y, transform.position.z);
		warningTime -= Time.deltaTime;
		nextFrame -= Time.deltaTime;
		if (warningTime <= 0) {
			//Spawn a ship!
			Vector3 pos = gameObject.transform.position;
			GameObject g = Instantiate(ship, gameObject.transform);
			g.transform.position = new Vector3 (pos.x + 2, pos.y, pos.z+1);
			g.transform.SetParent (null);
			Destroy (gameObject);
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
