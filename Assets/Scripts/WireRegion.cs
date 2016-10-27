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
        PlayerController p = c.GetComponent<PlayerController>();
        p.wireTransform = null;
        p.angle = Quaternion.FromToRotation(Vector3.forward, transform.forward).eulerAngles.y;
    }
}
