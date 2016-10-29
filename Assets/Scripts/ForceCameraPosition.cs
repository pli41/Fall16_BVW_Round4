using UnityEngine;
using System.Collections;

public class ForceCameraPosition : MonoBehaviour
{
    public CameraFollow mainCameraFollow;
    
    void OnTriggerEnter(Collider c)
    {
        mainCameraFollow.forcedCamera = GetComponentInChildren<Camera>();
        Debug.Log(mainCameraFollow.forcedCamera.gameObject.name);
    }

    void OnTriggerExit(Collider c)
    {
        mainCameraFollow.forcedCamera = null;
    }
}
