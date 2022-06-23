using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabObject : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObject; // 1
    private GameObject objectInHand; // 2

    private SpinLever gramophone;
    private Engine wheelController;

    [SerializeField]
    private GameObject ZeppelinCockpitPlayerPos;

    [SerializeField]
    private GameObject shipWheelCenterObj;

    [SerializeField]
    private Vector3 shipWheelCenterVector;

    private Coroutine resetWheel;

    private void Start()
    {
        gramophone = GameObject.Find("SpinPoint").GetComponent<SpinLever>();
        wheelController = GameObject.Find("ZeppelinCockpit").GetComponent<Engine>();
        shipWheelCenterVector = shipWheelCenterObj.transform.position;
        //ZeppelinCockpitPlayerPos = GameObject.Find("PlayerPosition");
    }
    // Update is called once per frame
    void Update()
    {
        shipWheelCenterVector = shipWheelCenterObj.transform.position;

        // gameObject.transform.position = ZeppelinCockpitPlayerPos.transform.localPosition;
        //gameObject.transform.rotation = ZeppelinCockpitPlayerPos.transform.localRotation;
        // 1
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

    }

    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        // 2
        collidingObject = col.gameObject;

        if(collidingObject.GetComponent<Renderer>() != null)
        {
            originalColor = collidingObject.GetComponent<Renderer>().material.color;

            collidingObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            if(collidingObject.name.Contains("Gramoph"))
            {
                Renderer renderer = collidingObject.GetComponentsInChildren<Renderer>()[3];

                originalColor = renderer.material.color;

                renderer.material.color = Color.green;
            }
        }
       
    }

    public Color originalColor;

    // 1
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        if (collidingObject.name.Contains("Gramoph") == false)
        {
            collidingObject.GetComponent<Renderer>().material.color = originalColor;
        }
        else
        {
            Renderer renderer = collidingObject.GetComponentsInChildren<Renderer>()[3];

            renderer.material.color = originalColor;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        
        if (objectInHand.name != "shipWheel")
        {
            if (objectInHand.name != "gramophoneLever")
            {
                if (objectInHand.GetComponentInParent<SpinControlLever>() != null)
                {
                    objectInHand.GetComponentInParent<SpinControlLever>().UserInput = true;

                    collidingObject.GetComponent<Renderer>().material.color = originalColor;
                }

                if(objectInHand.name.Contains("1"))
                {
                    objectInHand = collidingObject;

                    collidingObject = null;

                    var joint = AddFixedJoint();

                    joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
                }
            }
            else
            {
                gramophone.UserInput = true;

                gramophone.materialRenderer.material.color = originalColor;
            }

            collidingObject = null;

            //objectInHand = null;

        }
        else
        {
            if (transform.position.x > shipWheelCenterObj.transform.position.x)
            {
                Debug.Log("DIREITA");

                wheelController.InputDirection[(int) Engine.Dir.RIGHT] = 1;

                wheelController.InputDirection[(int) Engine.Dir.LEFT] = 0;

                CorouWheel();
            }
            else
            {
                Debug.Log("ESQUERDA");

                wheelController.InputDirection[(int)Engine.Dir.RIGHT] = 0;

                wheelController.InputDirection[(int)Engine.Dir.LEFT] = -1;

                CorouWheel();
            }
        }

      
    }

    private void CorouWheel()
    {
        if (resetWheel != null)
        {
            StopCoroutine(resetWheel);

            resetWheel = null;
        }

        resetWheel = StartCoroutine(ResetWheelContoller());
    }


    IEnumerator ResetWheelContoller()
    {
        yield return new WaitForSeconds(2f);

        wheelController.InputDirection[(int)Engine.Dir.RIGHT] = 0;
        wheelController.InputDirection[(int)Engine.Dir.LEFT] = 0;
    }

    // 3
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        // 1
        if (GetComponent<FixedJoint>())
        {
            // 2
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // 3
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

        }
        // 4
        objectInHand = null;
    }

}
