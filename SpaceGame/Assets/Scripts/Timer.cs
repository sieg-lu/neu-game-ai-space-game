﻿using UnityEngine;
using System.Collections;

public class Timer {
    //If the Timer is running 
    private bool b_Tricking;
    //Current time
    public float f_CurTime;
    //Time to reach
    private float f_TriggerTime;
    //Use delegate to hold the methods
    public delegate void EventHandler();
    //The trigger event list
    public event EventHandler tick;

    public Timer(float second) {
        f_CurTime = 0.0f;
        f_TriggerTime = second;
    }

    public void Start() {
        b_Tricking = true;
    }

    public void Update(float deltaTime) {
        if (b_Tricking) {
            f_CurTime += deltaTime;

            if (f_CurTime > f_TriggerTime) {
                //b_Tricking must set false before tick() , cause if u want to restart in the tick() , b_Tricking would be reset to fasle .
                b_Tricking = false;
                tick();
            }
        }
    }

    public void Stop() {
        b_Tricking = false;
    }

    public void Continue() {
        b_Tricking = true;
    }

    public void Restart() {
        b_Tricking = true;
        f_CurTime = 0.0f;
    }

    public void ResetTriggerTime(float second) {
        f_TriggerTime = second;
    }
}
