using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleShowHide : MonoBehaviour
{
    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ShowHide);
        item.SetActive(false); // hide by default
    }

    // simple button script that shows and hides thing
    private void ShowHide()
    {
        item.SetActive(!item.activeSelf);
    }
    
}
