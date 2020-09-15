using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private IEnumerator increment;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.StartEvent += StartCount; // start counting score when game start
        EventManager.Instance.StopEvent += StopCount; // stop counting score when game over
    }

    private void StartCount()
    {
        increment = IncrementScore();
        StartCoroutine(increment); // counter is coroutine
    }

    private void StopCount()
    {
        StopCoroutine(increment); // stop counter
    }

    IEnumerator IncrementScore()
    {
        this.gameObject.GetComponent<Text>().text = "Score: 0"; // initial score is zero
        int score = 1;
        while (true)
        { 
            this.gameObject.GetComponent<Text>().text = "Score: " + score; // change score text display
            score += 1; // 1 point per 1 second survived
            yield return new WaitForSeconds(1f); // wait 1 sec
        }
    }
}
