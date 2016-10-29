using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform targetUp;
    public Transform targetBack;

    public LayerMask visibleMeshes;

    public PlayerController player;

    public Camera forcedCamera { get; set; }

    private float fov, targetFov;
    private new Camera camera;
    
    void Start()
    {
        camera = GetComponent<Camera>();
        transform.parent = null;
        fov = targetFov = 60;
    }

    void Update()
    {
        Transform activeTarget = target;

        if (forcedCamera != null)
        {
            activeTarget = forcedCamera.transform;
            forcedCamera.transform.position = player.transform.position - forcedCamera.transform.forward * 5;
            targetFov = forcedCamera.fieldOfView;
        }
        else if (!player.canMove)
        {
            activeTarget = targetBack;
            targetFov = 80;
        }
        else
        {
            RaycastHit rayCastInfo;
            if (Physics.Raycast(player.transform.position, target.position - player.transform.position, out rayCastInfo, 2, visibleMeshes)) {
                if (rayCastInfo.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    activeTarget = targetUp;
                }
            }
            targetFov = 60;
        }

        

        transform.position = Vector3.Lerp(transform.position, activeTarget.position, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, activeTarget.rotation, Time.deltaTime);

        fov = Mathf.Lerp(fov, targetFov, Time.deltaTime);
        camera.fieldOfView = fov;
    }
}
