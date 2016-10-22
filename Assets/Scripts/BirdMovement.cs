using UnityEngine;
using System.Collections;
using UsefulThings;

public class BirdMovement : MonoBehaviour {

    public BezierSpline path;
    public PlayerController player;

    private bool hasEnded = false;

    private float birdStartTime;

	// Use this for initialization
	void Start () {
        birdStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = path.Evaluate((Time.time-birdStartTime)*0.1f);
        if ((Time.time-birdStartTime)*0.1f >= 2 && !hasEnded)
        {
            player.transform.parent = null;
            player.GetComponent<Collider>().enabled = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.canMove = true;
            hasEnded = true;
        }
	}


    void OnTriggerEnter (Collider other)
    {
        player.transform.parent = transform;
        player.GetComponent<Collider>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.canMove = false;
        player.transform.localPosition = Vector3.zero;
        player.angle = 180;
    }
}
