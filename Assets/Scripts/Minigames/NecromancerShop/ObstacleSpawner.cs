using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NecromancerShop;

namespace NecromancerShop
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_target;
        [SerializeField]
        private GameObject m_obstaclePrefab;
        [SerializeField]
        private float f_speed = 2.5f;

        private bool b_canMove = false;
        private GameObject m_obstacle;
        private RectTransform m_obstacleRect;
        private RectTransform m_spawnerRect;
        private GameObject m_currentTarget;
        private int i_minSecondsStop;
        private int i_maxSecondsStop;


        // Start is called before the first frame update
        void Start()
        {
            
            m_spawnerRect = GetComponent<RectTransform>();
            m_obstacle = Instantiate(m_obstaclePrefab, m_spawnerRect.transform);
            m_obstacleRect = m_obstacle.GetComponent<RectTransform>();
            m_currentTarget = m_target;
        }

        public void InitializeSpawner(int i_minSeconds, int i_maxSeconds) {
            i_minSecondsStop = i_minSeconds;
            i_maxSecondsStop = i_maxSeconds;
            StartCoroutine(stopMovement(i_minSecondsStop, i_maxSecondsStop));
        }
        public void StopSpawner()
        {
            b_canMove = false;
        }
        // Update is called once per frame
        void Update()
        {
            if (b_canMove)
            {

                if (m_obstacleRect.transform.position == m_currentTarget.GetComponent<RectTransform>().position) {
                    if (m_obstacleRect.position == m_spawnerRect.position)
                    {
                        m_currentTarget = m_target;
                        StartCoroutine(stopMovement(i_minSecondsStop, i_maxSecondsStop));
                    }
                    else {
                        m_currentTarget = gameObject;
                        StartCoroutine(stopMovement(i_minSecondsStop/2, i_maxSecondsStop/2));
                    }

                }
               m_obstacleRect.position = Vector3.MoveTowards(m_obstacleRect.position, m_currentTarget.GetComponent<RectTransform>().position, f_speed * Time.deltaTime);
            }
        }

        private IEnumerator stopMovement(float i_minSeconds,float i_maxSeconds) {
            b_canMove = false;
            yield return new WaitForSeconds(Random.Range(i_minSeconds, i_maxSeconds));
            b_canMove = true;
        }
    }
}