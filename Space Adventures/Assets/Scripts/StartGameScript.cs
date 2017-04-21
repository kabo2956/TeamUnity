using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RectTransform t = gameObject.GetComponent<RectTransform> ();
		t.position = new Vector3 (Screen.width-t.rect.width/2, Screen.height-t.rect.height/2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClick () {
		print ("I'm doing this correctly!");
	}
}
