using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceRotation : MonoBehaviour
{
    float arcLen = 20;
    float prevAngle = -999;
    float minMoveThreshold = 5 * Mathf.PI / 180; // 5 deg

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.FenceAngleEvent += SetFenceRotation; // listen to event to get new angle data
    }

    private void SetFenceRotation(float angle)
    {
        if (Mathf.Abs(angle - prevAngle) > minMoveThreshold) // only move fence if angle change is more than threshold, use for noise reduction
        {
            float y = Mathf.Cos(angle) * arcLen; // simple cos and sin calculation to get x and y position about world center
            float x = Mathf.Sin(angle) * arcLen;
            Vector3 localPos = this.transform.localPosition;
            localPos.x = x;
            localPos.z = y;
            this.transform.localPosition = localPos;

            angle = angle * (180 / Mathf.PI) + 90;
            this.transform.localRotation = Quaternion.Euler(0, angle, 0); // set rotation of fence so that it's faces world center

            //Debug.Log("x: " + x + ", y: " + y + ", angle: " + angle);
        }
    }
}
