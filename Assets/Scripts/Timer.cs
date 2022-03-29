using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitNinjaMinigame
{
    public class Timer : MonoBehaviour
    {
        private float m_TimeInSeconds;

        private int m_minutes, m_seconds;


        public float TimeInSeconds
        {
            get { return m_TimeInSeconds; }
        }
        public int Minutes { get { return m_minutes; } }
        public int Seconds { get { return m_seconds; } }


        private bool m_timeEndGuard = false;


        public delegate void timerEnded();
        public static event timerEnded OnTimeEnd;

        public void Initialize(int timerDuration)
        {
            m_TimeInSeconds = timerDuration;
            m_timeEndGuard = false;
        }

        private void Update()
        {
            if (!m_timeEndGuard)
                if (m_TimeInSeconds > 0)
                {
                    m_TimeInSeconds -= Time.deltaTime;
                    convertTime(m_TimeInSeconds);
                }
                else
                {
                    m_TimeInSeconds = 0;
                    m_timeEndGuard = true;
                    if (OnTimeEnd != null)
                        OnTimeEnd();
                }
        }

        private void convertTime(float timeInSeconds)
        {
            m_minutes = Mathf.CeilToInt(timeInSeconds) / 60;
            m_seconds = Mathf.CeilToInt(timeInSeconds) % 60;

            Debug.Log(m_minutes + ":" + m_seconds);
        }
    }
}
