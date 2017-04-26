using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartGameScript : MonoBehaviour {
	bool isHost, foundPlayerObjects;
	NetworkManager network;
	// Use this for initialization
	void Start () {
		RectTransform t = gameObject.GetComponent<RectTransform> ();
		t.position = new Vector3 (Screen.width-t.rect.width/2, Screen.height-t.rect.height/2);
		foundPlayerObjects = false;
		isHost = false;
	}

	void Update () {
		if (!foundPlayerObjects && GameObject.FindGameObjectsWithTag("Player").Length > 0) {
			GameObject g = GameObject.FindGameObjectsWithTag ("Player") [0];
			isHost = g.GetComponent<PlayerScript> ().isLocalPlayer;
			foundPlayerObjects = true;
		}
	}

	void ClickedTheButton() {
		if (isHost) {
			
		}
	}
}
