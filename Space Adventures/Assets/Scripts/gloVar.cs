using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * For global variables & functions
 * I know it can be bad practice, but I feel like we may need some variables that can be modified 
 * by one section and used in another section.
 * So... yeah.
 * */
public class gloVar : MonoBehaviour {
	public static float gravity = 10f;
	public static int isPressed = 2;
	public static float animationSpeed = 1/60.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
