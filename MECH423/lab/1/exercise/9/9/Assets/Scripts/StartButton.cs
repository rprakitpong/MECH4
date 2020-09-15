using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(StartGame);

        EventManager.Instance.FenceAngleEvent += CanStartGame; 
        this.gameObject.SetActive(false); // can't start game yet since there's no serial connection initially
    }

    private void CanStartGame(float angle)
    {
        this.gameObject.SetActive(true); // set active start game button only after an angle data is successfully published
        EventManager.Instance.FenceAngleEvent -= CanStartGame; // unsubscribe from event
    } 

    private void StartGame()
    {
        EventManager.Instance.PublishStartEvent(); // start a round of game
    }
}
