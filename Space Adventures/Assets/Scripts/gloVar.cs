using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For global varialbes & functions used elsewhere.
/// ...Yeah.
/// </summary>
public class gloVar : MonoBehaviour {
	/// <summary>
	/// The gravity of everyone.
	/// </summary>
	public static float gravity = 10f;
	/// <summary>
	/// How many frames a button is "pressed" before it is just "held."
	/// </summary>
	public static int isPressed = 2;
	/// <summary>
	/// How the player throws things.
	/// True for rigid, false for relative.
	/// </summary>
	public static bool throwingHandler = false;
	public static float animationSpeed = 1/60.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
