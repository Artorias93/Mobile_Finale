using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float m_duration, m_remainingTime;

    private int m_minutes, m_seconds;


    public float Duration
    {
        get { return m_duration; }
    }

    public float RemainingTime
    {
        get { return m_remainingTime; }
    }

    public int Minutes { get { return m_minutes; } }
    public int Seconds { get { return m_seconds; } }


    private bool m_destroyOnFinish = true, m_repeating = false, m_running;
    public bool Running
    {
        get { return m_running; }
    }


    public delegate void timerEnded();
    public event timerEnded OnTimeEnd;

    public static Timer Create(float timerDuration, timerEnded callback, bool destroyOnFinish = true, bool repeating = false, string name = "timer")
    {
        //If I have a repeating timer set to destroyOnFinish, it will not repeat

        Timer timer = new GameObject(name, typeof(Timer)).GetComponent<Timer>();

        timer.m_duration = timerDuration;
        timer.m_remainingTime = timerDuration;
        timer.m_destroyOnFinish = destroyOnFinish;
        timer.m_repeating = repeating;
        timer.m_running = false;
        timer.OnTimeEnd += callback;
        return timer;
    }

    private IEnumerator Countdown()
    {
        while(m_remainingTime > 0)
        {
            m_remainingTime -= Time.deltaTime;
            convertTime(m_remainingTime);
            yield return null;
        }

        if (OnTimeEnd != null)
        {
            OnTimeEnd();
        }

        if (m_destroyOnFinish)
            Destroy();
        else if (m_repeating) {
            Reset();
            StartCoroutine(Countdown());
        }

        m_running = m_repeating;
    }

    public void StartTimer()
    {
        if (!m_running)
        {
            m_running = true;
            StartCoroutine(Countdown());
        } else
        {
            Debug.Log("You are trying to start an already running timer");
        }
    }

    public void PauseTimer()
    {
        if (m_running)
        {
            StopAllCoroutines();
        }
    }

    public void StopTimer() 
    {
        if (m_running)
        {
            StopAllCoroutines();
            if (m_destroyOnFinish)
                Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void setDuration(float newDuration, bool reset = false)
    {
        m_duration = newDuration;
        if (reset)
            Reset();

    }

    public void Reset()
    {
        m_remainingTime = m_duration;
    }

    private void convertTime(float timeInSeconds)
    {
        m_minutes = Mathf.CeilToInt(timeInSeconds) / 60;
        m_seconds = Mathf.CeilToInt(timeInSeconds) % 60;
    }
}


