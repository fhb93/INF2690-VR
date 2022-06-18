using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointLogic : MonoBehaviour
{
    [SerializeField]
    private bool isReached = false;

    public bool IsReached { get { return isReached;  } }

    private CircuitController controller;

    public void WayPointIsReached()
    {
        if (isReached == false)
        {
            isReached = true;

            controller.UpdateWaypointsLeft();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<CircuitController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
