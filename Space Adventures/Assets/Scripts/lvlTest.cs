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
    private GameObject[] asteroidPool;
    public int asteroidPoolSize = 5;
    private int asteroidPoolIndex = 0;

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

    [SyncVar]
    float chanceTunnel;
    [SyncVar]
    float gap;
    [SyncVar]
    float chance;
    [SyncVar]
    float height;
    [SyncVar]
    float gapDelta;
    [SyncVar]
    int rand;
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
        camTransform.position += speed * Vector3.right;
        if (isServer)
        {
            chanceTunnel = Random.Range(0f, 1f);
            gap = (inTunnel) ? platformGap + tunnelLength : platformGap;
            chance = (inTunnel) ? 1f : 0.9f;
            height = Random.Range(spawnHeightRange[0], spawnHeightRange[1]);
            gapDelta = Random.Range(platformGapRange[0], platformGapRange[1]);
            rand = Random.Range(0, smallDebris.Length);
            RpcUpdateLevel(chanceTunnel, chance, height, gapDelta, rand);
        }else
        {
            Debug.Log("I'm a client");
        }
    }
    [ClientRpc]
    void RpcUpdateLevel(float ct, float c, float h, float gapD, int rIndex)
    {
        float gap = (inTunnel) ? platformGap + tunnelLength : platformGap;
        if (spawnTransform.position.x - startPos.x > gap & ct < c)
        {
            RepositionObject(GetGameObjectSlice(smallDebrisPool, smallDebrisSlice, rIndex, smallDebrisPoolSize), ref smallDebrisIndex[rIndex], smallDebrisPoolSize,
                spawnTransform.position + h * Vector3.up + gapD * Vector3.right,
                spawnTransform.rotation * (Quaternion.AngleAxis(180, Vector3.right)));
            if (smallDebrisIndex[rIndex] >= smallDebrisPoolSize - 1)
            {
                smallDebrisIndex[rIndex] = 0;
            }
            else
            {
                smallDebrisIndex[rIndex] += 1;
            }
            inTunnel = false;
            startPos = spawnTransform.position;
        }
        else if (spawnTransform.position.x - startPos.x > gap & ct >= 0.9f)
        {
            inTunnel = true;
            //GameObject g = Instantiate(tunnel, spawnTransform.position, spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right));
            RepositionObject(tunnelPool, ref tunnelPoolIndex, tunnelPoolSize, spawnTransform.position,
                spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right));
            if (tunnelPoolIndex >= tunnelPoolSize - 1)
            {
                tunnelPoolIndex = 0;
            }
            else
            {
                tunnelPoolIndex += 1;
            }
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
    void RepositionObject(GameObject[] obPool, ref int obIndex, int obPoolSize, Vector3 pos, Quaternion rot)
    {
        obPool[obIndex].transform.position = pos;
        obPool[obIndex].transform.rotation = rot;
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
            RepositionObject(shipPool, ref shipPoolIndex, shipPoolSize,
				new Vector3(camTransform.position.x+10, spawnTransform.position.y + Random.Range(spawnHeightRange[0] - 1.5f, spawnHeightRange[1] + 1.5f), -1),
                new Quaternion());
            if (shipPoolIndex >= shipPoolSize - 1)
            {
                shipPoolIndex = 0;
            }
            else
            {
                shipPoolIndex += 1;
            }
            //shipPool[shipPoolIndex].transform.position = new Vector3 (0, spawnTransform.position.y+Random.Range(spawnHeightRange[0]-1.5f,spawnHeightRange[1]+1.5f), -1);
            shipPool [prevSPI].GetComponent<WarningShip> ().Spawn ();
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
