using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform targetUp;
    public Transform targetBack;

    public LayerMask visibleMeshes;

    public PlayerController player;
    
    void Update()
    {
        Transform activeTarget = target;

        if (!player.canMove)
        {
            activeTarget = targetBack;
        }
        else
        {
            ///*
            RaycastHit rayCastInfo;
            Debug.DrawRay(target.position, player.transform.position - target.position, Color.blue);
            if (Physics.Raycast(player.transform.position, target.position - player.transform.position, out rayCastInfo, 2, visibleMeshes)) {
                Debug.Log("Hello");
                if (rayCastInfo.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    activeTarget = targetUp;
                }
            }
            //*/
        }

        transform.position = Vector3.Lerp(transform.position, activeTarget.position, 5*Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, activeTarget.rotation, 5*Time.deltaTime);
    }
}
