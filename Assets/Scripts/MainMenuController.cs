using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField]
    private Image[] images = new Image[2];

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private ParticleSystem[] particleSystems = new ParticleSystem[2];
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fadeout());

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit(0);
        }
    }

    IEnumerator Fadeout()
    {
        yield return new WaitForSecondsRealtime(8f);

        float alpha = 1;

        int imageIndex;

        for (imageIndex = 0; imageIndex < images.Length; imageIndex++)
        {
            while (images[imageIndex].color.a > 0)
            {
                images[imageIndex].color = new Color(1, 1, 1, alpha);

                alpha -= 2f * Time.unscaledDeltaTime;

                yield return new WaitForSecondsRealtime(0.05f);
            }

            images[imageIndex].color = Color.clear;

            alpha = 1;

            particleSystems[0].Play();

           // particleSystems[1].Play();

            yield return new WaitForSecondsRealtime(4f);
        }

        Time.timeScale = 1f;
    }
    
}
