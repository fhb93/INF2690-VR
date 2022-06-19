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

    private SpinLever grammophone;
    private Engine wheelController;

    [SerializeField]
    private GameObject ZeppelinCockpitPlayerPos;

    private void Start()
    {
        grammophone = GameObject.Find("SpinPoint").GetComponent<SpinLever>();
        wheelController = GameObject.Find("ZeppelinCockpit").GetComponent<Engine>();
        //ZeppelinCockpitPlayerPos = GameObject.Find("PlayerPosition");
    }
    // Update is called once per frame
    void Update()
    {

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

    }

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

        collidingObject = null;
    }

    private void GrabObject()
    {
        // 1
        //objectInHand = collidingObject;
        //collidingObject = null;
        //// 2
        //var joint = AddFixedJoint();
        //joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        objectInHand = collidingObject;
        
        if (objectInHand.name != "default")
        {
            if (objectInHand.name != "Cube_Cube_Lever")
            {
                if (objectInHand.GetComponentInParent<SpinControlLever>() != null)
                {
                    objectInHand.GetComponentInParent<SpinControlLever>().UserInput = true;
                }
            }
            else
            {
                grammophone.UserInput = true;
            }

            collidingObject = null;

            objectInHand = null;

        }
        else
        {
            

            if(transform.position.x > wheelController.gameObject.transform.position.x)
            {
                wheelController.InputDirection[(int) Engine.Dir.RIGHT] = 1;
                wheelController.InputDirection[(int) Engine.Dir.LEFT] = 0;
            }
            else
            {
                wheelController.InputDirection[(int)Engine.Dir.RIGHT] = 0;
                wheelController.InputDirection[(int)Engine.Dir.LEFT] = -1;
            }
        }

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
