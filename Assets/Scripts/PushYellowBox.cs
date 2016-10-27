using UnityEngine;
using System.Collections;

public class PushYellowBox : MonoBehaviour {

    public Rigidbody yellowBox;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter (Collision other)
    {
        yellowBox.AddForceAtPosition(transform.right * 10, transform.position + (Vector3.up * 10), ForceMode.Impulse);
        this.enabled = false;
    }
}
