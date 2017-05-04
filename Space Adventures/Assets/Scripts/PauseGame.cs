using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The pause menu.
/// </summary>
public class PauseGame : MonoBehaviour {
	/// <summary>
	/// The canvas on which the pause menu resides.
	/// </summary>
	public Transform canvas;
	// Update is called once per frame
	/// <summary>
	/// Update is called once per frame
	/// Pauses the game if you hit Escape.
	/// </summary>
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pause ();
		}
	}
	/// <summary>
	/// Sets the canvas the pause menu is on on and off when pausing the game.
	/// </summary>
	public void Pause(){

		if (canvas.gameObject.activeInHierarchy == false) {
			canvas.gameObject.SetActive (true);
		} else {
			canvas.gameObject.SetActive (false);
		}

	}
}
