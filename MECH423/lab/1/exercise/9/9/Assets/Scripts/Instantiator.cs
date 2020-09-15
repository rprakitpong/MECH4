using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    // Awake is called before Start
    // this makes sure that an instance of EventManager exists when any object calls it
    void Awake()
    {
        EventManager e = EventManager.Instance;
        this.gameObject.AddComponent<AudioManager>();
    }
}
