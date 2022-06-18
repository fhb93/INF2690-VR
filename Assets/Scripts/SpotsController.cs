using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotsController : MonoBehaviour
{
    [SerializeField]
    private float SpotsAngle;

    [SerializeField]
    private float StartAngle;

    [SerializeField]
    private float MaxAngle;

    [SerializeField]
    private float MoveFactor;

    private LineRenderer[] lr;

    private WaypointLogic wplogic;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponentsInChildren<LineRenderer>();

        wplogic = GetComponentInParent<WaypointLogic>();

        SpotsAngle = StartAngle;

        foreach (LineRenderer l in lr)
        {
            l.SetPositions(new Vector3[] { Vector3.zero, new Vector3(0, StartAngle, 512) });
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(wplogic.IsReached)
        {
            for (int i = 0; i < lr.Length; i++)
            {
                lr[i].gameObject.SetActive(false);
            }
        }

        if(SpotsAngle > MaxAngle)
        {
            SpotsAngle -= Time.deltaTime * MoveFactor;
        }
        else
        {
            SpotsAngle = 0f;
        }

        for(int i = 0; i < lr.Length; i++)
        {
            lr[i].SetPosition(1, new Vector3(0, SpotsAngle, 512));
        }

    }
}
