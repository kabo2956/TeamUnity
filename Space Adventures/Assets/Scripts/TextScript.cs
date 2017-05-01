using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {
	public GameObject following;
	public int index;
	public Color color;
	/// <summary>
	/// Use this for initialization.
	/// Blank :|
	/// </summary>
	void Start () {
		int playerIndex = -1;
		GameObject[] g = FindObjectsOfType (typeof(GameObject)) as GameObject[];
		for (int i = 0; i < g.Length; i++) {
			if (g [i].tag == "Player") {
				playerIndex++;
				if (playerIndex == index) {
					following = g [i];
					gameObject.GetComponent<Text>().text = g [i].GetComponent<PlayerScript> ().playerName;
				}
			} 
		}
		//Prototype.NetworkLobby.LobbyPlayer
	}

	/// <summary>
	/// Update this is called once per frame.
	/// This changes the position of the text to on top of the player.
	/// </summary>
	void Update () {
		if (following != null) {
			Vector3 foPos = following.transform.position;
			Vector3 worldPos = new Vector3 (foPos.x, foPos.y + 0.125f);
			transform.position = Camera.main.WorldToScreenPoint (worldPos);
		} 
	}
}
