using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSign : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.StopEvent += StartDisplay;
        this.GetComponent<Text>().color = new Color(134, 0, 255, 0); // make text not display at the start
    }

    private void StartDisplay()
    {
        this.GetComponent<Text>().color = new Color(134, 0, 255, 1); // text display when game over
        StartCoroutine(DisplayThis()); // fade out
    }
    
    IEnumerator DisplayThis()
    {
        yield return new WaitForSeconds(1);
        int secsToFade = 3;
        // loop to fade out over secsToFade
        for (float i = secsToFade; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            this.GetComponent<Text>().color = new Color(134, 0, 255, i / secsToFade);
            yield return null;
        }
    }
}
