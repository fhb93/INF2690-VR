using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneHead : MonoBehaviour
{

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name); 

        if (other.gameObject.name.Contains("Camera"))
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(MoveHeadToDisc());
            }
        }
    }

    IEnumerator MoveHeadToDisc()
    {
        transform.RotateAround(gameObject.transform.position, transform.forward, 10f); //blue then green

        yield return new WaitForSeconds(2f);

        transform.RotateAround(gameObject.transform.position, transform.up, -15f);

        yield return new WaitForSeconds(2f);

        transform.RotateAround(gameObject.transform.position, transform.forward, -10f);
    }
}

