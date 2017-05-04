using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Loads a scene.
/// </summary>
public class StartOnScript : MonoBehaviour {
	/// <summary>
	/// Loads the scene specified.
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
}