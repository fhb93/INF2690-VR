using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircuitController : MonoBehaviour
{

    public int WaypointsCount = 3;

    [SerializeField]
    private int WaypointsLeft;

    public bool CircuitFinished { get; private set; }

    [SerializeField]
    private Image fadeOutImg;

    private void Start()
    {
        CircuitFinished = false;

        WaypointsLeft = WaypointsCount;

        fadeOutImg = GameObject.Find("FadeOutImage").GetComponent<Image>();
    }

    public void UpdateWaypointsLeft()
    {
        WaypointsLeft--;

        CheckIfFinished();
    }

    private void CheckIfFinished()
    {
        if (WaypointsLeft == 0 && CircuitFinished == false)
        {
            CircuitFinished = true;

            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float alpha = 0;

        while(fadeOutImg.color.a < 1.01f)
        {
            fadeOutImg.color = new Color(0, 0, 0, alpha);

            alpha += 0.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
