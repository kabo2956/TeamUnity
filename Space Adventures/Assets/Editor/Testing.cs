using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class Testing {


	[Test]
	public void EditorTest() {
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}

	[Test]
	public void PlayerTest() {
		//GameObject g = GameObject.Instantiate (Resources.Load ("Prefab/Items/Asteroid"));
		//Assert.AreEqual ("Asteroid", g.name.Substring (0, 8));
		GameObject player = new GameObject();
		player.AddComponent<PlayerScript> ();
		player = GameObject.Instantiate (player);
		Assert.AreEqual(6, (int)(player.GetComponent<PlayerScript>().getValue("walkVelocity")));
	}
}
