using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public GameObject[] items;
	public float minSpeedX, maxSpeedX;
	public float momentsUntilDestruction;
	private bool impact;
	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
		gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range (minSpeedX, maxSpeedX), Random.Range (-5, 0), 0);
		gameObject.GetComponent<Rigidbody> ().drag = Random.Range (0, 1);
		momentsUntilDestruction = -1;
		impact = false;
	}
	
	// Update is called once per frame
	/// <summary>
	/// Update is called once per frame. 
	/// If it has been "destroyed", it counts down a few seconds before actually destroying itself to keep particles going.
	/// </summary>
	void Update () {
		if (momentsUntilDestruction > 0) {
			momentsUntilDestruction -= Time.deltaTime;
			if (momentsUntilDestruction <= 0) {
				Destroy (gameObject);
			}
		}
		if ((transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height - 3 ||
			transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize - 3) && !impact) {
			momentsUntilDestruction = 3;
			impact = true;
		}
	}

	/// <summary>
	/// If it collides with anything, it explodes.
	/// </summary>
	/// <param name="coll">The collision that occurs.</param>
	void OnCollisionEnter (Collision coll){
		int itemsBlastedOut = Random.Range (1, 6);
		for (int i = 0; i < itemsBlastedOut; i++) {
			int value = Random.Range(0,items.Length);
			GameObject g = Instantiate (items [value]);
			g.transform.SetParent (null);
			float offset = Random.Range (-1.0f, 1.0f);
			g.transform.position = new Vector3(gameObject.transform.position.x+offset, gameObject.transform.position.y+Random.Range(-1.0f, 1.0f),0);
			if (offset >= 0) {
				g.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range(1,5), Random.Range (1, 5), 0);
			} else {
				g.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range(-1,-5), Random.Range (1, 5), 0);
			}
		}
		gameObject.transform.position = new Vector3 (0, -999, 0);
		momentsUntilDestruction = 3;
		impact = true;
	}
}
