using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime);
    }
}
