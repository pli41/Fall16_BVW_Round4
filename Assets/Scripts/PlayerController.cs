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

    public bool keyControls;

    public Transform tiltAnchor;
    public Transform buttonAnchor;

    public GameObject bird;
    public GameObject ending;

    public bool canMove { get; set; }
    public Transform wireTransform { get; set; }

    private new Rigidbody rigidbody;

    private Transform head;
    private Transform hip;
    private Transform footL;
    private Transform footR;

    private float speed;
    public float angle { get; set; }
    private float buttonSpin;
    private float lastJumpTime;

    private float buttonLean;
    private float buttonLeanSpeed;

    private Vector3 RespawnPosition;

    private Queue<float> prevHeights;

    [SerializeField]
    FloatUI thoughtBubble;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        head = controller.Head.transform;
        hip = controller.Hip_Center.transform;
        footL = controller.Foot_Left.transform;
        footR = controller.Foot_Right.transform;
        prevHeights = new Queue<float>();
        canMove = true;

        angle = transform.eulerAngles.y;
    }

    void Update()
    {
        Debug.DrawLine(hip.position, head.position, Color.blue);
        Vector3 lean = head.position - hip.position;

        Vector3 leanDirection = Vector3.ProjectOnPlane(lean, Vector3.up);
        Debug.DrawRay(hip.position, leanDirection, Color.red);

        float xAxis = leanDirection.x;
        float zAxis = leanDirection.z;

        if (keyControls)
        {
            xAxis = Input.GetAxis("Horizontal");
            zAxis = -Input.GetAxis("Vertical");
            lean = Vector3.up;
        }

        if (canMove)
        {

            // Normal controls
            if (wireTransform == null)
            {
                float height = hip.position.y;
                if (prevHeights.Count < 60)
                {
                    prevHeights.Enqueue(height);
                }
                else
                {
                    float avgPrevHeight = prevHeights.Sum() / prevHeights.Count();

                    if (height - avgPrevHeight > 0.1f || (keyControls && Input.GetKey(KeyCode.Space)))
                    {
                        if (Time.time - lastJumpTime > jumpDelay)
                        {
                            rigidbody.velocity += jumpStrength * Vector3.up;
                            speed = Mathf.Max(speed, 3f);
                            lastJumpTime = Time.time;
                            SfxManager.PlaySfx(1);
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

                // Rotate the button
                transform.eulerAngles = Vector3.up * angle;

                // Set the speed of the button
                Vector3 gravity = Vector3.Project(rigidbody.velocity, Vector3.down);
                rigidbody.velocity = Quaternion.Euler(0, angle, 0) * Vector3.forward * speed + gravity;
                //rigidbody.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * speed);
            }
            else // Wire controls
            {
                // Accelerate/Decelerate the button
                if (zAxis < 0) // Forward
                {
                    speed = Mathf.Min(maxSpeed, speed - zAxis * 5 * Time.deltaTime);
                }
                else if (zAxis > 0.2f) // Backward
                {
                    speed = Mathf.Max(0, speed - zAxis * 3 * Time.deltaTime);
                }
                else
                {
                    speed = Mathf.Lerp(speed, 0, 0.5f * Time.deltaTime);
                }

                Debug.Log(xAxis);

                buttonLean += buttonLeanSpeed * Time.deltaTime;
                buttonLean = Mathf.Clamp(buttonLean, -60, 60);
                buttonLeanSpeed -= xAxis * 100 * Time.deltaTime;
                buttonLeanSpeed += Mathf.Sign(buttonLeanSpeed) * (1 + speed) * Time.deltaTime;

                tiltAnchor.localEulerAngles = new Vector3(0, 0, buttonLean);

                Vector3 gravity = Vector3.Project(rigidbody.velocity, Vector3.down);
                /*
                if (Mathf.Abs(buttonLean) > 75f)
                {
                    Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ObstaclePlatform"), LayerMask.NameToLayer("Player"), true);
                }
                else
                {*/
                transform.eulerAngles = new Vector3(0, wireTransform.eulerAngles.y, 0);
                rigidbody.velocity = wireTransform.forward * speed + gravity;
                //}

                // Spin the button based on the speed
                buttonSpin = (buttonSpin + 360 * speed * Time.deltaTime) % 360;
                buttonAnchor.localEulerAngles = Vector3.right * buttonSpin;
            }
        }
        else
        {
            angle = Mathf.Clamp(angle + xAxis * 180 * Time.deltaTime, 150, 250);
            speed = 0;
            transform.eulerAngles = Vector3.up * angle;
            rigidbody.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (LayerMask.NameToLayer("BirdTrigger") == other.gameObject.layer)
        {
            Debug.Log("Hello");
            bird.SetActive(true);
            
            
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            speed = 0;
            buttonLean = 0;
            buttonLeanSpeed = 0;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ObstaclePlatform"), LayerMask.NameToLayer("Player"), false);
            gameObject.transform.position = RespawnPosition;
        }
        else if (other.gameObject.CompareTag("Respawn"))
        {
            RespawnPosition = other.gameObject.transform.position;
        }
        else if (other.gameObject.CompareTag("EndGame"))
        {
            ending.SetActive(true);
        }
        else if (other.gameObject.name.CompareTo("Thoughtbubble_Zone") == 0)
        {
            Destroy(other.GetComponent<Collider>());
            thoughtBubble.Activate(other.GetComponent<ThoughtTrigger>());
        }
    }

    void OnCollisionEnter(Collision other)
    {
        SoundEffects[] sfx = other.gameObject.GetComponents<SoundEffects>();
        foreach (SoundEffects s in sfx)
        {
            if (!s.loop)
            {
                SfxManager.PlaySfx(s.id);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        SoundEffects[] sfx = other.gameObject.GetComponents<SoundEffects>();
        foreach (SoundEffects s in sfx)
        {
            if (s.loop)
            {
                SfxManager.StopLoop(s.id);
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        SoundEffects[] sfx = other.gameObject.GetComponents<SoundEffects>();
        foreach (SoundEffects s in sfx)
        {
            if (s.loop)
            {
                SfxManager.PlayLoop(s.id, Mathf.Abs(speed / maxSpeed));
            }
        }
    }
}
