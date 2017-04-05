using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvlTest : MonoBehaviour {

    public Transform camTransform;
    public float speed;

    private Vector3 startPos;
    public Transform spawnTransform;
    public float platformGap;
    public float[] platformGapRange;
    public float[] spawnHeightRange;

    public GameObject[] smallDebris;
    private bool inTunnel = false;
    public float tunnelLength;
    public GameObject tunnel;

	// Use this for initialization
	void Start () {
        startPos = spawnTransform.position;
	}
	
	// Update is called once per frame
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
            Instantiate(tunnel, spawnTransform.position, spawnTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right));
            startPos = spawnTransform.position;
        }
        
	}
}
