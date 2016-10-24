using UnityEngine;
using System.Collections;

public class WireRegion : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        c.GetComponent<PlayerController>().wireTransform = transform;
    }

    void OnTriggerStay(Collider c)
    {
        c.GetComponent<PlayerController>().wireTransform = transform;
    }

    void OnTriggerExit(Collider c)
    {
        c.GetComponent<PlayerController>().wireTransform = null;
    }
}
