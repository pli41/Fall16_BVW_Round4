using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Select (Map), Aggregate (Reduce), Where (Filter)
using UsefulThings;

public class PlayerController : MonoBehaviour
{

    public CubemanController controller;
    public float maxSpeed;
    public float jumpStrength;
    public float jumpDelay;

    public Transform tiltAnchor;
    public Transform buttonAnchor;

    public GameObject bird;

    public bool canMove { get; set; }

    private new Rigidbody rigidbody;

    private Transform head;
    private Transform hip;
    private Transform footL;
    private Transform footR;

    private float speed;
    public float angle { get; set; }
    private float buttonSpin;
    private float lastJumpTime;

    private Queue<float> prevHeights;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        head = controller.Head.transform;
        hip = controller.Hip_Center.transform;
        footL = controller.Foot_Left.transform;
        footR = controller.Foot_Right.transform;
        prevHeights = new Queue<float>();
        canMove = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SfxManager.PlaySfx(0);
            //SfxManager.PlayLoop(0);
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            //SfxManager.StopLoop(0);
        }

        Debug.DrawLine(hip.position, head.position, Color.blue);
        Vector3 lean = head.position - hip.position;

        Vector3 leanDirection = Vector3.ProjectOnPlane(lean, Vector3.up);
        Debug.DrawRay(hip.position, leanDirection, Color.red);

        float xAxis = leanDirection.x;
        float zAxis = leanDirection.z;

        if (canMove)
        {
            float height = hip.position.y;
            if (prevHeights.Count < 60)
            {
                prevHeights.Enqueue(height);
            }
            else
            {
                float avgPrevHeight = prevHeights.Sum() / prevHeights.Count();

                if (height - avgPrevHeight > 0.1f)
                {
                    if (Time.time - lastJumpTime > jumpDelay)
                    {
                        rigidbody.velocity += jumpStrength * Vector3.up;
                        speed = Mathf.Max(speed, 3f);
                        lastJumpTime = Time.time;
                    }
                }

                prevHeights.Enqueue(height);
                prevHeights.Dequeue();
            }

            // Tilt the button when turning
            float leanAngle = (xAxis < 0 ? 1 : -1) * Vector3.Angle(Vector3.up, Vector3.ProjectOnPlane(lean, Vector3.forward));
            tiltAnchor.localEulerAngles = new Vector3(0, 0, leanAngle);

            // Turn the button
            angle = (angle + xAxis * 180 * Time.deltaTime) % 360;

            // Accelerate/Decelerate the button
            if (zAxis < 0) // Forward
            {
                speed = Mathf.Min(maxSpeed, speed - zAxis * 5 * Time.deltaTime);
            }
            else if (zAxis > 0.2f) // Backward
            {
                speed = Mathf.Max(-maxSpeed, speed - zAxis * 3 * Time.deltaTime);
            }
            else
            {
                speed = Mathf.Lerp(speed, 0, 0.5f * Time.deltaTime);
            }

            // Spin the button based on the speed
            buttonSpin = (buttonSpin + 360 * speed * Time.deltaTime) % 360;
            buttonAnchor.localEulerAngles = Vector3.right * buttonSpin;

            // Set the speed of the button
            Vector3 gravity = Vector3.Project(rigidbody.velocity, Vector3.down);
            transform.eulerAngles = Vector3.up * angle;
            rigidbody.velocity = Quaternion.Euler(0, angle, 0) * Vector3.forward * speed + gravity;
        }

        else
        {
            angle = Mathf.Clamp(angle + xAxis * 180 * Time.deltaTime, 150, 210);
            speed = 0;
            transform.eulerAngles = Vector3.up * angle;
            rigidbody.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.NameToLayer("EventTrigger") == other.gameObject.layer)
        {
            bird.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        SoundEffects sfx = other.gameObject.GetComponent<SoundEffects>();
        if (sfx != null)
        {
            SfxManager.PlaySfx(sfx.id);
        }
    }

    void OnCollisionStay (Collision other) {
        SoundEffects sfx = other.gameObject.GetComponent<SoundEffects>();
        if (sfx != null)
        {
            SfxManager.PlayLoop(sfx.id);
        }
        else
        {
            SfxManager.StopLoop(sfx.id);
        }
    }
}
