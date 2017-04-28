using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartGameScript : MonoBehaviour {
	bool isHost, foundPlayerObjects;
	NetworkManager network;
	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
		RectTransform t = gameObject.GetComponent<RectTransform> ();
		t.position = new Vector3 (Screen.width-t.rect.width/2, Screen.height-t.rect.height/2);
		foundPlayerObjects = false;
		isHost = false;
	}

	/// <summary>
	/// Finds the host of the server.
	/// Updates once per frame.
	/// </summary>
	void Update () {
		if (!foundPlayerObjects && GameObject.FindGameObjectsWithTag("Player").Length > 0) {
			GameObject g = GameObject.FindGameObjectsWithTag ("Player") [0];
			isHost = g.GetComponent<PlayerScript> ().isLocalPlayer;
			foundPlayerObjects = true;
		}
	}

	/// <summary>
	/// If the host clicks the button, it moves all the players into the Lvl-Testing Area to begin the game.
	/// </summary>
	void ClickedTheButton() {
		if (isHost) {
			
		}
	}
}
