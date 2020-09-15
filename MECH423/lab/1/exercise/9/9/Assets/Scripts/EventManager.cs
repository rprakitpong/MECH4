using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static readonly EventManager INSTANCE = new EventManager(); // use as singleton so there's only one instance of each event

    public delegate void FenceAngleDelegate(float angle);
    public event FenceAngleDelegate FenceAngleEvent; // new fence angle

    public delegate void StartDelegate();
    public event StartDelegate StartEvent; // game round start

    public delegate void PigHitFenceDelegate();
    public event PigHitFenceDelegate PigHitFenceEvent; // pig hit fence

    public delegate void StopDelegate();
    public event StopDelegate StopEvent; // game round stop / game over

    private EventManager()
    {
        //
    }

    public static EventManager Instance
    {
        get
        {
            return INSTANCE;
        }
    }

    public void PublishFenceAngleEvent(float angle)
    {
        FenceAngleEvent(angle);
    }

    public void PublishStartEvent()
    {
        StartEvent();
    }

    public void PublishPigHitFenceEvent()
    {
        PigHitFenceEvent();
    }

    public void PublishStopEvent()
    {
        StopEvent();
    }
}
