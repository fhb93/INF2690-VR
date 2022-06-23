using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for Grammophone
/// </summary>
public class SpinLever : MonoBehaviour
{
    public Transform Disc { get; private set; }

    public GameObject Lever { get; private set; }

    public Transform LeverSpinPoint { get; private set; }

    public Renderer materialRenderer { get; private set; }

    [SerializeField]
    private AudioSource audioSource;

    private bool IsTurnedOn;

    public bool UserInput;

    private float angle;

    private float angleLever;


    // Start is called before the first frame update
    void Start()
    {
        Lever = GameObject.Find("Lever3");

        LeverSpinPoint = gameObject.transform;

       // Disc = GameObject.Find("Vinyl1").transform;

        audioSource = GetComponentInParent<AudioSource>();

        materialRenderer = GameObject.Find("gramophoneLever").GetComponent<Renderer>();

        audioSource.loop = false;

        audioSource.playOnAwake = false;
    }

    
   

    IEnumerator LeverCoroutine()
    {
        if(audioSource.isPlaying)
        {
            yield return null;
        }

        int i = 0;

        while(i < 512)
        {
            angleLever += 0.5f * Time.deltaTime;

            Lever.transform.RotateAround(LeverSpinPoint.position, Lever.transform.forward, angleLever);

            i++;

            yield return new WaitForSeconds(0.01f);
        }

        audioSource.Play();

        IsTurnedOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            UserInput = true;
        }

        if (UserInput)
        {
            UserInput = false;

            StartCoroutine(LeverCoroutine());
        }

        if(IsTurnedOn)
        {
            angle += 0.5f * Time.deltaTime;

            //Disc.RotateAround(Disc.position, Disc.up, angle);

            if(audioSource.isPlaying == false)
            {
                IsTurnedOn = false;
            }
        }
    }
}
