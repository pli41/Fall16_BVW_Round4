using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform targetUp;
    public Transform targetBack;

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
            /*
            RaycastHit rayCastInfo;
            if (Physics.Raycast(target.position, player.transform.position - target.position, out rayCastInfo, 2)) {
                if (rayCastInfo.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    activeTarget = targetUp;
                }
            }
            */
        }

        transform.position = Vector3.Lerp(transform.position, activeTarget.position, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, activeTarget.rotation, Time.deltaTime);
    }
}
