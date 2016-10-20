using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public CubemanController controller;
    public float maxSpeed;

    private new Rigidbody rigidbody;

    private Transform head;
    private Transform hip;

    private float speed;
    private float angle;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        head = controller.Head.transform;
        hip = controller.Hip_Center.transform;
    }

    void Update()
    {
        Debug.DrawLine(hip.position, head.position, Color.blue);
        Vector3 lean = head.position - hip.position;

        Vector3 leanDirection = Vector3.ProjectOnPlane(lean, Vector3.up);

        Debug.DrawRay(hip.position, leanDirection, Color.red);

        float xAxis = leanDirection.x;
        float zAxis = leanDirection.z;

        if (xAxis < -0.2f)
        {
            Debug.Log("Left");
            angle = (angle - 30 * Time.deltaTime) % 360;
        }
        else if (xAxis > 0.2f)
        {
            Debug.Log("Right");
            angle = (angle + 30 * Time.deltaTime) % 360;
        }
        else if (zAxis < -0.1f)
        {
            Debug.Log("Forward");
            speed = Mathf.Min(maxSpeed, speed + 0.5f * Time.deltaTime);
        }
        else if (zAxis > 0.2f)
        {
            Debug.Log("Backward");
            speed = Mathf.Max(-maxSpeed, speed - 0.5f * Time.deltaTime);
        }
        else
        {
            Debug.Log("Straight");
            speed = Mathf.Lerp(speed, 0, Time.deltaTime);
        }

        transform.eulerAngles = Vector3.up * angle;
        rigidbody.velocity = transform.forward * speed;
    }
}
