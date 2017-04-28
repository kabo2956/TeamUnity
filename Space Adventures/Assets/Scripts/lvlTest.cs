using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the logic for the main game.
/// </summary>
public class lvlTest : MonoBehaviour {

    public Transform camTransform;
    public float speed;

    private Vector3 startPos;
    public Transform spawnTransform;
    public float platformGap;
    public float[] platformGapRange;
    public float[] spawnHeightRange;

    public GameObject[] smallDebris;
	public GameObject[] lasers;
	public GameObject ship;
	public GameObject asteroid;
	public GameObject[] items;
    private bool inTunnel = false;
    public float tunnelLength;
	public float shipSpawnMin, shipSpawnMax;
	private float shipSpawn;
	public float asteroidSpawnMin, asteroidSpawnMax;
	private float asteroidSpawn;
    public GameObject tunnel;

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
        startPos = spawnTransform.position;
		shipSpawn = Random.Range (shipSpawnMin, shipSpawnMax);
		asteroidSpawn = Random.Range (asteroidSpawnMin, asteroidSpawnMax);
	}

	/// <summary>
	/// The main logic of the game is located here.
	/// Controls what spawns when, and moves the screen and the wall.
	/// Update is called once per frame
	/// </summary>
	void Update () {
        camTransform.position += speed * Vector3.right;
        float chanceTunnel = Random.Range(0f, 1f);
        float gap = (inTunnel) ? platformGap + tunnelLength : platformGap;
        float chance = (inTunnel) ? 1f : 0.9f;
        if (spawnTransform.position.x - startPos.x > gap & chanceTunnel < chance) {
            float height = Random.Range(spawnHeightRange[0], spawnHeightRange[1]);
            float gapDelta = Random.Range(platformGapRange[0], platformGapRange[1]);
            Instantiate(smallDebris[Random.Range(0, 3)], spawnTransform.position + height * Vector3.up
            + gapDelta * Vector3.right, spawnTransform.rotation * Quaternion.AngleAxis(180, Vector3.right));
            inTunnel = false;
            startPos = spawnTransform.position;
        }
        else if(spawnTransform.position.x - startPos.x > gap & chanceTunnel >= 0.9f) {
            inTunnel = true;
            GameObject g = Instantiate(tunnel, spawnTransform.position, spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right));
			BoxCollider floor = g.GetComponentInChildren<BoxCollider> ();
			Vector3 gPos = g.transform.position;
			g.transform.position = new Vector3 (gPos.x+floor.size.y/2, gPos.y, gPos.z);
            startPos = spawnTransform.position;
			int numberOfLasersSpawned = Random.Range (0, 5);
			for (int i = 0; i < numberOfLasersSpawned; i++) {
				int tSpawned = Random.Range (0, lasers.Length);
				GameObject l = Instantiate(lasers[tSpawned]);
				l.transform.position = new Vector3 (gPos.x+Random.Range(0.3f, floor.size.y-0.3f), gPos.y+l.transform.localScale.y, 0.125f);
			}
        }
		destroyLaggedObjects ();
		spawnShips ();
		spawnAsteroid ();
	}

	/// <summary>
	/// Spawns the Ships. (Or at least the warnings for them...)
	/// </summary>
	void spawnShips() {
		shipSpawn -= Time.deltaTime;
		if (shipSpawn <= 0 && shipSpawnMin > 0) {
			shipSpawn = Random.Range (shipSpawnMin, shipSpawnMax);
			GameObject s = Instantiate (ship);
			Vector3 sPos = s.transform.position;
			s.transform.position = new Vector3 (sPos.x, spawnTransform.position.y+Random.Range(spawnHeightRange[0]-1.5f,spawnHeightRange[1]+1.5f), sPos.z);

		}
	}

	/// <summary>
	/// Spawns an Asteroid.
	/// </summary>
	void spawnAsteroid() {
		asteroidSpawn -= Time.deltaTime;
		if (asteroidSpawn <= 0 && asteroidSpawnMin > 0) {
			asteroidSpawn = Random.Range (asteroidSpawnMin, asteroidSpawnMax);
			GameObject a = Instantiate (asteroid);
			//Vector3 aPos = a.transform.position;
			a.transform.position = new Vector3(camTransform.position.x+Random.Range(0,10),Random.Range(camTransform.position.y+8,camTransform.position.y+6),0);
		}
	}

	/// <summary>
	/// Destroys the objects that are lagging behind the wall to prevent memory leaks.
	/// </summary>
	void destroyLaggedObjects() {
		GameObject[] g = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for (int i = 0; i < g.Length; i++) {
			if (!g [i].tag.Equals ("Untagged")) {
				if (g [i].transform.position.x < camTransform.position.x - tunnelLength) {
					Destroy (g [i]);
				}
			}
		}
	}
}
