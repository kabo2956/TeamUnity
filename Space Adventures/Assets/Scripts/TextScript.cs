using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The text that hovers over all of the players.
/// </summary>
public class TextScript : MonoBehaviour {
	/// <summary>
	/// The player that the Text is following.
	/// </summary>
	public GameObject following;
	/// <summary>
	/// What player it is actually following in a multiplayer match. Can go from 0-3, 0 being Player 1, and 3 being Player 4.
	/// </summary>
	public int index;
	/// <summary>
	/// The color of the text.
	/// </summary>
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
