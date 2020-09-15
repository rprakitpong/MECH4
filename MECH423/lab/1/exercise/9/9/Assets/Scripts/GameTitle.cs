using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour
{
    IEnumerator fade;

    // Start is called before the first frame update
    void Start()
    {
        fade = FadeOut();
        StartCoroutine(fade);
        EventManager.Instance.StartEvent += DisappearInstantly; // title should disappear instantly if use click to start before it's done fading out
    }

    private void DisappearInstantly()
    {
        StopCoroutine(fade);
        this.GetComponent<Text>().color = new Color(255, 255, 129, 0);
        EventManager.Instance.StartEvent -= DisappearInstantly; // unsubscribe from event since this has to be called only once at start
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2);
        int secsToFade = 3;
        // loop over secsToFade, slowly decreasing alpha
        for (float i = secsToFade; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            this.GetComponent<Text>().color = new Color(255, 255, 129, i / secsToFade);
            yield return null;
        }
        EventManager.Instance.StartEvent -= DisappearInstantly;
    }
}
