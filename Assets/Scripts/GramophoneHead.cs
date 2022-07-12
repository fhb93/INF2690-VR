using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneHead : MonoBehaviour
{

    private Coroutine coroutine;

    public bool IsHeadReady;

    private Color originalColor;

    private Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GameObject.Find("Head").GetComponentInChildren<Renderer>();

        originalColor = render.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name); 

        if (other.gameObject.name.Contains("vr_"))
        {
            if (coroutine == null && IsHeadReady == false)
            {
                render.material.color = Color.green;

                if (IsHeadReady == false)
                {
                    coroutine = StartCoroutine(MoveHeadToDisc());
                }
               
            }
        }
    }

    IEnumerator MoveHeadToDisc()
    {
        int angle = 0;
        int maxAngle = 30;

        while (angle < maxAngle)
        {
            transform.RotateAround(gameObject.transform.position, transform.forward, 1); //blue then green
            angle++;
            yield return new WaitForSeconds(0.01f);
        }


        yield return new WaitForSeconds(2f);

        angle = 30;
        maxAngle = 0;


        while (angle > maxAngle)
        {
            //Problema da Soma de quaternios obrigda usar vector3.up aqui
            transform.RotateAround(gameObject.transform.position, Vector3.up, -1); //blue then green

            angle--;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        angle = 30;
        maxAngle = 0;

        while (angle > maxAngle)
        {
            transform.RotateAround(gameObject.transform.position, transform.forward, -1); //blue then green
            angle--;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);

        ResetState(true);
    }



    IEnumerator MoveHeadAwayFromDisc()
    {
        int angle = 0;
        int maxAngle = 30;

        while (angle < maxAngle)
        {
            transform.RotateAround(gameObject.transform.position, transform.forward, 1); //blue then green
            angle++;
            yield return new WaitForSeconds(0.01f);
        }


        yield return new WaitForSeconds(2f);

        angle = 30;
        maxAngle = 0;


        while (angle > maxAngle)
        {
            //Problema da Soma de quaternios obrigda usar vector3.up aqui
            transform.RotateAround(gameObject.transform.position, Vector3.up, 1); //blue then green

            angle--;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        angle = 30;
        maxAngle = 0;

        while (angle > maxAngle)
        {
            transform.RotateAround(gameObject.transform.position, transform.forward, -1); //blue then green
            angle--;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);

        ResetState(false);
    }

    private void ResetState(bool isReady)
    {
        if (render.material.color == Color.green)
        {
            render.material.color = originalColor;
        }

        IsHeadReady = isReady;
    }
}

