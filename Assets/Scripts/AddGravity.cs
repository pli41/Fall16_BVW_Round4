using UnityEngine;
using System.Collections;

public class AddGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GetComponent<Rigidbody>().AddForceAtPosition(transform.right * 10, transform.position + (Vector3.up * 10));
       // GetComponent<Rigidbody>().AddForce(transform.right * 500);
        GetComponent<Rigidbody>().useGravity = true;

    }
}
