﻿using UnityEngine;
using System.Collections;

using UsefulThings;

public class Wind : MonoBehaviour {

    public Curve activationCurve;
    public ParticleSystem particles;
    public float strength = 50;

	void Start () {
	    
	}
	
	void Update () {
	    if (IsActive())
        {
            particles.Emit((int)(600 * Time.deltaTime));
        }
	}

    void OnTriggerStay(Collider c)
    {
        if (IsActive())
        {
            c.GetComponent<Rigidbody>().AddForce(transform.forward * strength);
        }
    }

    void OnTriggerEnter(Collider c)
    {
        DMXController.SetFan(true);
    }

    void OnTriggerExit(Collider c)
    {
        DMXController.SetFan(false);
    }

    private bool IsActive()
    {
        return activationCurve.Evaluate(Time.time) > 0.5f;
    }
}
