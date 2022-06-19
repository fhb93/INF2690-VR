using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips = new AudioClip[5];

    [SerializeField]
    private AudioClip propellerClip;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioSource propellerAudio;

    private Engine engine;

    // Start is called before the first frame update
    void Start()
    {
        SetupGeneralSFX();

        SetupPropellerAudio();

        StartCoroutine(SFXPlayer());
    }

    private void SetupGeneralSFX()
    {
        audioSource.spatialBlend = 0.6f;

        audioSource.dopplerLevel = 0f;

        audioSource.volume = 0.125f;

        audioSource.loop = false;

        audioSource.clip = clips[Random.Range(0, clips.Length)];
    }

    private void SetupPropellerAudio()
    {
        propellerAudio.spatialBlend = 0.55f;

        propellerAudio.dopplerLevel = 0f;

        propellerAudio.volume = 0;

        propellerAudio.loop = true;

        propellerAudio.clip = propellerClip;

        engine = GetComponentInParent<Engine>();

        propellerAudio.volume = engine.EngineAudioSourceVol;

        propellerAudio.Play();
    }


    private void Update()
    {
        propellerAudio.volume = engine.EngineAudioSourceVol;
    }

    IEnumerator SFXPlayer()
    {
        while(Application.isPlaying)
        {
            if (propellerAudio.volume > 0f)
            {
                audioSource.Play();

                yield return new WaitForSecondsRealtime(audioSource.clip.length);

                audioSource.clip = clips[Random.Range(0, clips.Length)];

                float delay = Random.Range(0.5f, 11f) * 2f;

                yield return new WaitForSecondsRealtime(delay);
            }

            yield return new WaitForSecondsRealtime(0f);
        }
    }
}
