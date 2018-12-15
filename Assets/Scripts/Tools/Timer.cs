using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer {

    public string timerName;

    public float Ratio { get { return _timer / Duration; } }

    public float Duration { get; private set; }
    private bool resetTimerOnComplete;

    private Action completionCallback;
    private Action onUpdateCallback;

    private float _timer;


    public Timer(string timerName, float duration, bool resetOnComplete = false, Action completionCallback = null, Action onUpdateCallback = null) {
        this.timerName = timerName;
        this.Duration = duration;
        this.resetTimerOnComplete = resetOnComplete;

        //Debug.Log(timerName + " has a duration of " + duration);

        if (completionCallback != null)
            this.completionCallback += completionCallback;

        if (onUpdateCallback != null)
            this.onUpdateCallback += onUpdateCallback;
    }

    public void ModifyDuration(float mod) {

        Duration += mod;

        if (Duration <= 0f) {
            Duration = 0f;
        }

        if (_timer > Duration) {
            _timer = 0f;
        }
    }

    public void SetNewDuration(float duration)
    {
        Duration = duration;

        if (Duration <= 0f)
        {
            Duration = 0f;
        }

        if (_timer > Duration)
        {
            _timer = 0f;
        }
    }

    public void SetOnUpdateCallback(Action action) {
        onUpdateCallback = null;
        onUpdateCallback += action;
    }

    public void UpdateClock() {
        if (_timer < Duration) {
            _timer += Time.deltaTime;

            //Debug.Log(timerName + " Clock is updating");

            if (onUpdateCallback != null)
                onUpdateCallback();

            if (_timer >= Duration) {

                //Debug.Log(timerName + " Clock is Complete");

                if (completionCallback != null)
                    completionCallback();

                if (resetTimerOnComplete) {
                    ResetTimer();
                }
            }
        }
    }

    public void ResetTimer() {
        _timer = 0f;
    }

}


public class ComplexTimer {
    public Timer duration;
    public Timer interval;

    public bool Complete { get; private set; }

    public ComplexTimer(float totalDuration, float intervalDuration, Action onCompleteAction, Action onIntervalAction) {
        onCompleteAction += Finish;

        duration = new Timer("Total Duration", totalDuration, false, onCompleteAction);
        interval = new Timer("Interval Duration", intervalDuration, true, onIntervalAction);
    }

    public void RefreshDuration()
    {
        duration.ResetTimer();
    }

    public void ModifyDuration(float mod)
    {
        duration.ModifyDuration(mod);
    }

    public void ModifyInterval(float mod)
    {
        interval.ModifyDuration(mod);
    }

    public void UpdateClocks() {
        if(Complete == false) {
            //Debug.Log("Updating complex timer");
            duration.UpdateClock();
            interval.UpdateClock();
        }
    }

    private void Finish() {
        //Debug.Log("Finishing a complex timer");
        Complete = true;
    }

}
