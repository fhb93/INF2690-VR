using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Engine : MonoBehaviour
{

    [SerializeField]
    private float Mass;

    [SerializeField]
    private float Acc;

    [SerializeField]
    [Range(1f, 4f)]
    private float MaxAcc;

    public bool EngineOn;

    public bool HydrogenOn;

    [SerializeField]
    private Vector3 Force;

    private Vector3 AuxForce;

    private Rigidbody rb;

    //Roation Angle
    private float RotationAmount = 0;

    [Range(1f, 10f)]
    public float RateUp = 1f;

    private Vector3 m_EulerAngleVelocity;

    [SerializeField]
    private AudioSource audioSource;

    private bool IsFirstTimeEngineOn = true;

    [SerializeField]
    private float maxHeight = 3;


    private static Quaternion forwardDirection = Quaternion.identity;
    private static Quaternion rightDirection = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
    private static Quaternion leftDirection = Quaternion.FromToRotation(Vector3.forward, Vector3.left);
    private static Quaternion backDirection = Quaternion.FromToRotation(Vector3.forward, Vector3.back);

    private Vector3 backPosition;

    public Vector3 RotationForce;

    private float maxTimer = 0.4f;


    public enum Dir{
        FRONT,
        LEFT,
        BACK,
        RIGHT,
        UP,
        //DOWN,
    }

    [Range(-1, 1)]
    public float[] InputDirection = new float[4];

    [SerializeField]
    private Transform Wheel;

    public float WheelAngle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = Mass;

        audioSource = GetComponent<AudioSource>();

        m_EulerAngleVelocity = new Vector3(0, 1, 0);

        backPosition = GameObject.Find("ForcesTarget").transform.position;

        Wheel = GameObject.Find("ShipWheel").transform;

        WheelAngle = Wheel.rotation.z;

        for (int i = 0; i < InputDirection.Length; i++)
        {
            InputDirection[i] = 0f;
        }


        //Time.timeScale = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        //Engine AudioSource and ACC volume handler
        if (HydrogenOn == false)
        {
            if (EngineOn == false)
            {
                //smooth decrease
                if (audioSource.volume > 0f)
                {
                    audioSource.volume -= Time.deltaTime;
                }
                else
                {
                    audioSource.volume = 0f;
                }

                SmoothChangeAcc(false);
            }
            else
            {
                SmoothChangeAcc(true);

                //smooth increase
                if (audioSource.volume < 1f)
                {
                    audioSource.volume += Time.deltaTime;
                }
                else
                {
                    audioSource.volume = 1f;
                }
            }
        }
        else
        {
            if (EngineOn == false)
            {
                if(audioSource.volume > 0f)
                {
                    audioSource.volume -= Time.deltaTime;
                }
                else
                {
                    audioSource.volume = 0f;
                }

                SmoothChangeAcc(false);
            }
            else
            {
                SmoothChangeAcc(true);

                if (audioSource.volume < 1f)
                {
                    audioSource.volume += Time.deltaTime;
                }
                else
                {
                    audioSource.volume = 1f;
                }
            }
        }

        InputDirection[(int) Dir.FRONT] = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
        
        InputDirection[(int) Dir.LEFT] = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 0);

        InputDirection[(int) Dir.BACK] = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

        InputDirection[(int) Dir.RIGHT] = Mathf.Clamp(Input.GetAxis("Horizontal"), 0, 1);
       

    }

    void SmoothChangeAcc(bool positiveSign)
    {
        if (positiveSign)
        {
            Acc = Acc < MaxAcc ? Acc + Time.deltaTime : MaxAcc;
        }
        else
        {
            Acc = Acc > 0 ? Acc - Time.deltaTime : 0;
        }
    }

    void FixedUpdate()
    {
        if (EngineOn == false && HydrogenOn == false)
        {
            return;
        }

        if (HydrogenOn == true)
        {
            if (EngineOn == false)
            {
                Force = transform.position.y < maxHeight ? transform.up * RateUp * MovementConstant : Vector3.zero;
            }
            else
            {
                Force = transform.position.y < maxHeight ? transform.forward * MovementConstant + transform.up * RateUp : transform.forward * MovementConstant;

                Force *= Acc;
            }
        }
        else
        {
            if (EngineOn == true)
            {
                Force = transform.forward * MovementConstant;

                Force *= Acc;
            }
        }

        rb.AddForce(Force);

        for (int i = 0; i < 4; i++)
        {
            if (InputDirection[i] > 0f)
            {
                //if (i == (int)Dir.FRONT)
                //{
                //    //TODO usar transform.Forward
                //}
                //else 
                if (i == (int)Dir.RIGHT)
                {

                    Rotation(Vector3.right);
                    MoveWheel(false);

                }
            }
            else if (InputDirection[i] < 0f)
            {
                if (i == (int)Dir.LEFT)
                {
                    Rotation(Vector3.left);
                    MoveWheel(true);
                }
                else if (i == (int)Dir.BACK)
                {

                    Force = Vector3.back;
                }
            }

        }
    }


    private void MoveWheel(bool left)
    {
        if(left)
        {
            Wheel.Rotate(Vector3.forward, Mathf.Lerp(WheelAngle, 90, Time.deltaTime));
        }
        else
        {
            Wheel.Rotate(Vector3.forward, Mathf.Lerp(WheelAngle, -90, Time.deltaTime));
        }

    }


    public float RotationConstant = 0f;
    public float MovementConstant = 0f;

    void Rotation(Vector3 dir)
    {
        //timer += Time.fixedDeltaTime;

        //if(timer > maxTimer)
        //{
        //    return;
        //}
        //RotationForce = Vector3.zero;

        if (dir.x > 0)
        {
            //Multiplicar por uma constante e testar no inspector
            RotationForce = Vector3.left;
        }
        else
        {
            //Multiplicar por uma constante e testar no inspector
            RotationForce = Vector3.right;
        }

        //rb.AddTorque(RotationForce * Acc * 10000); //.rotation = Quaternion.Euler(0, RotationForce.x, 0);

       // rb.AddTorque(Vector3.right * 100000 * Input.GetAxis("Horizontal"));

        //multiplicar pela constante do inspector
        rb.AddForceAtPosition(RotationForce * RotationConstant, backPosition);

        Force = rb.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Wayp"))
        {
            other.gameObject.GetComponent<WaypointLogic>().WayPointIsReached();
        }
    }
}
