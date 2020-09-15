using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour
{
    private float speed = 10f;
    private float speedIncrement = 1f;
    private bool move = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.StartEvent += StartPigMovement;
    }

    private void StartPigMovement()
    {
        gameObject.SetActive(true); // make pig appear
        transform.position = Vector3.zero; // set pig to initial position
        speed = 10f; // reset speed
        gameOver = false; // game not over
        move = true; // set pig to move
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameOver)
        {
            speed = speed + speedIncrement; // pig moves faster

            Vector3 localRot = transform.localRotation.eulerAngles;
            localRot.y = localRot.y + 180f + UnityEngine.Random.Range(-40f, 40f); // make pig rotate 180deg with some randomness
            transform.localRotation = Quaternion.Euler(localRot);

            EventManager.Instance.PublishPigHitFenceEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move) // if it's still set to move
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed); // make pig move forward per time and speed
        }
        if (Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.z * transform.position.z) > 30 && !gameOver) // if outside of fence and game isn't over yet
        {
            gameOver = true; // then game is now over
            EventManager.Instance.PublishStopEvent(); // publish stop event
            StartCoroutine(StopMoveIn5Secs()); // make pig run away for a bit before stopping it
        }
    }

    IEnumerator StopMoveIn5Secs()
    { 
        yield return new WaitForSeconds(5); // pig runs away for 5 secs
        move = false; // make it stop moving
        gameObject.SetActive(false); // make it disappear
    }
}
