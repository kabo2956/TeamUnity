using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour {
	public GameObject following;
	public string followName;
	public int index;
	public Color color;
	/// <summary>
	/// Use this for initialization.
	/// Blank :|
	/// </summary>
	void Start () {
		
	}

	/// <summary>
	/// Update this is called once per frame.
	/// This changes the position of the text to on top of the player.
	/// </summary>
	void Update () {
		if (following != null) {
			Vector3 foPos = following.transform.position;
			Vector3 worldPos = new Vector3 ((foPos.x - Camera.main.transform.position.x), foPos.y - Camera.main.transform.position.y + 1);
			transform.position = Camera.main.WorldToScreenPoint (worldPos);
		} 
	}
}
