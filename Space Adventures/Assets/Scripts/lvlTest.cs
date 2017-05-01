using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This is the logic for the main game.
/// </summary>
public class lvlTest : NetworkBehaviour {
	public Transform camTransform;
	public float speed;

	private Vector3 startPos;
	public Transform spawnTransform;
	public float platformGap;
	public float[] platformGapRange;
	public float[] spawnHeightRange;

	public GameObject[] smallDebris;
	private GameObject[] smallDebrisSlice;
	private GameObject[,] smallDebrisPool;
	public int smallDebrisPoolSize = 10;
	private int[] smallDebrisIndex;

	public GameObject[] lasers;
	private GameObject[,] lasersPool;
	public int laserPoolSize = 10;
	private int[] lasersPoolIndex;

	public GameObject ship;
	private GameObject[] shipPool;
	private int shipPoolIndex = 0;
	public int shipPoolSize = 5;

	public GameObject asteroid;

	public GameObject[] items;
	private GameObject[,] itemsPool;
	public int itemsPoolSize = 5;
	private int[] itemPoolIndex;

	private bool inTunnel = false;
	public float tunnelLength;

	public float shipSpawnMin, shipSpawnMax;
	private float shipSpawn;

	public float asteroidSpawnMin, asteroidSpawnMax;
	private float asteroidSpawn;

	public GameObject tunnel;
	private GameObject[] tunnelPool;
	public int tunnelPoolSize = 3;
	private int tunnelPoolIndex = 0;

	float chanceTunnel;
	float gap;
	float chance;
	float height;
	float gapDelta;
	int rand;

	[SyncVar]
	Vector3 sPos;
	[SyncVar]
	Quaternion sRot;
	[SyncVar]
	int obType;
	[SyncVar]
	int index;
	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () {
		startPos = spawnTransform.position;
		shipSpawn = Random.Range (shipSpawnMin, shipSpawnMax);
		asteroidSpawn = Random.Range (asteroidSpawnMin, asteroidSpawnMax);
		Debug.Log("small debris");
		smallDebrisPool = new GameObject[smallDebris.Length, smallDebrisPoolSize];
		for(int i = 0; i < smallDebris.Length; i++)
		{
			for(int j = 0; j < smallDebrisPoolSize; j++)
			{
				smallDebrisPool[i, j] = Instantiate(smallDebris[i], new Vector3(0, 2000, 0), spawnTransform.rotation * Quaternion.AngleAxis(180, Vector3.right)) as GameObject;
			}
		}
		smallDebrisSlice = new GameObject[smallDebrisPoolSize];
		smallDebrisIndex = new int[smallDebris.Length];
		for(int i = 0; i < smallDebris.Length; i++)
		{
			smallDebrisIndex[i] = 0;
		}
		Debug.Log("lasersPool");
		lasersPool = new GameObject[lasers.Length, laserPoolSize];
		for (int i = 0; i < lasers.Length; i++)
		{
			for (int j = 0; j < laserPoolSize; j++)
			{
				lasersPool[i, j] = Instantiate(lasers[i], new Vector3(0, 2000, 0), new Quaternion()) as GameObject;
			}
		}
		lasersPoolIndex = new int[lasers.Length];
		for (int i = 0; i < lasers.Length; i++)
		{
			lasersPoolIndex[i] = 0;
		}

		Debug.Log("shipPool");
		shipPool = new GameObject[shipPoolSize];
		for (int i = 0; i < shipPoolSize; i++)
		{
			shipPool[i] = Instantiate(ship, new Vector3(0, 2000, 0), new Quaternion()) as GameObject;
		}
		/*
        Debug.Log("asteroidPool");
        asteroidPool = new GameObject[asteroidPoolSize];
        for (int i = 0; i < asteroidPoolSize; i++)
        {
            asteroidPool[i] = Instantiate(asteroid, new Vector3(0, 2000, 0), new Quaternion()) as GameObject;
        }
        */

		Debug.Log("itemsPool");
		itemsPool = new GameObject[items.Length, itemsPoolSize];
		for (int i = 0; i < items.Length; i++)
		{
			for (int j = 0; j < itemsPoolSize; j++)
			{
				itemsPool[i, j] = Instantiate(items[i], new Vector3(0, 2000, 0), new Quaternion()) as GameObject;
			}
		}
		itemPoolIndex = new int[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			itemPoolIndex[i] = 0;
		}

		Debug.Log("tunnelPool");
		tunnelPool = new GameObject[tunnelPoolSize];
		for (int i = 0; i < tunnelPoolSize; i++)
		{
			tunnelPool[i] = Instantiate(tunnel, new Vector3(0, 2000, 0), spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right)) as GameObject;
		}

	}

	/// <summary>
	/// The main logic of the game is located here.
	/// Controls what spawns when, and moves the screen and the wall.
	/// Update is called once per frame
	/// </summary>
	/// 

	void Update()
	{
		camTransform.position += speed * Vector3.right * Time.deltaTime * 60;
		if (isServer)
		{
			//RpcUpdateLevel(camTransform.position, new Quaternion(), 3, 0);
			chanceTunnel = Random.Range(0f, 1f);
			gap = (inTunnel) ? platformGap + tunnelLength : platformGap;
			chance = (inTunnel) ? 1f : 0.9f;
			height = Random.Range(spawnHeightRange[0], spawnHeightRange[1]);
			gapDelta = Random.Range(platformGapRange[0], platformGapRange[1]);
			rand = Random.Range(0, smallDebris.Length);
			if (spawnTransform.position.x - startPos.x > gap & chanceTunnel < chance)
			{
				sPos = spawnTransform.position + height * Vector3.up + gap * Vector3.right;
				sRot = spawnTransform.rotation * (Quaternion.AngleAxis(180, Vector3.right));

				RepositionObject(GetGameObjectSlice(smallDebrisPool, smallDebrisSlice, rand, smallDebrisPoolSize),
					ref smallDebrisIndex[rand], smallDebrisPoolSize, sPos, sRot);
				RpcUpdateLevel(sPos, sRot, 1, rand);
				inTunnel = false;
				startPos = spawnTransform.position;
			}
			else if (spawnTransform.position.x - startPos.x > gap & chanceTunnel >= 0.9f)
			{
				inTunnel = true;
				//GameObject g = Instantiate(tunnel, spawnTransform.position, spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right));
				sPos = spawnTransform.position;
				sRot = spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right);
				RepositionObject(tunnelPool, ref tunnelPoolIndex, tunnelPoolSize, sPos, sRot);
				RpcUpdateLevel(sPos, sRot, 2, 1);

				BoxCollider floor = tunnelPool[tunnelPoolIndex - 1].GetComponentInChildren<BoxCollider>();
				Vector3 gPos = tunnelPool[tunnelPoolIndex - 1].transform.position;
				tunnelPool[tunnelPoolIndex - 1].transform.position = new Vector3(gPos.x + floor.size.y / 2, gPos.y, gPos.z);
				startPos = spawnTransform.position;
				int numberOfLasersSpawned = Random.Range(0, 5);
				for (int i = 0; i < numberOfLasersSpawned; i++)
				{
					int tSpawned = Random.Range(0, lasers.Length);
					GameObject l = Instantiate(lasers[tSpawned]);
					l.transform.position = new Vector3(gPos.x + Random.Range(0.3f, floor.size.y - 0.3f), gPos.y + l.transform.localScale.y, 0.125f);
				}
			}
			//destroyLaggedObjects ();
			spawnShips();
			//spawnAsteroid ();
			//spawnAsteroid ();}
		}
		else
		{
			Debug.Log("I'm a client");
		}
	}
	[ClientRpc]
	void RpcUpdateLevel(Vector3 pos, Quaternion rot, int type, int index)
	{
		if (isServer)
		{
			return;
		}
		if (type == 1)
		{
			RepositionObject(GetGameObjectSlice(smallDebrisPool, smallDebrisSlice, index, smallDebrisPoolSize),
				ref smallDebrisIndex[index], smallDebrisPoolSize, pos, rot);
		}
		else if(type == 2)
		{
			RepositionObject(tunnelPool, ref tunnelPoolIndex, tunnelPoolSize, pos, rot);
			BoxCollider floor = tunnelPool[tunnelPoolIndex - 1].GetComponentInChildren<BoxCollider>();
			Vector3 gPos = tunnelPool[tunnelPoolIndex - 1].transform.position;
			tunnelPool[tunnelPoolIndex - 1].transform.position = new Vector3(gPos.x + floor.size.y / 2, gPos.y, gPos.z);
			startPos = spawnTransform.position;
			int numberOfLasersSpawned = Random.Range(0, 5);
			for (int i = 0; i < numberOfLasersSpawned; i++)
			{
				int tSpawned = Random.Range(0, lasers.Length);
				GameObject l = Instantiate(lasers[tSpawned]);
				l.transform.position = new Vector3(gPos.x + Random.Range(0.3f, floor.size.y - 0.3f), gPos.y + l.transform.localScale.y, 0.125f);
			}
		}
		else if(type == 3)
		{
			pos = Vector3.Lerp(pos, camTransform.position, 0.5f);
			camTransform.position = pos;
			return;
		}
		else if(type == 4)
		{
			int prevSPI = shipPoolIndex;
			RepositionObject(shipPool, ref shipPoolIndex, shipPoolSize, pos, new Quaternion());
			shipPool[prevSPI].GetComponent<WarningShip>().Spawn();
		}
		spawnShips();
	}
	void RepositionObject(GameObject[] obPool, ref int obIndex, int obPoolSize, Vector3 pos, Quaternion rot)
	{
		obPool[obIndex].transform.position = pos;
		obPool[obIndex].transform.rotation = rot;
		if (obIndex >= obPoolSize - 1)
		{
			obIndex = 0;
		}
		else
		{
			obIndex += 1;
		}
		Debug.Log(obPool[obIndex].name + ": " + obIndex.ToString());
		//return obIndex;
	}

	GameObject[] GetGameObjectSlice(GameObject[,] ob2D, GameObject[] ob1D, int index, int poolSize)
	{
		for(int i = 0; i < poolSize; i++)
		{
			ob1D[i] = ob2D[index, i];
		}
		return ob1D;
	}
	/// <summary>
	/// Spawns the Ships. (Or at least the warnings for them...)
	/// </summary>
	void spawnShips() {
		shipSpawn -= Time.deltaTime;
		if (shipSpawn <= 0 && shipSpawnMin > 0) {
			shipSpawn = Random.Range (shipSpawnMin, shipSpawnMax);
			int prevSPI = shipPoolIndex;
			sPos = new Vector3(camTransform.position.x + 10,
				spawnTransform.position.y + Random.Range(spawnHeightRange[0] - 1.5f, spawnHeightRange[1] + 1.5f), -1);
			RepositionObject(shipPool, ref shipPoolIndex, shipPoolSize, sPos, new Quaternion());
			RpcUpdateLevel(sPos, sRot, 4, 0);
			//shipPool[shipPoolIndex].transform.position = new Vector3 (0, spawnTransform.position.y+Random.Range(spawnHeightRange[0]-1.5f,spawnHeightRange[1]+1.5f), -1);
			shipPool[prevSPI].GetComponent<WarningShip> ().Spawn ();
			//GameObject s = Instantiate (ship);
			//Vector3 sPos = s.transform.position;
			//s.transform.position = new Vector3 (sPos.x, spawnTransform.position.y+Random.Range(spawnHeightRange[0]-1.5f,spawnHeightRange[1]+1.5f), sPos.z);
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